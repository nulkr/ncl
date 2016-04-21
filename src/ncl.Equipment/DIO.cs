using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;

namespace ncl
{
    namespace Equipment
    {
        /// <summary>
        /// IO SUB 의 Char 속성
        /// </summary>
        public enum DIOSubType { None = '_', Input = 'I', Output = 'O' }

        public class DIOItem
        {
            public int ModIndex = 0;
            public int SubIndex = 0;
            public int BitIndex = 0;

            public string Category = "";
            public string Text = "";
            public string Comment = "";

            public DIOItem(int mod = 0, int sub = 0, int bit = 0, string cate = "Spare", string txt = "", string cmt = "")
            {
                ModIndex = 0;
                SubIndex = 0;
                BitIndex = 0;
                Category = cate;
                Text = txt;
                Comment = cmt;
            }
        }

        public class DIOList
        {
            public readonly DataInfo DataInfo = new DataInfo(1, "ncl.Equipment.DIOs");

            private int _ModCount = 2;
            private int _SubCount = 16;
            private int _BitCount = 16;

            /// <summary>
            /// Output IO의 값이 변동되었을때만 Write 하는 Flag
            /// </summary>
            public volatile bool NeedWriting = false;

            /// <summary>
            /// DIOItem이 저장된 Dictionary
            /// </summary>
            public Dictionary<string, DIOItem> Items = new Dictionary<string, DIOItem>();
            /// <summary>
            /// Module 안의 각각의 Sub 들의 속성 (Input / Output / None)
            /// </summary>
            public DIOSubType[,] SchemaArray = null; // [Mod, Sub]
            /// <summary>
            /// IO 카드의 16,32 비트 unsigned int 값을 저장하는 Array
            /// </summary>
            public volatile uint[,] DataArray = null; // [Mod, Sub]            
            /// <summary>
            /// 빠른 억세스를 위한 Input Boolean Array
            /// </summary>
            public volatile bool[] Bits = null; // [Mod Count * Sub Count * BitCount]

            /// <summary>
            /// IO Module 갯수
            /// </summary>
            public int ModCount { get { return _ModCount; } }
            /// <summary>
            /// IO Module의 Sub 갯수
            /// </summary>
            public int SubCount { get { return _SubCount; } } // in Mod
            /// <summary>
            /// IO Sub의 비트 갯수 (16, 32)
            /// </summary>
            public int BitCount { get { return _BitCount; } } // in Sub

            #region constructor

            public DIOList(int modCount = 2, int subCount = 16, int bitCount = 16)
            {
                _ModCount = modCount;
                _SubCount = subCount;
                _BitCount = bitCount;

                DataArray = new uint[_ModCount, _SubCount];
                SchemaArray = new DIOSubType[_ModCount, _SubCount];
                Bits = new bool[_ModCount * _SubCount * _BitCount];

                for (int mod = 0; mod < _ModCount; mod++)
                    for (int sub = 0; sub < _SubCount; sub++)
                    {
                        SchemaArray[mod, sub] = DIOSubType.None;

                        for (int bit = 0; bit < _BitCount; bit++)
                        {
                            Items.Add(string.Format("{0:D4}", bit + (sub + mod * _SubCount) * 100), new DIOItem(mod, sub, bit, "Spare"));
                        }
                    }
            }
            #endregion

