using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;

namespace ncl
{
    namespace Equipment
    {
        [Flags]
        public enum RecipeKindFlag
        {
            None = 0x0000,
            Fixed = 0x0001,	// symbol = !
            Param = 0x0002,   // symbol = *
            CIM = 0x0004,	// symbol = @
            PMAC = 0x0010,   // symbol = D
            PLC = 0x0020,   // sympol = P
            AJIN = 0x0040,   // sympol = A
            Bool = 0x1000,   // symbol = B
            Int = 0x2000,   // symbol = #
            String = 0x4000,   // symbol = S
            All = 0xFFFFFF  // all flags
        };

        [DataContractAttribute()]
        public class RecipeItem
        {
            #region field

            public RecipeKindFlag Kind;

            [DataMemberAttribute()]
            public double Value;

            [DataMemberAttribute()]
            public string Text;

            [DataMemberAttribute()]
            public string Category;

            [DataMemberAttribute()]
            public string Comment;
            #endregion

            #region property

            [DataMemberAttribute()]
            public string Symbols
            {
                get
                {
                    string s = "";
                    if ((Kind & RecipeKindFlag.Fixed) > 0) s += '!';
                    if ((Kind & RecipeKindFlag.Param) > 0) s += '*';
                    if ((Kind & RecipeKindFlag.CIM) > 0) s += '@';
                    if ((Kind & RecipeKindFlag.PMAC) > 0) s += 'D';
                    if ((Kind & RecipeKindFlag.PLC) > 0) s += 'P';
                    if ((Kind & RecipeKindFlag.AJIN) > 0) s += 'A';
                    if ((Kind & RecipeKindFlag.Bool) > 0) s += 'B';
                    if ((Kind & RecipeKindFlag.Int) > 0) s += '#';
                    if ((Kind & RecipeKindFlag.String) > 0) s += 'S';
                    return s;
                }
                set
                {
                    Kind = 0;
                    foreach (char c in value)
                        switch (c)
                        {
                            case '!': Kind |= RecipeKindFlag.Fixed; break;
                            case '*': Kind |= RecipeKindFlag.Param; break;
                            case '@': Kind |= RecipeKindFlag.CIM; break;
                            case 'D': Kind |= RecipeKindFlag.PMAC; break;
                            case 'P': Kind |= RecipeKindFlag.PLC; break;
                            case 'A': Kind |= RecipeKindFlag.AJIN; break;
                            case 'B': Kind |= RecipeKindFlag.Bool; break;
                            case '#': Kind |= RecipeKindFlag.Int; break;
                            case 'S': Kind |= RecipeKindFlag.String; break;
                        }
                }
            }

            public int AsInt
            {
                get { return (int)Math.Round(Value); }
                set { Value = value; }
            }

            public bool AsBool
            {
                get { return AsInt != 0; }
                set
                {
                    if (value) Value = 1;
                    else Value = 0;
                }
            }
            #endregion

            #region method

            public bool IsString() { return (Kind & RecipeKindFlag.String) > 0; }
            public bool IsBool() { return (Kind & (RecipeKindFlag.Bool)) > 0; }
            public bool IsInt() { return (Kind & RecipeKindFlag.Int) > 0; }
            #endregion
        }

        [DataContractAttribute()]
        public class Recipe
        {
            #region field

            private byte[] _BackupBuffer;

            [DataMemberAttribute()]
            public readonly DataInfo DataInfo = new DataInfo(1, "ncl Recipe");

            [DataMemberAttribute()]
            public Dictionary<string, RecipeItem> Dictionary = new Dictionary<string, RecipeItem>();
            #endregion

            #region property

            public RecipeItem this[string key]
            {
                get { return Dictionary[key]; }
                set { Dictionary[key] = value; }
            }
            #endregion

            #region method

            /// Control 의 이름에 따라 Key를 추출하고 해당 Key 가 존재하면 item 에 넣어준다
            /// <param name="ctrl">검사할 Control</param>
            /// <param name="type">일치해야할 Control Type</param>
            /// <param name="sPrefix">포함 해야할 Control.Name의 Prefix</param>
            /// <param name="kind">포함 해야할 Kind Flag</param>
            /// <param name="item">해당 Item을 넘겨 받을 객체</param>
            /// <returns>KindFlag를 포함할때만 return true</returns>
            private bool ValidKey(Control ctrl, Type type, string sPrefix, RecipeKindFlag kind, ref RecipeItem item)
            {
                if ((ctrl.GetType() != type) || (ctrl.Name.IndexOf(sPrefix) != 0))
                    return false;

                string sKey = ctrl.Name.Substring(sPrefix.Length);

                if (!Dictionary.ContainsKey(sKey))
                    return false;

                item = Dictionary[sKey];

                if ((item.Kind & kind) == kind) return true;
                else return false;
            }

