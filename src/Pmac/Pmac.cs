using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ncl;

namespace ncl
{
    public class Pmac
    {
        #region types

        public enum BinRotResult { Ok, Busy, Error }

        [StructLayout(LayoutKind.Sequential, Pack = 32)]
        struct AxisStatus
        {
            internal volatile Int32 ActPos;
            internal volatile float ActVel;
            internal volatile UInt32 SrvSts;
            internal volatile UInt32 GnlSts;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 16)]
        struct DprBuffer
        {
            internal UInt32[] IOCard;
            internal AxisStatus[] AxisSts;
            internal Int16[] ErrCode;
            internal Int32[] UserVar;

            internal void Initialize()
            {
                IOCard = new UInt32[32];
                AxisSts = new AxisStatus[32];
                ErrCode = new Int16[40];
                UserVar = new Int32[70];
            }
        }

        public struct AxisInfo
        {
            public double CtsPerUnit;
            public string UnitName;
            public string Name;
        }
        
        public delegate void ProcDownloadMsg(String str, Int32 newline);
        public delegate void ProcDownloadGet(Int32 nIndex, String lpszBuffer, Int32 nMaxLength);
        public delegate void ProcDownloadProgress(Int32 nPercent);
        public delegate void ProcDownloadError(String fname, Int32 err, Int32 line, String szLine);

        #endregion

        #region constant

        // dpram buffer size
        const int BUFFER_SIZE = 1000;   // sizeof(DprBuffer)

        // dpram address
        const uint ADDR_IO = 0xC000; // DP:$063000
        const uint ADDR_IO_OUTPUT = 0xC040; //DP:$063010
        const uint ADDR_AXIS_STATUS = 0xC080; // DP:$063020
        const uint ADDR_ERROR_CODE = 0xC280; // DP:$0630A0
        const uint ADDR_USER_VAR = 0xC2D0; // DP:$0630B4

        // axis servo status bit
        const uint SERVO_ACTIVATE = 0x800000;
        const uint NEGATIVE_LIMIT = 0x400000;
        const uint POSITIVE_LIMIT = 0x200000;
        const uint AMP_ENABLED = 0x080000;
        const uint OPEN_LOOP = 0x040000;
        const uint MOTOR_RUNNING = 0x020000;
        const uint INTEGRATION_MODE = 0x010000;
        const uint DWELL_IN_PROGRESS = 0x008000;
        const uint DATA_BLOCK_ERROR = 0x004000;
        const uint DESIRED_VEL_ZERO = 0x002000;
        const uint ABORT_DECELERATION = 0x001000;
        const uint BLOCK_REQUEST = 0x000800;
        const uint SEARCHING_HOME = 0x000400;
        const uint FOLLOWING_ENABLED = 0x000010;

        const uint ANY_LIMIT = NEGATIVE_LIMIT + POSITIVE_LIMIT;

        // axis general status bit
        const uint STOP_ON_LIMIT = 0x000800;
        const uint HOME_COMPLETED = 0x000400;
        const uint PHASING_SEARCH_ERROR = 0x000100;
        const uint TRIGGER_MODE = 0x000080;
        const uint INTEGRATED_FOLLOW_ERROR = 0x000040;
        const uint I2TAMP_FAULT_ERROR = 0x000020;
        const uint BACKLASH_DIR_FLAG = 0x000010;
        const uint AMP_FAULT_ERROR = 0x000008;
        const uint FATAL_FOLLOW_ERROR = 0x000004;
        const uint WARN_FOLLOW_ERROR = 0x000002;
        const uint IN_POSITION = 0x000001;

        #endregion

        #region variables

        private bool _Connected = false;
        private UInt32 _DeviceIndex = 0;
        private DprBuffer _DprBuffer = new DprBuffer();
        private IntPtr _BufferPtr;
        private int _MonitoringInterval = 50;
        private System.Threading.Timer _MonitoringTimer = null;
        private AxisInfo[] _AxisInfos = new AxisInfo[32]; 

        #endregion

        #region constructor / destructor

        Pmac()
        {
            _DprBuffer.Initialize();

            _BufferPtr = Marshal.AllocHGlobal(BUFFER_SIZE);
        }
        ~Pmac()
        {
            _MonitoringTimer.Dispose();

            Marshal.FreeHGlobal(_BufferPtr);
        }

