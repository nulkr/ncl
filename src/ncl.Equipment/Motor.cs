using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.IO;

namespace ncl
{
    namespace Equipment
    {
        [Flags]
        public enum MotorErrorKinds
        {
            Ok = 0x00,
            OpenLoop = 0x01,
            FollowingError = 0x02,
            AmpFault = 0x04
        }

        public class MotorItem
        {
            #region field

            private int _No = 0;

            public string Name = "";

            public double CtsPerUnit = 10000;

            public double SpeedMin = 0.001; // [unit/s]
            public double SpeedMax = 100;   // [unit/s]

            public double PosSwLimit = 0;  // [unit]
            public double NegSwLimit = 0;  // [unit]

            public volatile int CtsPosition = 0;
            public volatile int CtsTargetPosition = 0;
            public volatile float CtsSpeed = 0;

            public volatile bool IsRunning = false;
            public volatile bool IsInPosition = true;
            public volatile bool IsPosLimit = false;
            public volatile bool IsNegLimit = false;
            public volatile bool IsHomeCompleted = false;
            public volatile bool IsServoOn = false;
            public volatile MotorErrorKinds ErrorKinds = MotorErrorKinds.OpenLoop;
            #endregion

            public int No { get { return _No; } }

            public bool IsArrived
            {
                get { return IsInPosition && Utils.InRange(CtsPosition - CtsTargetPosition, -10, 10); }
            }

            #region constructor

            public MotorItem(int no)
            {
                _No = no;
                Name = string.Format("AXIS-{0:D2}", no);
            }
            #endregion

            public double GetPosition()
            {
                return CtsPosition / CtsPerUnit;
            }

            public double GetSpeed()
            {
                return CtsSpeed / CtsPerUnit;
            }
        }

        public class MotorList
        {
            public readonly DataInfo DataInfo = new DataInfo(1, "ncl.Equipment.MotorList");

            private int _MotorCount = 32;

            private MotorItem[] _Items;

            public bool Connected { get; set; }

            public MotorItem this[int no] { get { return _Items[no]; } }

            #region constructor

            public MotorList(int motorCount = 32)
            {
                _Items = new MotorItem[motorCount + 1]; // 1~64, ignore 0

                for (int i = 1; i <= motorCount; i++)
                    _Items[i] = new MotorItem(i);

                _MotorCount = motorCount;
            }
            #endregion

            #region method
            
            public bool IsArrived(int axis1, int axis2)
            {
                return _Items[axis1].IsArrived && _Items[axis2].IsArrived;
            }
            public bool IsArrived(int[] axes)
            {
                foreach (int i in axes)
                    if (!_Items[i].IsArrived)
                        return false;
                return true;
            }

            // CSV 파일을 Parsing 하여 데이터 목록을 얻고 Items에 적용 한다.
            public void LoadFromCsv(string filename, char seperator = ',')
            {
                string sLine;
                using (var fr = new StreamReader(filename))
                    while ((sLine = fr.ReadLine()) != null)
                    {
                        sLine.Trim();
                        if (string.IsNullOrEmpty(sLine)) continue;

                        string[] words = sLine.Split(seperator);

                        // check no & name & cts/unit
                        if (words.Length < 3)
                        {
                            continue;
                        }

                        int no;
                        if (!int.TryParse(words[0].Trim(), out no) || !Utils.InRange(no, 1, _MotorCount))
                        {
                            continue;
                        }

                        double ctsPerUnit = 0;
                        
                        if (!double.TryParse(words[2].Trim(), out ctsPerUnit))
                        {
                            MsgBox.Error("MotorList Invalid cts/unit - \n" + sLine);
                            continue;
                        }

                        _Items[no].Name = words[1].Trim();
                        _Items[no].CtsPerUnit = ctsPerUnit;

                        for (int i = 3; i < words.Length; i++)
                        {
                            switch (i)
                            {
                                case 3: double.TryParse(words[i].Trim(), out _Items[no].SpeedMin); break;
                                case 4: double.TryParse(words[i].Trim(), out _Items[no].SpeedMax); break;
                                case 5: double.TryParse(words[i].Trim(), out _Items[no].PosSwLimit); break;
                                case 6: double.TryParse(words[i].Trim(), out _Items[no].NegSwLimit); break;
                            }
                        }
                    }
            }

            // 데이터 목록을 CSV 파일에 쓴다
            public void SaveToCsv(string filename, char seperator = '|')
            {
                using (var w = new StreamWriter(filename, false))
                {
                    w.WriteLine("------------------------------------------------------------------------------------------------------------");
                    w.WriteLine(DataInfo.ToString());
                    w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    w.WriteLine("------------------------------------------------------------------------------------------------------------");
                    w.WriteLine("No | Name                           |    cts/unit     | Min Speed | Max Speed | SW Limit (+) | SW Limit (-) ");
                    w.WriteLine("------------------------------------------------------------------------------------------------------------");
                    string fmt = "{0:D2} " + seperator + " {1,-30} " + seperator + " {2,15:0.000000} " + seperator + " {3,9} " + seperator + " {4,9} " + seperator + " {5,12} " + seperator + " {6,12}";

                    for (int i = 1; i < _MotorCount; i++)
                        w.WriteLine(string.Format(fmt, _Items[i].No, _Items[i].Name, _Items[i].CtsPerUnit, _Items[i].SpeedMin, _Items[i].SpeedMax, _Items[i].PosSwLimit, _Items[i].NegSwLimit));
                }
            }

            #endregion
        }

        public interface IMotorController
        {
            #region property

            bool Connected { get; set; }
            #endregion

            #region method

            void ReadData(MotorList motorList);
            void SetServoOn(int axisNo);

            double GetJogSpeed(int axisNo);
            void SetJogSpeed(int axisNo, double unitPerSec);
            void SetJogSpeedPercent(int axisNo, double speedPercent);
            void SetJogSpeedPercent(int[] axesNo, double[] speedPercent);

            void Home(int axisNo);
            void JogAbs(int axisNo, double unitPosAbs);
            void JogAbs(int axis1, int axis2, double unitPosAbs1, double unitPosAbs2);
            void JogInc(int axisNo, double unitPosInc);
            void JogInc(int axis1, int axis2, double unitPosInc1, double unitPosInc2);
            void JogP(int axisNo);
            void JogN(int axisNo);
            void JogStop(int axisNo);
            void JogStop(int[] axesNo);
            #endregion
        }
    }
}