            /// Stream 에서 압축 풀어 읽음
            /// <para>override method 에서는 이걸 가지고 각 버전에 맞게 읽을수 있도록 하자</para>
            /// <param name="reader"></param>
            /// <param name="ver">파일에서 읽어들인 Version No</param>
            protected virtual void ReadBinary(BinaryReader reader, ref int ver)
            {
                Dictionary.Clear();

                ver = reader.ReadInt32();

                int nCnt = reader.ReadInt32();

                while (nCnt-- > 0)
                {
                    RecipeItem item = new RecipeItem();

                    string key = reader.ReadString();
                    item.Kind = (RecipeKindFlag)reader.ReadInt32();
                    item.Value = reader.ReadDouble();
                    item.Text = reader.ReadString();
                    item.Category = reader.ReadString();
                    item.Comment = reader.ReadString();

                    Dictionary.Add(key, item);
                }
            }

            /// Stream 에 저장. 항상 최신 버전으로 저장한다
            /// <param name="writer"></param>
            protected virtual void WriteBinary(BinaryWriter writer)
            {
                writer.Write(DataInfo.VersionNo);

                writer.Write(Dictionary.Count);
                foreach (KeyValuePair<string, RecipeItem> kvp in Dictionary)
                {
                    writer.Write(kvp.Key);
                    writer.Write((int)kvp.Value.Kind);
                    writer.Write(kvp.Value.Value);
                    writer.Write(kvp.Value.Text);
                    writer.Write(kvp.Value.Category);
                    writer.Write(kvp.Value.Comment);
                }
            }

            public void LoadFromCSV(string filename)
            {
                //Dictionary.Clear();

                string s;
                StreamReader fr = new StreamReader(filename, false);

                while ((s = fr.ReadLine()) != null)
                {
                    s = s.Trim();
                    if (s.Length < 1) continue;

                    // ignore comment
                    if (s.IndexOf("//") == 0) continue;
                    if (s.IndexOf(';') == 0) continue;

                    string[] words = s.Split(',');
                    if (words.Length < 1)
                        continue;

                    RecipeItem item = new RecipeItem();
                    string key = "";

                    for (int i = 0; i < words.Length; i++)
                    {
                        switch (i)
                        {
                            case 0: item.Category = words[i].Trim(); break;
                            case 1: key = words[i].Trim(); break;
                            case 2: item.Value = Convert.ToDouble(words[i]); break;
                            case 3: item.Text = words[i].Trim(); break;
                            case 4: item.Comment = words[i].Trim(); break;
                            case 5: item.Symbols = words[i].Trim(); break;
                        }
                    }

                    if (key != "")
                    {
                        if (Dictionary.ContainsKey(key))
                        {
                            if (MsgBox.Query(key + " is aleady exists!\nAbort process?"))
                                break;
                        }
                        else
                            Dictionary.Add(key, item);
                    }
                }

                fr.Close();
            }

            public void SaveToCSV(string filename)
            {
                StreamWriter fw = new StreamWriter(filename, false);

                fw.WriteLine("// Category, Key, Value, Text, Comment, Symbols (!:Fixed *:Param @:CIM 0:PMAC 1:PLC I:Input O:Output B:Boolean #:Integer S:String)");

                foreach (KeyValuePair<string, RecipeItem> kvp in Dictionary)
                {
                    fw.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", kvp.Value.Category, kvp.Key, kvp.Value.Value, kvp.Value.Text, kvp.Value.Comment, kvp.Value.Symbols));
                }

                fw.Close();
            }

            public void LoadFromFile(string filename)
            {
                using (var file = new FileStream(filename, FileMode.Open))
                using (var deflate = new DeflateStream(file, CompressionMode.Decompress))
                using (var reader = new BinaryReader(deflate))
                {
                    // TODO : check version
                    int ver = DataInfo.VersionNo;
                    ReadBinary(reader, ref ver);
                }
            }

            public void SaveToFile(string filename)
            {
                using (var file = new FileStream(filename, FileMode.OpenOrCreate))
                using (var deflate = new DeflateStream(file, CompressionMode.Compress))
                using (var writer = new BinaryWriter(deflate))
                {
                    WriteBinary(writer);
                }
            }

            /// 내부 Backup Buffer로 데이터를 저장한다.
            public void Backup()
            {
                using (var ms = new MemoryStream())
                using (var ds = new DeflateStream(ms, CompressionMode.Compress))
                using (var writer = new BinaryWriter(ds))
                {
                    WriteBinary(writer);

                    _BackupBuffer = ms.GetBuffer();
                }
            }

