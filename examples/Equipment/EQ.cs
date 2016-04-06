using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using j2k;

namespace j2kTestEquipment
{
    public static class EQ
    {
        public volatile static EquipmentMode Mode = EquipmentMode.StandBy;
        public volatile static EquipmentStatus Status = EquipmentStatus.Idle;
        public volatile static EquipmentAuthority Authority = EquipmentAuthority.Operator;

        public static readonly IDIOController IOController = null;
        public static readonly IMotorController MotorController = null;

        public static DIOList IOList = new DIOList();
        public static MotorList MotorList = new MotorList();
        public static Recipe Recipe = new Recipe();
        public static Alarms Alarms = new Alarms();

        public static Sequence MainSeq = new Sequence();

        // create on Program.cs
        public static FrmMain MainForm = null; 

        public static FrmAlarm AlarmForm = null;
        public static FrmAuto AutoForm = null;
        public static FrmRecipe RecipeForm = null;

        public static void Init()
        {
            MainSeq.WorkerReportsProgress = false;
            MainSeq.OnWork += MainSeqWork;

            AutoForm = new FrmAuto();
            RecipeForm = new FrmRecipe();
            AlarmForm = new FrmAlarm();

            Alarms.AlarmForm = AlarmForm;

            try
            {
                // 1st Read I/O
                for (int i = 0; i < IOList.CardCount; i++)
                {
                    IOList.XDatas[i] = IOController.GetX(i);
                    IOList.YDatas[i] = IOController.GetY(i);
                }
            }
            catch (Exception e)
            {
                App.Logger.Fatal(e);
            }

            MainSeq.Start();
        }

        public static void Final()
        {
            MainSeq.Abort();
        }

        private static void MainSeqWork(object sender, DoWorkEventArgs e)
        {
           
            // Read I/O
            for (int i = 0; i < IOList.CardCount; i++)
            {
                IOList.XDatas[i] = IOController.GetX(i);
            }

            // Write I/O
            if (IOList.NeedWriting)
            {
                for (int i = 0; i < IOList.CardCount; i++)
                    IOController.SetY(i, IOList.YDatas[i]);

                IOList.NeedWriting = false;
            }

            // MotoList
            MotorController.ReadData(MotorList);

            // Alarm
            // TODO :

            
        }
    }
}