        #endregion

        #region properties

        public bool Connected 
        { 
            get { return _Connected; } 
        }
        public int MonitoringInterval 
        { 
            get { return _MonitoringInterval; } 
            set { _MonitoringInterval = value; } 
        }
        public bool Monitoring 
        {
            get { return _MonitoringTimer != null; } 
            set 
            {
                if (_MonitoringTimer == null)
                {
                    System.Threading.TimerCallback cb = (obj) =>
                    {
                        ReadDPR(ADDR_IO, BUFFER_SIZE, _BufferPtr);
                        Marshal.PtrToStructure(_BufferPtr, _DprBuffer);
                    };
  
                    _MonitoringTimer = new System.Threading.Timer(cb, this, 50, _MonitoringInterval);
                }
                else
                {
                    _MonitoringTimer.Dispose();
                    _MonitoringTimer = null;
                }
            }
        }

        #endregion

        #region Communications

        public bool Open(UInt32 nDevice)
        {
            _DeviceIndex = nDevice;
            _Connected = OpenPmacDevice(nDevice);
            return _Connected;
        }
        public void Close()
        {
            ClosePmacDevice(_DeviceIndex);
            _Connected = false;
        }
        public bool SelectDevice(int nHandle)
        {
            int n = PmacSelect(nHandle);

            if (n >= 0 && n <= 7)
            {
                _DeviceIndex = (UInt32)n;
                return true;
            }
            else
                return false;          
        }
        public string Send(string S)
        {
            StringBuilder sbResult = new StringBuilder(1024);
            
            PmacGetResponseA(_DeviceIndex, S, 1024, sbResult);
            
            return sbResult.ToString();
        }
        public string Send(string sFormat, params object[] args)
        {
            return Send(String.Format(sFormat, args));
        }
        public void WriteVar(string sVarName, double value)
        {
            Send(sVarName + "={0:F5}", value);
        }
        public void WriteVar(string sVarName, int value)
        {
            Send(sVarName + "={0:D}", value);
        }
        public void WriteVar(string sVarName, bool value)
        {
            Send(sVarName + "={0:D}", value ? 1: 0);
        }
        public void ReadDPR(UInt32 nOffset, UInt32 nCount, IntPtr value)
        {
            PmacDPRGetMem(_DeviceIndex, nOffset, nCount, ref value);
        }
        public void WriteDPR(UInt32 nOffset, UInt32 nCount, IntPtr value)
        {
            PmacDPRSetMem(_DeviceIndex, nOffset, nCount, ref value);
        }
        public UInt32 ReadDPR32(UInt32 nOffset)
        {
            return PmacDPRGetDWord(_DeviceIndex, nOffset);
        }
        public void WriteDPR32(UInt32 nOffset, UInt32 value)
        {
            PmacDPRSetDWord(_DeviceIndex, nOffset, value);
        }
        /// <summary>
        /// BIN ROT 에 명령을 보냄
        /// </summary>
        /// <param name="S"></param>
        /// <param name="nBufIndex">0 = 1번 좌표계</param>
        /// <returns>
        /// 2 EOF
        /// -59 Unable to allocate memory"
        /// -60 Unable to pack floating point number
        /// -61 Unable to convert string to float number
        /// -62 Illegal Command or Format in string
        /// -63 Integer number out of range
        /// -70: command syntax error
        /// </returns>
        public BinRotResult SendBinRot(string S, UInt16 nBufIndex)
        {
            Int16 nRes = PmacDPRAsciiStrToRotA(_DeviceIndex, S, nBufIndex);

            switch (nRes)
            {
                case 0: return BinRotResult.Ok;
                case 1: return BinRotResult.Busy;
            }

            return BinRotResult.Error;
        }

        #endregion

        #region jog