            /// <summary>
            /// 정수 배열인 DataArray의 값을 Boolean 배열인 BitsX, BitsY에 매핑한다
            /// Output I/O는 최초 한번만 읽어 오도록한다.
            /// </summary>
            /// <param name="assignOutput"></param>
            public void DataArrayToBits(bool assignOutput = false)
            {
                int index = 0;

                for (int mod = 0; mod < _ModCount; mod++)
                    for (int sub = 0; sub < _SubCount; sub++)
                    {
                        uint data = DataArray[mod, sub];

                        if ((SchemaArray[mod, sub] == DIOSubType.Input) || (assignOutput && SchemaArray[mod, sub] == DIOSubType.Output))
                        {
                            for (int i = 0; i < _BitCount; i++)
                            {
                                Bits[index] = (data & 0x0001) == 1 ? true : false;
                                data >>= 1;
                                index++;
                            }
                        }
                        else
                            index += _BitCount;
                    }
            }
            /// <summary>
            /// Boolean 배열인 BitsX, BitsY의 값을 정수 배열인 DataArray에 매핑한다
            /// Output만 골라서 매핑
            /// </summary>
            public void BitsToDataArray()
            {
                int index = 0;

                for (int mod = 0; mod < _ModCount; mod++)
                    for (int sub = 0; sub < _SubCount; sub++)
                    {
                        uint data = 0;

                        if (SchemaArray[mod, sub] == DIOSubType.Output)
                        {
                            for (int i = 0; i < _BitCount; i++)
                            {
                                Utils.SetBit32(ref data, i, Bits[index]);
                                index++;
                            }
                            DataArray[mod, sub] = data;
                        }
                        else
                            index += _BitCount;
                    }
            }
            /// <summary>
            /// IO 이름으로 Input 값를 취득 
            /// </summary>
            /// <param name="ioName"></param>
            /// <returns></returns>
            public bool GetBitX(string ioName)
            {
                var v = Items[ioName];
                return Utils.GetBit32(DataArray[v.ModIndex, v.SubIndex], v.BitIndex);
            }
            /// <summary>
            /// IO 이름으로 Output 값를 취득 
            /// </summary>
            /// <param name="ioName"></param>
            /// <returns></returns>
            public bool GetBitY(string ioName)
            {
                var v = Items[ioName];
                return Utils.GetBit32(DataArray[v.ModIndex, v.SubIndex], v.BitIndex);
            }
            /// <summary>
            /// 해당 Output IO를 On 혹은 Off
            /// </summary>
            /// <param name="ioName"></param>
            /// <param name="state"></param>
            public void SetBitY(string ioName, bool state)
            {
                var v = Items[ioName];
                Utils.SetBit32(ref DataArray[v.ModIndex, v.SubIndex], v.BitIndex, state);
                NeedWriting = true;
            }
            /// <summary>
            /// 해당 Output IO를 Toggle
            /// </summary>
            /// <param name="ioName"></param>
            public void ToggleBitY(string ioName)
            {
                var v = Items[ioName];
                Utils.ToggleBit32(ref DataArray[v.ModIndex, v.SubIndex], v.BitIndex);
                NeedWriting = true;
            }
            /// <summary>
            /// PMAC Define 파일로부터 I/O 정보를 취득하여 추가함
            /// </summary>
            /// <param name="filename"></param>
            public void AddFromPMAC(string filename)
            {
                // #define Defined-Name Var // Category, Comment 의 구조
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

                        // 뒤 두자리가 비트
                        item.SubIndex = no / 100;

                        // X: M#00 ~ M#31, Y: M#50 ~ M#81
                        item.BitIndex = no % 50;

                        // PMAC 은 Module 개념이 없으므로 항상 IN:0 OUT:1임
                        if ((no % 100) >= 50)
                            item.ModIndex = 1;
                        else
                            item.ModIndex = 0;                        

                        if (item.SubIndex > 16) 
                        {
                            // TODO : M1600 이상일시 처리
                        }

                        if (words.Length > 1)
                            item.Category = words[1].Trim();
                        if (words.Length > 2)
                            item.Comment = words[2].Trim();

                        // TODO : Check exists key
                        Items.Add(key, item);
                    }

