/*
 * usage : 
 * 1. form inheritance
 * 2. override Send, {AddToListBox}
 * 3. add event serialPort
 * 4. add event seqMonitoring
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace ncl
{
    public partial class ComPort : Form
    {
        #region field

        private string _ComName;
        #endregion

        #region property

        public bool Connected
        {
            get { return serialPort.IsOpen;  }
            set
            {
                if (value) serialPort.Open();
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
        }
        public ComPort(string comName, string iniFileName)
        {
            InitializeComponent();

            propGrid.Visible = false;

            IniFileName = iniFileName;
            ComName = comName;

            if (File.Exists(iniFileName))
                LoadSettings(iniFileName);
        }
        #endregion

        #region method

        public void LoadSettings(string iniFilename)
        {
            Connected = false;

            IniFile ini = new IniFile(iniFilename);

            ini.Load();

            serialPort.PortName = ini.Read(ComName, "PortName", serialPort.PortName.ToString());
            serialPort.BaudRate = ini.Read(ComName, "BaudRate", serialPort.BaudRate);
            serialPort.DataBits = ini.Read(ComName, "DataBits", serialPort.DataBits);
            serialPort.Handshake = ini.Read<Handshake>(ComName, "Handshake", serialPort.Handshake);
            serialPort.Parity = ini.Read<Parity>(ComName, "Parity", serialPort.Parity);
            serialPort.StopBits = ini.Read<StopBits>(ComName, "StopBits", serialPort.StopBits);

            serialPort.DiscardNull = ini.Read(ComName, "DiscardNull", serialPort.DiscardNull);
            serialPort.DtrEnable = ini.Read(ComName, "DtrEnable", serialPort.DtrEnable);
            serialPort.ParityReplace = (byte)ini.Read(ComName, "ParityReplace", serialPort.ParityReplace);
            serialPort.ReadBufferSize = ini.Read(ComName, "ReadBufferSize", serialPort.ReadBufferSize);
            serialPort.ReadTimeout = ini.Read(ComName, "ReadTimeout", serialPort.ReadTimeout);
            serialPort.ReceivedBytesThreshold = ini.Read(ComName, "ReceivedBytesThreshold", serialPort.ReceivedBytesThreshold);
            serialPort.RtsEnable = ini.Read(ComName, "RtsEnable", serialPort.RtsEnable);
            serialPort.WriteBufferSize = ini.Read(ComName, "WriteBufferSize", serialPort.WriteBufferSize);
            serialPort.WriteTimeout = ini.Read(ComName, "WriteTimeout", serialPort.WriteTimeout);
        }
        public void SaveSettings(string iniFilename)
        {
            IniFile ini = new IniFile(iniFilename);

            ini.Write(ComName, "PortName", serialPort.PortName);
            ini.Write(ComName, "BaudRate", serialPort.BaudRate);
            ini.Write(ComName, "DataBits", serialPort.DataBits);
            ini.Write(ComName, "Handshake", serialPort.Handshake);
            ini.Write(ComName, "Parity", serialPort.Parity);
            ini.Write(ComName, "StopBits", serialPort.StopBits);

            ini.Write(ComName, "DiscardNull", serialPort.DiscardNull);
            ini.Write(ComName, "DtrEnable", serialPort.DtrEnable);
            ini.Write(ComName, "ParityReplace", serialPort.ParityReplace);
            ini.Write(ComName, "ReadBufferSize", serialPort.ReadBufferSize);
            ini.Write(ComName, "ReadTimeout", serialPort.ReadTimeout);
            ini.Write(ComName, "ReceivedBytesThreshold", serialPort.ReceivedBytesThreshold);
            ini.Write(ComName, "RtsEnable", serialPort.RtsEnable);
            ini.Write(ComName, "WriteBufferSize", serialPort.WriteBufferSize);
            ini.Write(ComName, "WriteTimeout", serialPort.WriteTimeout);

            ini.Save();
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

        #endregion

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            SaveSettings(IniFileName);

            Hide();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (propGrid.SelectedObject == null)
            {
                propGrid.SelectedObject = serialPort;
            }

            if (propGrid.Visible)
            {
                propGrid.Visible = false;
                Connected = true;
            }
            else
            {
                Connected = false;
                propGrid.Visible = true;
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
        #endregion

    }
}
