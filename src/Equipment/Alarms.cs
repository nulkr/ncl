using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace ncl
{
    namespace Equipment
    {
        [DataContractAttribute()]
        public class AlarmItem
        {
            #region field

            [DataMemberAttribute(Name = "Code")]
            private int _Code;

            [DataMemberAttribute()]
            public Bitmap Bitmap = null;

            [DataMemberAttribute()]
            public string Title, Category, Text, Module;

            [DataMemberAttribute()]
            public int Severity;

            public int Code { get { return _Code; } }
            #endregion

            #region constructor

            public AlarmItem(int code, string title = "", string category = "", string txt = "", string module = "", int severity = 0, int imgIndex = -1)
            {
                _Code = code;
                Title = title;
                Category = category;
                Text = txt;
                Module = module;
                Severity = severity;
            }
            #endregion
        }

        [DataContractAttribute()]
        public class Alarms
        {
            #region constant

            public const int BUFFER_COUNT = 32;
            public const int ALARM_COUNT = 1000;

            #endregion

            #region field

            [DataMemberAttribute()]
            public readonly DataInfo DataInfo = new DataInfo(1, "ncl AlarmList");

            [DataMemberAttribute(Name = "AlarmList")]
            AlarmItem[] _Array = new AlarmItem[ALARM_COUNT];

            public volatile int[] Buffers = new int[BUFFER_COUNT];

            public Form AlarmForm = null;
            #endregion

            #region property

            public AlarmItem this[int code]
            {
                get { return _Array[code]; }
            }

            public bool AlarmExists { get { return Buffers[0] != 0; } }
            #endregion

            #region constructor

            public Alarms()
            {
                for (int i = 0; i < ALARM_COUNT; i++)
                    _Array[i] = new AlarmItem(i);

                for (int i = 0; i < BUFFER_COUNT; i++)
                    Buffers[i] = 0;
            }
            #endregion

            #region method

            public void LoadFromCSV(string filename, char seperator = '|')
            {
                foreach (AlarmItem item in _Array)
                {
                    item.Bitmap.Dispose();
                    item.Category = "";
                    item.Text = "";
                    item.Module = "";
                    item.Severity = 0;
                    item.Title = "";
                }

                using (StreamReader reader = new StreamReader(filename))
                {
                    string sLine;
                    while ((sLine = reader.ReadLine()) != null)
                    {
                        // ignore comment
                        if (sLine.IndexOf("//") == 0) continue;
                        if (sLine.IndexOf(';') == 0) continue;

                        string[] words = sLine.Split(seperator);
                        if (words.Length < 7) continue;

                        int code = Convert.ToInt32(words[0]);

                        this[code].Title = words[1];
                        this[code].Category = words[2];
                        this[code].Text = words[3];
                        this[code].Module = words[4];
                        this[code].Severity = Convert.ToInt32(words[5]);
                        this[code].Bitmap = Utils.StrToBitmap(words[6]);
                    }

                    reader.Close();
                }
            }
            public void SaveToCSV(string filename, char seperator = '|')
            {
                using (StreamWriter writer = new System.IO.StreamWriter(filename))
                {
                    writer.WriteLine("//------------------------------------------------------------------------------");
                    writer.WriteLine("// Error Code Text File");
                    writer.WriteLine("//   - Automatically created by NUL (ncl.Alarms)");
                    writer.WriteLine("//   - " + DateTime.Now.ToString("yyyy-MM-dd hh:nn:ss"));
                    writer.WriteLine("//");
                    writer.WriteLine("// Code | Title | Category | Text | Module | Severity | ImageData");
                    writer.WriteLine("//");

                    string fmt = "{0} " + seperator + " {1} " + seperator + " {2} " + seperator + " {3} " + seperator + " {4} " + seperator + " {5} " + seperator + " {6} " + seperator + " {7} " + seperator + " {8}";

                    foreach (AlarmItem item in _Array)
                    {
                        string sImg = Utils.BitmapToStr(item.Bitmap);
                        string sLine = String.Format(fmt, item.Code, item.Title, item.Category, item.Text, item.Module, item.Severity, sImg);

                        writer.WriteLine(sLine);
                    }

                    writer.Close();
                }
            }

            /// PMAC Define 파일을 Parsing 하여 Dictionary에 추가 한다. 기존 데이터와 겹치는 건 패스
            /// #define title code // category, comment
            /// <param name="filename"></param>
            /// <param name="kind"></param>
            public void AddFromPMAC(string filename, RecipeKindFlag kind = RecipeKindFlag.None)
            {
                string s;
                StreamReader fr = new StreamReader(filename, Encoding.GetEncoding("ks_c_5601-1987"), true);

                while ((s = fr.ReadLine()) != null)
                {
                    s = s.Trim();
                    if (s.Length < 1) continue;

                    // ignore comment
                    if (s.IndexOf("//") == 0) continue;
                    if (s.IndexOf(';') == 0) continue;

                    s = s.Replace("//", ",");
                    s = s.Replace("\t", "  ");

                    string[] words = s.Split(',');
                    if (words.Length < 1)
                        continue;

                    // extract #define, title, code
                    words[0] = Regex.Replace(words[0], "#define", "", RegexOptions.IgnoreCase).Trim();

                    int code = 0;
                    string[] sNameAndCode = words[0].Split(' ');
                    if (sNameAndCode.Length < 2 || !int.TryParse(sNameAndCode[sNameAndCode.Length - 1], out code) || !Utils.InRange(code, 1, ALARM_COUNT - 1) || !string.IsNullOrEmpty(_Array[code].Title))
                        continue;

                    _Array[code].Title = sNameAndCode[0];

                    // extract category, text
                    if (words.Length > 1)
                        _Array[code].Category = words[1].Trim();
                    if (words.Length > 2)
                        _Array[code].Text = words[2].Trim();
                }

                fr.Close();
            }

            public void ClearBuffers()
            {
                for (int i = 0; i < BUFFER_COUNT; i++)
                    Buffers[i] = 0;
            }
            public void AlarmOccur(int code)
            {
                for (int i = 0; i < BUFFER_COUNT; i++)
                    if (Buffers[i] == code) // aleady exists
                    {
                        AlarmForm.InvokeIfNeeded(() => { AlarmForm.Show(); AlarmForm.BringToFront(); });
                        break;
                    }
                    else if (Buffers[i] == 0) // empty buffer
                    {
                        Buffers[i] = code;
                        AlarmForm.InvokeIfNeeded(() => { AlarmForm.Hide(); AlarmForm.Show(); AlarmForm.BringToFront(); });
                        break;
                    }
            }
            #endregion
        }
    }
}
