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
        [DataContractAttribute()]
        public class DIOItem
        {
            #region field

            [DataMemberAttribute()]
            public int CardIndex = 0;

            [DataMemberAttribute()]
            public int BitIndex = 0;

            [DataMemberAttribute()]
            public string Text = "";

            [DataMemberAttribute()]
            public string Category = "";

            [DataMemberAttribute()]
            public string Comment = "";
            #endregion
        }


        // XDatas, YDatas array 의 값은 별도의 쓰레드에서 한번에 읽고 쓰자
        [DataContractAttribute()]
        public class DIOList
        {
            #region field

            [DataMemberAttribute()]
            public readonly DataInfo DataInfo = new DataInfo(1, "ncl DIOList");

            public volatile bool NeedWriting = false;

            [DataMemberAttribute(Name = "CardCount")]
            private int _CardCount = 32;

            [DataMemberAttribute(Name = "BitCount")]
            private int _BitCount = 32;

            [DataMemberAttribute()]
            public Dictionary<string, DIOItem> XItems = new Dictionary<string, DIOItem>();

            [DataMemberAttribute()]
            public Dictionary<string, DIOItem> YItems = new Dictionary<string, DIOItem>();

            public volatile UInt32[] XDatas = null;
            public volatile UInt32[] YDatas = null;
            #endregion

            #region property

            public int CardCount { get { return _CardCount; } }
            public int BitCount { get { return _BitCount; } }
            #endregion

            #region constructor

            public DIOList(int cardCount = 32, int bitCount = 32)
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
                        item.CardIndex = no / 200; // TODO : 32 개씩 잘랐을 경우 no /100 해야 함
                        item.BitIndex = no % 50;

                        if (words.Length > 1)
                            item.Category = words[1].Trim();
                        if (words.Length > 2)
                            item.Comment = words[2].Trim();

                        if ((no % 100) < 50)
                            XItems.Add(key, item);
                        else
                            YItems.Add(key, item);
                    }

                    fr.Close();
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