            /// 내부 Backup Buffer 로부터 데이터를 읽어온다
            /// <returns></returns>
            public bool Restore()
            {
                using (var ms = new MemoryStream(_BackupBuffer))
                using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
                using (var reader = new BinaryReader(ds))
                {
                    int ver = DataInfo.VersionNo;
                    ReadBinary(reader, ref ver);
                }

                return true;
            }

            /// PMAC Define 파일을 Parsing 하여 Dictionary에 추가 한다. 기존 데이터와 겹치는 건 패스
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

                    RecipeItem item = new RecipeItem();
                    string key = "";

                    for (int i = 0; i < words.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                words[i] = Regex.Replace(words[i], "#define", "", RegexOptions.IgnoreCase).Trim();

                                if (words[i].IndexOf('"') > -1)
                                {
                                    string[] sNameAndText = words[i].Split('"');
                                    key = sNameAndText[0].Trim();
                                    item.Text = sNameAndText[1];
                                    item.Kind |= RecipeKindFlag.String;
                                }
                                else
                                {
                                    string[] sNameAndText = words[i].Split(' ');
                                    if (sNameAndText.Length > 1)
                                    {
                                        key = sNameAndText[0];
                                        if (!double.TryParse(sNameAndText[sNameAndText.Length - 1], out item.Value))
                                        {
                                            item.Text = sNameAndText[1];
                                            item.Kind |= RecipeKindFlag.String;
                                        }
                                    }
                                }
                                break;
                            case 1:
                                item.Category = words[i].Trim();
                                break;
                            case 2:
                                item.Value = Convert.ToDouble(words[i]);
                                break;
                            case 3:
                                item.Comment = words[i].Trim();
                                break;
                            case 4:
                                item.Symbols = words[i].Trim();
                                break;
                        }
                    }

                    if (key != "" && !Dictionary.ContainsKey(key))
                    {
                        item.Kind |= RecipeKindFlag.PMAC | kind;

                        Dictionary.Add(key, item);
                    }
                }

