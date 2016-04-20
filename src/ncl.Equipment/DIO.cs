using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ncl
{
    namespace Equipment
    {
        public class DIOItem
        {
            public int CardIndex = 0;
            public int BitIndex = 0;
            public string Text = "";
            public string Category = "";
            public string Comment = "";
        }

        // XDatas, YDatas array 의 값은 별도의 쓰레드에서 한번에 읽고 쓰자
        public class DIOList
        {
            public readonly DataInfo DataInfo = new DataInfo(1, "ncl.Equipment.DIOs");

            public volatile bool NeedWriting = false;

            private int _CardCount = 32;
            private int _BitCount = 32;

            public Dictionary<string, DIOItem> XItems = new Dictionary<string, DIOItem>();
            public Dictionary<string, DIOItem> YItems = new Dictionary<string, DIOItem>();

            public volatile UInt32[] XDatas = null;
            public volatile UInt32[] YDatas = null;

            public int CardCount { get { return _CardCount; } }
            public int BitCount { get { return _BitCount; } }

            #region constructor

            public DIOList(int cardCount = 32, int bitCount = 16)
            {
                _CardCount = cardCount;
                _BitCount = bitCount;

                XDatas = new UInt32[cardCount];
                YDatas = new UInt32[cardCount];

                for (int c = 0; c < cardCount; c++)
                {
                    for (int bit = 0; bit < bitCount; bit++)
                    {
                        DIOItem xIO = new DIOItem();
                        xIO.CardIndex = c;
                        xIO.BitIndex = bit;
                        xIO.Category = "X";

                        DIOItem yIO = new DIOItem();
                        yIO.CardIndex = c;
                        yIO.BitIndex = bit;
                        yIO.Category = "Y";

                        XItems.Add(string.Format("iox_{0}", bit + c * 100), xIO);
                        YItems.Add(string.Format("ioy_{0}", bit + c * 100), yIO);
                    }
                }
            }
            #endregion

            #region method

            public bool GetBitX(string key)
            {
                var v = XItems[key];
                return Utils.GetBit32(XDatas[v.CardIndex], v.BitIndex);
            }
            public bool GetBitY(string key)
            {
                var v = YItems[key];
                return Utils.GetBit32(YDatas[v.CardIndex], v.BitIndex);
            }
            public void SetBitY(string key, bool state)
            {
                var v = YItems[key];
                Utils.SetBit32(ref YDatas[v.CardIndex], v.BitIndex, state);
                NeedWriting = true;
            }
            public void ToggleBitY(string key)
            {
                var v = YItems[key];
                Utils.ToggleBit32(ref YDatas[v.CardIndex], v.BitIndex);
                NeedWriting = true;
            }

            // #define Defined-Name Var // Category, Comment 의 구조
            public void AddFromPMAC(string filename)
            {
                string s;
                using (StreamReader fr = new StreamReader(filename, Encoding.GetEncoding("ks_c_5601-1987"), true))
                {
                    while ((s = fr.ReadLine()) != null)
                    {
                        s = s.Trim();
                        if (s.Length < 1) continue;

                        // ignore comment
                        if (s.IndexOf("//") == 0) continue;
                        if (s.IndexOf(';') == 0) continue;

                        // make comma text
                        s = s.Replace("//", ",");
                        s = s.Replace("\t", "  ");

                        // split
                        string[] words = s.Split(',');
                        if (words.Length < 1)
                            continue;

                        int no = -1;

                        words[0] = Regex.Replace(words[0], "#define", "", RegexOptions.IgnoreCase).Trim();

                        string[] sNameAndText = words[0].Split(' ');

                        // check name and m-var
                        if (sNameAndText.Length < 2 || string.IsNullOrEmpty(sNameAndText[sNameAndText.Length - 1]))
                            continue;

                        // check m-var
                        string mVar = sNameAndText[sNameAndText.Length - 1].ToUpper();
                        if (mVar.IndexOf('M') != 0)
                            continue;

                        // check m-var no
                        if (!int.TryParse(mVar.Substring(1), out no) || !Utils.InRange(no, 0, 31))
                            continue;

                        DIOItem item = new DIOItem();
                        string key = sNameAndText[0];

                        // 32 개씩 잘랐을 경우 no /100 해야 함
                        if (_BitCount == 32)
                            item.CardIndex = no / 200; 
                        else
                            item.CardIndex = no / 100;

                        item.BitIndex = no % 50;

                        if (words.Length > 1)
                            item.Category = words[1].Trim();
                        if (words.Length > 2)
                            item.Comment = words[2].Trim();

                        // TODO : Check exists key
                        if ((no % 100) < 50)
                            XItems.Add(key, item);
                        else
                            YItems.Add(key, item);
                    }

                    fr.Close();
                }
            }

            public void LoadCsvSchema(string filename, char seperator = '|')
            {
                XItems.Clear();
                YItems.Clear();

                using (StreamReader r = new StreamReader(filename))
                {
                    string sLine;
                    while ((sLine = r.ReadLine()) != null)
                    {
                        sLine.Trim();
                        if (string.IsNullOrEmpty(sLine)) continue;

                        // check X, Y
                        bool isX = sLine.IndexOf('X') == 0 || sLine.IndexOf('x') == 0;
                        bool isY = sLine.IndexOf('Y') == 0 || sLine.IndexOf('y') == 0;

                        if (!isX && !isY) continue;

                        string[] words = sLine.Split(seperator);

                        // check no & name
                        if (words.Length < 2)
                        {
                            MsgBox.Error("IOList Parsing Error - \n" + sLine);
                            continue;
                        }

                        int no;
                        if (!int.TryParse(words[0].Substring(1, words[0].Length - 1).Trim(), out no))
                        {
                            MsgBox.Error("IOList Parsing Error - \n" + sLine);
                            continue;
                        }

                        int cardIndex = no / 100;
                        int bitIndex = no % 100;

                        if (!Utils.InRange(cardIndex, 0, _CardCount - 1) || !Utils.InRange(bitIndex, 0, _BitCount - 1))
                        {
                            MsgBox.Error("IOList Invalid NO - \n" + sLine);
                            continue;
                        }

                        DIOItem item = new DIOItem();
                        item.CardIndex = cardIndex;
                        item.BitIndex = bitIndex;
                        item.Text = words[1].Trim(); // Text 지정이 안되었으면 Name을 사용

                        string key = item.Text;

                        for (int i = 2; i < words.Length; i++)
                            switch(i)
                            {
                                case 2: item.Text = words[i].Trim(); break;
                                case 3: item.Category = words[i].Trim(); break;
                                case 4: item.Comment = words[i].Trim(); break;
                            }

                        if (isX)
                            XItems.Add(key, item);
                        else
                            YItems.Add(key, item);
                    }
                }
            }
            public void SaveCsvSchema(string filename, char seperator = '|')
            {
                using (StreamWriter w = new System.IO.StreamWriter(filename))
                {
                    w.WriteLine("----------------------------------------------------------------");
                    w.WriteLine("DIO List File");
                    w.WriteLine("  - created by Nulkr (ncl.Equipment.DIOs)");
                    w.WriteLine("  - " + DateTime.Now.ToString("yyyy-MM-dd hh:nn:ss"));
                    w.WriteLine("----------------------------------------------------------------");
                    w.WriteLine(" NO   | Name                 | Text       | Category   | Comment");
                    w.WriteLine("----------------------------------------------------------------");

                    string xFmt = "X{0,4:D4} " + seperator + " {1,-20} " + seperator + " {2,-10} " + seperator + " {3,-10} " + seperator + " {4}";
                    string yFmt = "Y{0,4:D4} " + seperator + " {1,-20} " + seperator + " {2,-10} " + seperator + " {3,-10} " + seperator + " {4}";

                    foreach (var kvp in XItems)
                        w.WriteLine(String.Format(xFmt, kvp.Value.CardIndex * 100 + kvp.Value.BitIndex, kvp.Key, kvp.Value.Text, kvp.Value.Category, kvp.Value.Comment));
                    foreach (var kvp in YItems)
                        w.WriteLine(String.Format(yFmt, kvp.Value.CardIndex * 100 + kvp.Value.BitIndex, kvp.Key, kvp.Value.Text, kvp.Value.Category, kvp.Value.Comment));
                }
            }
            #endregion
        }

        public interface IDIOController
        {
            #region property

            bool Connected { get; set; }
            #endregion

            #region method

            uint GetX(int cardIndex);
            uint GetY(int cardIndex);
            void SetY(int cardIndex, uint value);

            bool GetBitX(int cardIndex, int bitIndex);
            bool GetBitY(int cardIndex, int bitIndex);
            void SetBitY(int cardIndex, int bitIndex, bool state);
            void ToggleBitY(int cardIndex, int bitIndex);
            #endregion
        }
    }
}