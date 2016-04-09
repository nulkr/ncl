using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ncl;

namespace ncl
{
    namespace Equipment
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

            #endregion

            #region constructor / destructor

            Pmac()
            {
                _DprBuffer.Initialize();

                _BufferPtr = Marshal.AllocHGlobal(BUFFER_SIZE);
            }
            ~Pmac()
            {
                Marshal.FreeHGlobal(_BufferPtr);
            }

            #endregion

            #region properties

            public bool Connected
            {
                get { return _Connected; }
            }

            #endregion

            #region Communications

            public bool Open(int nDevice = -1)
            {
                if (nDevice > -1)
                    _DeviceIndex = (uint)nDevice;

                _Connected = OpenPmacDevice(_DeviceIndex);

                return _Connected;
            }
            public void Close()
            {
                ClosePmacDevice(_DeviceIndex);
                _Connected = false;
            }
            public bool SelectDevice(IntPtr nHandle)
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
                Send(sVarName + "={0:D}", value ? 1 : 0);
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

            #region private extern PComm32.dll

            [DllImport("PComm32.dll")]
            public static extern int PmacSelect(IntPtr nHandle);

            [DllImport("PComm32.dll")]
            private static extern bool OpenPmacDevice(uint nDevice);

            [DllImport("PComm32.dll")]
            private static extern uint ClosePmacDevice(uint dwDevice);

            [DllImport("PComm32.dll", CharSet = CharSet.Ansi)]
            private static extern int PmacGetResponseA(uint dwDevice, string sCmd, uint maxchar, StringBuilder sOut);

            [DllImport("PComm32.dll", CharSet = CharSet.Ansi)]
            private static extern int PmacDownloadA(uint dwDevice, ProcDownloadMsg msgp, ProcDownloadGet getp, ProcDownloadProgress pprg, string filename, int macro, int map, int log, int dnld);

            [DllImport("PComm32.dll")]
            private static extern IntPtr PmacDPRSetMem(uint dwDevice, UInt32 offset, uint count, ref IntPtr val);

            [DllImport("PComm32.dll")]
            private static extern IntPtr PmacDPRGetMem(uint dwDevice, uint offset, uint count, ref IntPtr val);

            [DllImport("PComm32.dll", CharSet = CharSet.Ansi)]
            private static extern Int16 PmacDPRAsciiStrToRotA(uint dwevice, string sCmd, UInt16 nBufNo);

            [DllImport("PComm32.dll")]
            private static extern UInt32 PmacDPRGetDWord(uint dwevice, uint offset);
            [DllImport("PComm32.dll")]
            private static extern void PmacDPRSetDWord(uint dwevice, uint offset, uint value);
            [DllImport("PComm32.dll")]
            private static extern UInt16 PmacDPRGetWord(uint dwevice, uint offset);
            [DllImport("PComm32.dll")]
            private static extern void PmacDPRSetWord(uint dwevice, uint offset, UInt16 value);

            #endregion
        }

        public class PmacIOController : IDIOController
        {
            #region Field

            private Pmac _Pmac = null;
            private int IOCardByteSize = 2;

            public uint AddressIOX = 0;
            public uint AddressIOY = 0;
            #endregion 
            
            #region constructor

            public PmacIOController(Pmac pmac, int ioCardByteSize = 2)
            {
                _Pmac = pmac;
                IOCardByteSize = ioCardByteSize;
            }
            #endregion

            #region properties

            public Pmac Pmac { get { return _Pmac; } }
            #endregion

            #region implementation of IDIOController

            public bool Connected
            {
                get { return _Pmac.Connected; }
                set {
                    if (value) _Pmac.Open();
                    else _Pmac.Close();
                } 
            }

            public uint GetX(int cardIndex)
            {
                return _Pmac.ReadDPR32((uint)(AddressIOX + IOCardByteSize * cardIndex));
            }
            public uint GetY(int cardIndex)
            {
                return _Pmac.ReadDPR32((uint)(AddressIOY + IOCardByteSize * cardIndex));
            }
            public void SetY(int cardIndex, uint value)
            {
                _Pmac.WriteDPR32((uint)(AddressIOY + IOCardByteSize * cardIndex), value);
            }

            public bool GetBitX(int cardIndex, int bitIndex)
            { 
                return Utils.GetBit32(GetX(cardIndex), bitIndex);
            }
            public bool GetBitY(int cardIndex, int bitIndex)
            {
                return Utils.GetBit32(GetY(cardIndex), bitIndex);
            }
            public void SetBitY(int cardIndex, int bitIndex, bool state)
            {
                uint v = GetY(cardIndex);
                Utils.SetBit32(ref v, bitIndex, state);
                SetY(cardIndex, v);
            }
            public void ToggleBitY(int cardIndex, int bitIndex)
            {
                uint v = GetY(cardIndex);
                Utils.ToggleBit32(ref v, bitIndex);
                SetY(cardIndex, v);
            }
            #endregion
        }

        public class PmacController : IMotorController
        {
            #region Field

            private Pmac _Pmac = null;
            #endregion

            #region properties

            public Pmac Pmac { get { return _Pmac; } }
            #endregion

            #region constructor

            public PmacController(Pmac pmac)
            {
                _Pmac = pmac; 
            }
            #endregion

            #region implementation of IMotorController

            public bool Connected { get; set; }

            public void ReadData(MotorList motorList)
            {
            }
            public void SetServoOn(int axisNo)
            {
            }

            public double GetJogSpeed(int axisNo)
            {
                // TODO : calc
                return Convert.ToInt32(_Pmac.Send("I0x22"));
            }
            public void SetJogSpeed(int axisNo, double unitPerSec)
            {
            }
            public void SetJogSpeed(int axisNo, MotorSpeedLevel speedLevel)
            {
            }
            public void SetJogSpeed(int[] axesNo, MotorSpeedLevel speedLevel)
            {
            }

            public void Home(int axisNo)
            {
            }
            public void JogAbs(int axisNo, double unitPosAbs)
            {
            }
            public void JogAbs(int axis1, int axis2, double unitPosAbs1, double unitPosAbs2)
            {
            }
            public void JogInc(int axisNo, double unitPosInc)
            {
            }
            public void JogInc(int axis1, int axis2, double unitPosInc1, double unitPosInc2)
            {
            }
            public void JogP(int axisNo)
            {
            }
            public void JogN(int axisNo)
            {
            }
            public void JogStop(int axisNo)
            {
            }
            public void JogStop(int[] axesNo)
            {
            }
            #endregion
        }
    }
}

/*
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
                return Axis_OpenLoop(nAxisNo) ||
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
*/