                fr.Close();
            }

            /// kind 속성을 갖는 Recipe Item을 그리드에 출력 
            /// <param name="dstGrid"></param>
            /// <param name="kind">Grid에 출력할 RecipeKindFlag</param>
            public void AssignToGrid(DataGridView dstGrid, RecipeKindFlag kind)
            {
                dstGrid.SuspendDrawing();
                dstGrid.ColumnCount = 4;
                dstGrid.RowCount = 1;
                dstGrid.Columns[0].HeaderText = "Category";
                dstGrid.Columns[0].ReadOnly = true;
                dstGrid.Columns[1].HeaderText = "Name";
                dstGrid.Columns[1].ReadOnly = true;
                dstGrid.Columns[2].HeaderText = "Value";
                dstGrid.Columns[3].HeaderText = "Comment";
                dstGrid.Columns[3].ReadOnly = true;

                foreach (KeyValuePair<string, RecipeItem> kvp in Dictionary)
                {
                    // kind 가 지정되어 있고, RecipeItem.Kind 가 해당되지 않으면 패스
                    if (kind != RecipeKindFlag.None && (kvp.Value.Kind & kind) == 0)
                        continue;

                    if (kvp.Value.IsString())
                    {
                        string[] row = { kvp.Value.Category, kvp.Key, kvp.Value.Text, kvp.Value.Comment };
                        dstGrid.Rows.Add(row);
                    }
                    else
                    {
                        string[] row = { kvp.Value.Category, kvp.Key, kvp.Value.Value.ToString(), kvp.Value.Comment };
                        int nRow = dstGrid.Rows.Add(row);

                        if (kvp.Value.IsBool())
                        {
                            if (kvp.Value.AsBool)
                                dstGrid.Rows[nRow].Cells[2].Style.BackColor = System.Drawing.Color.Lime;
                            else
                                dstGrid.Rows[nRow].Cells[2].Style.BackColor = System.Drawing.SystemColors.Control;

                            dstGrid.Rows[nRow].Cells[2].ReadOnly = true;
                        }
                    }
                }
                dstGrid.ResumeDrawing();
            }

            /// Grid로부터 변경된 Recipe Item을 입력 받는다
            /// <param name="srcGrid"></param>
            public void AssignFromGrid(DataGridView srcGrid)
            {
                for (int i = 1; i < srcGrid.RowCount; i++)
                {
                    string key = srcGrid.Rows[i].Cells[1].ToString();

                    if (Dictionary.ContainsKey(key))
                    {
                        RecipeItem item = this[key];

                        if (item.IsString())
                            item.Text = srcGrid.Rows[i].Cells[2].ToString();
                        else if (item.IsInt() || item.IsBool())
                            item.AsInt = Convert.ToInt32(srcGrid.Rows[i].Cells[2].ToString());
                        else
                            item.Value = Convert.ToDouble(srcGrid.Rows[i].Cells[2].ToString());
                    }
                }
            }

            /// Grid Cell 의 Edit 후에 변경된 값이 Recipe Item 에 자동 적용되도록 한다.
            /// <para>이벤트가 중복되지 않게 한번만 등록하도록 하자</para>
            /// <param name="dstGrid"></param>
            public void RegisterGrid(DataGridView dstGrid)
            {
                DataGridViewCellEventHandler gridCellEndEdit = (sender, e) =>
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                    string key = (sender as DataGridView).Rows[e.RowIndex].Cells[1].FormattedValue.ToString();

                    if (key == "") return;

                    if (this[key].IsString())
                        this[key].Text = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
                    else
                    {
                        if (e.ColumnIndex == 2)
                            this[key].Value = Convert.ToDouble((sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString());
                    }
                };

                dstGrid.CellEndEdit += new DataGridViewCellEventHandler(gridCellEndEdit);
            }

            /// Grid Cell 의 값 변경시 Recipe Item 에 자동 적용되도록 한다.
            /// <param name="dstPropGrid"></param>
            /// <param name="kind"></param>
            public void RegisterProperty(PropertyGrid dstPropGrid, RecipeKindFlag kind)
            {
                RecipeTypeDescriptor rtd = new RecipeTypeDescriptor();

                rtd.Clear();

                dstPropGrid.SuspendDrawing();

                dstPropGrid.SelectedObject = rtd;

                foreach (KeyValuePair<string, RecipeItem> kvp in Dictionary)
                {
                    if ((kvp.Value.Kind & kind) > 0)
                        rtd.Add(kvp.Key, kvp.Value);
                }

                dstPropGrid.ResumeDrawing();

                dstPropGrid.Refresh();
            }

            /// Recipe -> Child Control
            /// <param name="ctrlParent"></param>
            /// <param name="kind"></param>
            public void AssignToControls(Control ctrlParent, RecipeKindFlag kind = RecipeKindFlag.Param)
            {
                RecipeItem item = null;

                foreach (Control ctrl in ctrlParent.Controls)
                {
                    if (ValidKey(ctrl, typeof(ValueEdit), "edt_", kind, ref item))
                    {
                        if (item.IsString())
                            (ctrl as ValueEdit).Text = item.Text;
                        else if (item.IsInt() || item.IsBool())
                            (ctrl as ValueEdit).Value = item.AsInt;
                        else
                            (ctrl as ValueEdit).Value = item.Value;
                    }
                    else if (ValidKey(ctrl, typeof(ComboBox), "cb_", kind, ref item))
                        (ctrl as ComboBox).SelectedIndex = item.AsInt;
                    else if (ValidKey(ctrl, typeof(RadioGroup), "rg_", kind, ref item))
                        (ctrl as RadioGroup).SelectedIndex = item.AsInt;
                    else if (ValidKey(ctrl, typeof(StateButton), "sbtn_", kind, ref item))
                        (ctrl as StateButton).State = item.AsBool;
                    else if (ValidKey(ctrl, typeof(CheckBox), "chk_", kind, ref item))
                        (ctrl as CheckBox).Checked = item.AsBool;

                    if (ctrl.HasChildren)
                        AssignToControls(ctrl, kind);
                }
            }

            /// Child Control -> Recipe
            /// <param name="ctrlParent"></param>
            /// <param name="kind"></param>
            public void AssignFromControls(Control ctrlParent, RecipeKindFlag kind = RecipeKindFlag.Param)
            {
                RecipeItem item = null;

                foreach (Control ctrl in ctrlParent.Controls)
                {
                    if (ValidKey(ctrl, typeof(ValueEdit), "edt_", kind, ref item))
                    {
                        if (item.IsString())
                            item.Text = (ctrl as ValueEdit).Text;
                        else if (item.IsInt() || item.IsBool())
                            item.AsInt = (ctrl as ValueEdit).AsInt;
                        else
                            item.Value = (ctrl as ValueEdit).Value;
                    }
                    else if (ValidKey(ctrl, typeof(ComboBox), "cb_", kind, ref item))
                        item.AsInt = (ctrl as ComboBox).SelectedIndex;
                    else if (ValidKey(ctrl, typeof(RadioGroup), "rg_", kind, ref item))
                        item.AsInt = (ctrl as RadioGroup).SelectedIndex;
                    else if (ValidKey(ctrl, typeof(StateButton), "sbtn_", kind, ref item))
                        item.AsBool = (ctrl as StateButton).State;
                    else if (ValidKey(ctrl, typeof(CheckBox), "chk_", kind, ref item))
                        item.AsBool = (ctrl as CheckBox).Checked;

                    if (ctrl.HasChildren)
                        AssignFromControls(ctrl, kind);
                }
            }

            #endregion
        }
    }
}
