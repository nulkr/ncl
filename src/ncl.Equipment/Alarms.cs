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
        public class AlarmItem
        {
            private int _Code;

            public string Title, Category, Text, Module, ImgFile;

            public int Severity;

            public int Code { get { return _Code; } }

            public AlarmItem(int code, string title = "", string category = "", string txt = "", string module = "", int severity = 0, string imgFile = "")
            {
                _Code = code;
                Title = title;
                Category = category;
                Text = txt;
                Module = module;
                Severity = severity;
                ImgFile = imgFile;
            }
        }

        public class Alarms
        {
            public const int BUFFER_COUNT = 32;
            public const int ALARM_COUNT = 1000;

            public readonly DataInfo DataInfo = new DataInfo(1, "ncl.Equipment.Alarms");

            public string ImgPath = "";

            AlarmItem[] _Array = new AlarmItem[ALARM_COUNT];

            public volatile int[] Buffers = new int[BUFFER_COUNT];

            public Form AlarmForm = null;

            public AlarmItem this[int code] { get { return _Array[code]; } }

            public bool AlarmExists { get { return Buffers[0] != 0; } }

            public Alarms(string imgPath = "")
            {
                for (int i = 0; i < ALARM_COUNT; i++)
                    _Array[i] = new AlarmItem(i);

                for (int i = 0; i < BUFFER_COUNT; i++)
                    Buffers[i] = 0;

                if (imgPath == "")
                    ImgPath = App.Path;
                else
                    ImgPath = imgPath;
            }

            public void LoadFromCSV(string filename, char seperator = '|')
            {
                foreach (AlarmItem item in _Array)
                {
                    item.Category = "";
                    item.Text = "";
                    item.Module = "";
                    item.Severity = 0;
                    item.Title = "";
                    item.ImgFile = "";
                }

                using (StreamReader reader = new StreamReader(filename))
                {
                    string sLine;
                    while ((sLine = reader.ReadLine()) != null)
                    {
                        sLine.Trim();

                        // ignore comment
                        if (sLine.IndexOf("//") == 0) continue;
                        if (sLine.IndexOf(';') == 0) continue;

                        string[] words = sLine.Split(seperator);
                        if (words.Length < 1) continue;

                        int code = -1;
                        if (!int.TryParse(words[0], out code) || code >= ALARM_COUNT || code < 0) continue;

                        for (int i = 1; i < words.Length; i++)
                            switch (i)
                            {
                                case 1: this[code].Title = words[1].Trim(); break;
                                case 2: this[code].Category = words[2].Trim(); break;
                                case 3: this[code].Text = words[3].Trim(); break;
                                case 4: this[code].Module = words[4].Trim(); break;
                                case 5: this[code].Severity = Convert.ToInt32(words[5]); break;
                                case 6: this[code].ImgFile = words[6].Trim(); break;
                            }
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
                    writer.WriteLine("//   - created by Nulkr (ncl.Equipment.Alarms)");
                    writer.WriteLine("//   - " + DateTime.Now.ToString("yyyy-MM-dd hh:nn:ss"));
                    writer.WriteLine("//");
                    writer.WriteLine("// Code | Title | Category | Text | Module | Severity | ImageFile");
                    writer.WriteLine("//");

                    string fmt = "{0,4:D4} " + seperator + " {1} " + seperator + " {2} " + seperator + " {3} " + seperator + " {4} " + seperator + " {5} " + seperator + " {6}";

                    foreach (AlarmItem item in _Array)
                    {
                        string sLine = String.Format(fmt, item.Code, item.Title, item.Category, item.Text, item.Module, item.Severity, item.ImgFile);

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
        }
    }
}