        public void JogAbs(int nAxisNo, double posUnit)
        {
            Send("#{0:D}J={1:F5}", nAxisNo, posUnit * GetAxisInfo(nAxisNo).CtsPerUnit);
        }
        public void JogAbs(int nAxisNo1, int nAxisNo2, double posUnit1, double posUnit2)
        {
            Send("#{0:D}J={1:F5} #{2:D}J={3:F5}", nAxisNo1, posUnit1 * GetAxisInfo(nAxisNo1).CtsPerUnit, nAxisNo2, posUnit2 * GetAxisInfo(nAxisNo2).CtsPerUnit);
        }
        public void JogAbs(int[] nAxesNo, double[] posUnit)
        {
            string s = "";
            for (int i = 0; i < nAxesNo.Length; i++)
                s += String.Format("#{0:D}J={1:F5}", nAxesNo[i], posUnit[i] * GetAxisInfo(nAxesNo[i]).CtsPerUnit);

            Send(s);
        }
        public void JogInc(int nAxisNo, double posUnit)
        {
            Send("#{0:D}J:{1:F5}", nAxisNo, posUnit * GetAxisInfo(nAxisNo).CtsPerUnit);
        }
        public void JogStop(int nAxisNo)
        {
            Send("#{0:D}J/", nAxisNo);
        }
        public void JogStop(params int[] nAxesNo)
        {
            string s = "";
            foreach (int n in nAxesNo)
                s += String.Format("#{0:D}J/", n);
            Send(s);
        }

        #endregion

        #region Axis

        public AxisInfo GetAxisInfo(int nAxisNo)
        {
            // 1..32
            return _AxisInfos[nAxisNo - 1];
        }

        public double Axis_Position(int nAxisNo)
        {
            return _DprBuffer.AxisSts[nAxisNo - 1].ActPos * GetAxisInfo(nAxisNo).CtsPerUnit;
        }
        public double Axis_Speed(int nAxisNo)
        {
            return _DprBuffer.AxisSts[nAxisNo - 1].ActVel * GetAxisInfo(nAxisNo).CtsPerUnit;
        }