                    fr.Close();
                }
            }
            /// <summary>
            /// CSV 형식의 파일로부터 I/O 정보를 취득하여 초기화함
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="seperator"></param>
            public void SaveCsvSchema(string filename, char seperator = '|')
            {
                string sep = " " + seperator + " ";
                string fmt = "{0:D2}" + sep + "{1:D2}" + sep + "{2:D2}" + sep + "{3,-20}" + sep + "{4,-16}" + sep + "{5,-10}" + sep + "{6}";

                using (StreamWriter w = new System.IO.StreamWriter(filename))
                {
                    // write class info
                    w.WriteLine("# " + DataInfo.ToString());
                    w.WriteLine("");

                    // write counts
                    w.WriteLine("ModCount = " + _ModCount.ToString());
                    w.WriteLine("SubCount = " + _SubCount.ToString());
                    w.WriteLine("BitCount = " + _BitCount.ToString());
                    w.WriteLine("");

                    // MOD[00] = IIIIOOOO________
                    for (int mod = 0; mod < _ModCount; mod++)
                    {
                        string s = string.Format("Mod[{0,2:D2}] = ", mod);
                        for (int sub = 0; sub < _SubCount; sub++)
                            s += (char)SchemaArray[mod, sub];
                        w.WriteLine(s);
                    }
                    w.WriteLine("");

                    w.WriteLine("# Mod | Sub | Bit | Name | Category | Text | Comment");
                    w.WriteLine("");                    

                    foreach (var kvp in Items)
                        w.WriteLine(string.Format(fmt, 
                            kvp.Value.ModIndex,
                            kvp.Value.SubIndex,
                            kvp.Value.BitIndex,
                            kvp.Key,
                            kvp.Value.Category,
                            kvp.Value.Text,
                            kvp.Value.Comment));
                }
            }
            /// <summary>
            /// I/O 정보를 CSV 형식의 파일로 저장함
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="seperator"></param>
            public void LoadCsvSchema(string filename, char seperator = '|')
            {
                Items.Clear();

                using (StreamReader r = new StreamReader(filename))
                {
                    string sLine;
                    while ((sLine = r.ReadLine()) != null)
                    {
                        sLine.Trim();
                        if (string.IsNullOrEmpty(sLine) || sLine.IndexOf('#') == 0)
                            continue;

                        if (sLine.IndexOf("ModCount") == 0)
                        {
                            int n = sLine.IndexOf('=');
                            if (n > 0 && int.TryParse(sLine.Substring(n + 1), out _ModCount))
                                continue;
                        }
                        if (sLine.IndexOf("SubCount") == 0)
                        {
                            int n = sLine.IndexOf('=');
                            if (n > 0 && int.TryParse(sLine.Substring(n + 1), out _SubCount))
                                continue;
                        }
                        if (sLine.IndexOf("BitCount") == 0)
                        {
                            int n = sLine.IndexOf('=');
                            if (n > 0 && int.TryParse(sLine.Substring(n + 1), out _BitCount))
                                continue;
                        }

                        if (sLine.IndexOf("Mod[") == 0)
                        {
                            int modIndex;
                            int n1 = sLine.IndexOf(']');
                            int n2 = sLine.IndexOf('=');
                            if (n1 > 0 && n2 > 0 && int.TryParse(sLine.Substring(4, n1 - 4), out modIndex))
                            {
                                string schema = sLine.Substring(n2 + 1).Trim();

                                for (int sub = 0; sub < Math.Min(schema.Length, _SubCount); sub++)
                                    switch ((DIOSubType)schema[sub])
                                    {
                                        case DIOSubType.Input: SchemaArray[modIndex, sub] = DIOSubType.Input; break;
                                        case DIOSubType.Output: SchemaArray[modIndex, sub] = DIOSubType.Output; break;
                                        default: SchemaArray[modIndex, sub] = DIOSubType.None; break;   
                                    }
                                continue;
                            }
                        }

                        string[] words = sLine.Split(seperator);

                        // check 0:modIndex, 1:subIndex, 2:bitIndex, 3:name
                        if (words.Length < 4)
                            continue;

                        DIOItem item = new DIOItem();

                        if (!int.TryParse(words[0], out item.ModIndex) || !int.TryParse(words[1], out item.SubIndex) || !int.TryParse(words[2], out item.BitIndex))
                            continue;

                        for (int i = 4; i < words.Length; i++)
                            switch (i)
                            {
                                case 4: item.Category = words[i].Trim(); break;
                                case 5: item.Text = words[i].Trim(); break;
                                case 6: item.Comment = words[i].Trim(); break;
                            }

                        Items.Add(words[3].Trim(), item);
                    }
                }
            }
        }
    }
}