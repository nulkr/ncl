using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using j2k;
using System.IO;


namespace j2kTestEquipment
{
    
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            App.Init();
            EQ.Init();

            EQ.Recipe.AddFromPMAC(@"D:\WORK\2015\KLLO-7000\trunk\main\UMAC_KLLO7000\DEFINE_VAR.h");
            EQ.Alarms.AddFromPMAC(@"D:\WORK\2015\KLLO-7000\trunk\main\UMAC_KLLO7000\DEFINE_ERR.h");
            EQ.IOList.AddFromPMAC(@"D:\WORK\2015\KLLO-7000\trunk\main\UMAC_KLLO7000\DEFINE_IO.h");

            Text = App.FullName;
        }

        ~FrmMain()
        {
            EQ.Final();
        }

        public void ShowForm(Form form)
        {
            panMain.SuspendDrawing();

            panMain.HideChildForms();

            form.ShowInside(panMain);

            panMain.ResumeDrawing();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Utils.SaveObjectToJson(EQ.Recipe, "Recipe.json");
            Utils.SaveObjectToJson(EQ.Alarms, "Alarms.json");
            Utils.SaveObjectToJson(EQ.IOList, "DIOs.json");
            Utils.SaveObjectToJson(EQ.MotorList, "Motors.json");            
        }            

        private void btnLoad_Click(object sender, EventArgs e)
        {
            EQ.Recipe = Utils.LoadObjectFromJson(typeof(Recipe), "Recipe.json") as Recipe;
            EQ.Alarms = Utils.LoadObjectFromJson(typeof(Alarms), "Alarms.json") as Alarms;            
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            EQ.AutoForm.ShowInside(panMain);

        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            EQ.AutoForm.ShowInside(panMain);
        }

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            EQ.RecipeForm.ShowInside(panMain);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            App.Logger.Debug("Debug");
            App.Logger.Info("Info");
            App.Logger.Warn("Warn");
            App.Logger.Error("Error");
            App.Logger.Fatal("Fatal가나다라");

        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            new LogView(@"D:\WORK\P\j2k\j2k\test\j2kTestEquipment\j2kTestEquipment\bin\Debug\Log\Exception-16-04-05.log").Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnCommTest_Click(object sender, EventArgs e)
        {
            ComTest ComTest = new ComTest("asd", App.IniFileName);
            ComTest.Show();
            ComTest.SaveSettings("PORT.json");
        }
    }
}
