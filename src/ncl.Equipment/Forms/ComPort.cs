/// Base COM port terminal
///
/// usage :
///  1. form inheritance
///  2. override Send, {AddToListBox}
///  3. add event serialPort
///  4. add event seqMonitoring
///
using System;
using System.IO;
using System.Windows.Forms;

namespace ncl
{
    public partial class ComPort : Form
    {
        #region field

        private string _ComName;

        #endregion field

        #region property

        public bool Connected
        {
            get { return serialPort.IsOpen; }
            set
            {
                if (value)
                {
                    try
                    {
                        serialPort.Open();
                    }
                    catch (Exception e)
                    {
                        MsgBox.Error(e.Message);
                        App.Logger.Fatal(e);
                    }
                }
                else serialPort.Close();

                btnConnected.State = serialPort.IsOpen;
            }
        }

        public string IniFileName { get; set; }

        public string ComName
        {
            get { return _ComName; }
            set
            {
                _ComName = value;
                Text = "ComPort - " + _ComName;
            }
        }

        #endregion property

        #region constructor

        // 기본 생성자가 없으면 상속시 디자이너의 오류가 발생한다
        public ComPort()
        {
            InitializeComponent();

            propGrid.Visible = false;

            IniFileName = "ComPort.ini";
            ComName = GetType().Name;

            if (File.Exists(IniFileName))
                LoadSettings();
        }

        public ComPort(string comName, string iniFilename = "ComPort.ini")
        {
            InitializeComponent();

            propGrid.Visible = false;

            IniFileName = iniFilename;
            ComName = comName;

            if (File.Exists(IniFileName))
                LoadSettings();
        }

        #endregion constructor

        #region method

        public void LoadSettings()
        {
            IniFile ini = new IniFile(IniFileName);

            bool oldconn = Connected;
            Connected = false;

            ini.Read(ComName, serialPort);

            Connected = oldconn;
        }

        public void SaveSettings()
        {
            IniFile ini = new IniFile(IniFileName);

            ini.Write(ComName, serialPort);
        }

        public virtual void Send(string s)
        {
            try
            {
                AddToListBox("> " + s);
                serialPort.Write(s);
            }
            catch (Exception e)
            {
                App.Logger.Fatal(e.ToString()); // TODO : exception handling
            }
        }

        public virtual void AddToListBox(string s)
        {
            listBox.InvokeIfNeeded(() =>
                {
                    if (listBox.Items.Count > 128)
                        listBox.Items.RemoveAt(0);

                    listBox.Items.Add(s);
                });
        }

        #endregion method

        #region event

        private void btnConnected_Click(object sender, EventArgs e)
        {
            Connected = !Connected;
            btnConnected.State = Connected;
        }

        private void btnMonitoring_Click(object sender, EventArgs e)
        {
            if (seqMonitoring.SeqNo < 1)
                seqMonitoring.Start();
            else
                seqMonitoring.Abort();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (propGrid.SelectedObject == null)
            {
                propGrid.SelectedObject = serialPort;
            }

            if (propGrid.Visible)
            {
                SaveSettings();
                propGrid.Visible = false;
                Connected = true;
                btnSettings.Text = "Show Settings";
            }
            else
            {
                Connected = false;
                propGrid.Visible = true;
                btnSettings.Text = "Hide && Save Settings";
            }
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Send(cbInput.Text);

                cbInput.InvokeIfNeeded(() => cbInput.Text = "");
            }
        }

        private void ComPort_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            Hide();
        }

        #endregion event
    }
}