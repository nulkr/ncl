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

        public enum MotorSpeedLevel { Low, Mid, High, Max }

        [DataContractAttribute()]
        public class MotorItem
        {
            #region field

            [DataMemberAttribute(Name = "No")]
            private int _No = 0;

            [DataMemberAttribute()]
            public string Name = "";

            [DataMemberAttribute()]
            public int PosSwLimit = 0;  // [unit]

            [DataMemberAttribute()]
            public int NegSwLimit = 0;  // [unit]

            [DataMemberAttribute()]
            public double CtsPerUnit = 10000;

            [DataMemberAttribute()]
            public double[] SpeedSet = new double[Enum.GetValues(typeof(MotorSpeedLevel)).Length];

            public volatile int CtsPosition = 0;
            public volatile int CtsTargetPosition = 0;
            public volatile float CtsSpeed = 0;

            public bool IsRunning = false;
            public bool IsInPosition = true;
            public bool IsPosLimit = false;
            public bool IsNegLimit = false;
            public bool IsHomeCompleted = false;
            public bool IsServoOn = false;
            public MotorErrorKinds ErrorKinds = MotorErrorKinds.OpenLoop;
            #endregion

            #region property

            public int No { get { return _No; } }

            public bool IsArrived
            {
                get { return IsInPosition && Utils.InRange(CtsPosition - CtsTargetPosition, -10, 10); }
            }
            #endregion

            #region constructor

            public MotorItem(int no)
            {
                _No = no;
                Name = string.Format("{0:##}", no);
            }
            #endregion

            #region method

            public double GetPosition()
            {
                return CtsPosition / CtsPerUnit;
            }

            public double GetSpeed()
            {
                return CtsSpeed / CtsPerUnit;
            }
            #endregion
        }

        [DataContractAttribute()]
        public class MotorList
        {
            const int MAX_MOTORS = 64;

            #region field

            [DataMemberAttribute()]
            public readonly DataInfo DataInfo = new DataInfo(1, "ncl MotorList");

            [DataMemberAttribute()]
            public MotorItem[] Items;
            #endregion

            #region property

            public bool Connected { get; set; }
            #endregion

            #region constructor

            public MotorList(int motorCount = MAX_MOTORS)
            {
                Items = new MotorItem[motorCount + 1]; // 1~64, ignore 0

                for (int i = 1; i <= motorCount; i++)
                    Items[i] = new MotorItem(i);
            }
            #endregion

            #region method
            
            public bool IsArrived(int axis1, int axis2)
            {
                return Items[axis1].IsArrived && Items[axis2].IsArrived;
            }
            public bool IsArrived(int[] axes)
            {
                foreach (int i in axes)
                    if (!Items[i].IsArrived)
                        return false;
                return true;
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
            void SetJogSpeed(int axisNo, MotorSpeedLevel speedLevel);
            void SetJogSpeed(int[] axesNo, MotorSpeedLevel speedLevel);

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