        public bool Axis_OpenLoop(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].SrvSts & OPEN_LOOP) > 0;
        }
        public bool Axis_AmpEnabled(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].SrvSts & AMP_ENABLED) > 0;
        }
        public bool Axis_AmpFault(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].GnlSts & AMP_FAULT_ERROR) > 0;
        }
        public bool Axis_FolError(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].GnlSts & FATAL_FOLLOW_ERROR) > 0;
        }
        public bool Axis_PhaseSearchErr(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].GnlSts & PHASING_SEARCH_ERROR) > 0;
        }
        public bool Axis_IntFolErr(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].GnlSts & INTEGRATED_FOLLOW_ERROR) > 0;
        }
        public bool Axis_I2TFaultErr(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].GnlSts & I2TAMP_FAULT_ERROR) > 0;
        }
        public bool Axis_LimitP(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].SrvSts & POSITIVE_LIMIT) > 0;
        }
        public bool Axis_LimitN(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].SrvSts & NEGATIVE_LIMIT) > 0;
        }
        public bool Axis_AnyLimit(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].SrvSts & ANY_LIMIT) > 0;
        }
        public bool Axis_AnyLimit(params int[] nAxes)
        {
            bool bRes = false;
            foreach (int n in nAxes)
                bRes |= Axis_AnyLimit(n);

            return bRes;
        }
        public bool Axis_AnyError(int nAxisNo)
        {
            return  Axis_OpenLoop(nAxisNo) ||
                    !Axis_AmpEnabled(nAxisNo) ||
                    Axis_AmpFault(nAxisNo) ||
                    Axis_FolError(nAxisNo) ||
                    Axis_PhaseSearchErr(nAxisNo) ||
                    Axis_IntFolErr(nAxisNo) ||
                    Axis_I2TFaultErr(nAxisNo);

        }
        public bool Axis_AnyError(params int[] nAxes)
        {
            bool bRes = false;
            foreach (int n in nAxes)
                bRes |= Axis_AnyError(n);

            return bRes;
        }
        public bool Axis_InPos(int nAxisNo)
        {
            return (_DprBuffer.AxisSts[nAxisNo - 1].GnlSts & IN_POSITION) > 0 &&
                   (_DprBuffer.AxisSts[nAxisNo - 1].SrvSts & MOTOR_RUNNING) == 0;
        }
        public bool Axis_InPos(params int[] nAxes)
        {
            bool bRes = true;
            foreach (int n in nAxes)
                bRes &= Axis_InPos(n);

            return bRes;
        }

        #endregion

        #region IO

        public bool GetIO(int nCardIndex, int nBitIndex)
        {
            return (_DprBuffer.IOCard[nCardIndex] & (1 << nBitIndex)) > 0;
        }
        public void WriteIO(int nCardIndex, int nBitIndex, bool value)
        {
            if (value) 
                _DprBuffer.IOCard[nCardIndex] |= ((uint)1 << nBitIndex);
            else
                _DprBuffer.IOCard[nCardIndex] &= (((uint)1 << nBitIndex) ^ 0xFFFFFFFF);

            WriteDPR32(ADDR_IO_OUTPUT + ((uint)nCardIndex << 2), _DprBuffer.IOCard[nCardIndex]);
        }
        public void ToggleIO(int nCardIndex, int nBitIndex)
        {
            _DprBuffer.IOCard[nCardIndex] ^= ((uint)1 << nBitIndex); // xor
            
            WriteDPR32(ADDR_IO_OUTPUT + ((uint)nCardIndex << 2), _DprBuffer.IOCard[nCardIndex]);
        }

        #endregion

        #region error code

        public int GetErrorCode(int nIndex)
        {
            return _DprBuffer.ErrCode[nIndex];
        }

        #endregion

        #region user var

        public int GetUserVar(int nIndex)
        {
            return _DprBuffer.UserVar[nIndex];
        }
        public void WriteUserVar(int nIndex, int value)
        {
            _DprBuffer.UserVar[nIndex] = value;

            WriteDPR32(ADDR_USER_VAR + (uint)nIndex, (uint)value);
        }

        #endregion

        #region etc

        public void LoadSettings(string sFileName)
        {
            IniFile ini = new IniFile(sFileName);

            ini.Load();

            for (int i = 0; i < 32; i++)
            {
                // INI Sample is below
                //
                // [PMAC]
                // 01=AXIS-X,1000.0,mm
                // 02=AXIS-Y,1000.0,mm
                // ...
                string sLine = ini.Read("PMAC", String.Format("{0:00}", i + 1), String.Format("AXIS-{0:00},1.0,mm", i + 1));

                string[] words = sLine.Split(',');
                if (words.Length > 2)
                {
                    _AxisInfos[i].Name = words[0].Trim();
                    _AxisInfos[i].UnitName = words[2].Trim();
                    _AxisInfos[i].CtsPerUnit = Convert.ToDouble(words[2].Trim());
                }
            }
        }

        #endregion

        #region private extern PComm32.dll

        [DllImport("PComm32.dll")]
        public static extern Int32 PmacSelect(Int32 nHandle);

        [DllImport("PComm32.dll")]
        private static extern bool OpenPmacDevice(UInt32 nDevice);

        [DllImport("PComm32.dll")]
        private static extern UInt32 ClosePmacDevice(UInt32 dwDevice);

        [DllImport("PComm32.dll")]
        private static extern Int32 PmacGetResponseA(UInt32 dwDevice, string sCmd, UInt32 maxchar, StringBuilder sOut);

        [DllImport("PComm32.dll")]
        private static extern Int32 PmacDownloadA(UInt32 dwDevice, ProcDownloadMsg msgp, ProcDownloadGet getp, ProcDownloadProgress pprg, string filename, Int32 macro, Int32 map, Int32 log, Int32 dnld);

        [DllImport("PComm32.dll")]
        private static extern IntPtr PmacDPRSetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, ref IntPtr val);

        [DllImport("PComm32.dll")]
        private static extern IntPtr PmacDPRGetMem(UInt32 dwDevice, UInt32 offset, UInt32 count, ref IntPtr val);
        
        [DllImport("PComm32.dll")]
        private static extern Int16 PmacDPRAsciiStrToRotA(UInt32 dwevice, string sCmd, UInt16 nBufNo);

        [DllImport("PComm32.dll")]
        private static extern UInt32 PmacDPRGetDWord(UInt32 dwevice, UInt32 offset);
        [DllImport("PComm32.dll")]
        private static extern void PmacDPRSetDWord(UInt32 dwevice, UInt32 offset, UInt32 value);
        [DllImport("PComm32.dll")]
        private static extern UInt16 PmacDPRGetWord(UInt32 dwevice, UInt32 offset);
        [DllImport("PComm32.dll")]
        private static extern void PmacDPRSetWord(UInt32 dwevice, UInt32 offset, UInt16 value);

        #endregion
    }
}
