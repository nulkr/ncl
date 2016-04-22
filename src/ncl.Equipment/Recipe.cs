using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ncl
{
    namespace Equipment
    {
        [Flags]
        public enum RecipeKindFlag
        {
            None = 0x0000,
            Fixed = 0x0001,	// symbol = !
            CIM = 0x0002,	// symbol = @
            PMAC = 0x0010,   // symbol = D
            PLC = 0x0020,   // sympol = P
            AJIN = 0x0040,   // sympol = A
            Bool = 0x1000,   // symbol = B
            Int = 0x2000,   // symbol = #
            String = 0x4000,   // symbol = S
        };

        // Key, Value, Text만 저정한다
        public class RecipeItem
        {
            public double Value = 0;
            public string Text = "";
            public RecipeKindFlag Kind;
            public string Category = "";
            public string Comment = "";

            public string Symbols
            {
                get
                {
                    string s = "";
                    if ((Kind & RecipeKindFlag.Fixed) > 0) s += '!';
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

            public bool IsString()
            {
                return (Kind & RecipeKindFlag.String) > 0;
            }

            public bool IsBool()
            {
                return (Kind & (RecipeKindFlag.Bool)) > 0;
            }

            public bool IsInt()
            {
                return (Kind & RecipeKindFlag.Int) > 0;
            }
        }

        public class Recipe
        {
            private readonly char[] PMAC_PREFIX = { 'M', 'I', 'Q', 'P' };
            private MemoryStream _BackupStream;

            public readonly DataInfo DataInfo = new DataInfo(1, "ncl.Equipment Recipe"); // 항상 최신 버전을 가지고 있어야 하므로 readonly

            public string FixedRcpFile;

            public Dictionary<string, RecipeItem> Items = new Dictionary<string, RecipeItem>();

            public RecipeItem this[string key]
            {
                get
                {
                    try
                    {
                        return Items[key];
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                        return new RecipeItem();
                    }
                }
                set
                {
                    try
                    {
                        Items[key] = value;
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                    }
                }
            }

            public Recipe(string fixedRcpFile = "")
            {
                if (fixedRcpFile == "")
                    FixedRcpFile = App.Path + App.Name + ".fxrcp";
                else
                    FixedRcpFile = fixedRcpFile;

                _BackupStream = new MemoryStream();
            }

            public event ReadBinaryEventHandler OnReadBinary;

            public event WriteBinaryEventHandler OnWriteBinary;

            // 모든 데이터 목록 삭제
            public void ClearSchema(string filename)
            {
                Items.Clear();
            }

            // PMAC Define 파일을 Parsing 하여 데이터 목록을 얻고 Items에 추가 한다.
            public void AddPmacSchema(string filename, RecipeKindFlag kind = RecipeKindFlag.None)
            {
                string s;
                using (var fr = new StreamReader(filename, Encoding.GetEncoding("ks_c_5601-1987"), true))
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
                                    double.TryParse(words[i], out item.Value);
                                    break;

                                case 3:
                                    item.Comment = words[i].Trim();
                                    break;

                                case 4:
                                    item.Symbols = words[i].Trim();
                                    break;
                            }
                        }

                        if (key != "" && !Items.ContainsKey(key))
                        {
                            if (item.Kind.HasFlag(RecipeKindFlag.String) || item.Text.Length < 1 || item.Text.ToUpper().IndexOfAny(PMAC_PREFIX) != 0)
                                item.Kind |= kind;
                            else
                                item.Kind |= RecipeKindFlag.PMAC | kind;

                            Items.Add(key, item);
                        }
                    }
            }

            // CSV 파일을 Parsing 하여 데이터 목록을 얻고 Items에 추가 한다.
            public void LoadCsvSchema(string filename, char seperator = ',')
            {
                string s;
                using (var fr = new StreamReader(filename))
                    while ((s = fr.ReadLine()) != null)
                    {
                        s = s.Trim();
                        if (s.Length < 1) continue; // ignore empty
                        if (s.IndexOf("//") == 0) continue; // ignore comment
                        if (s.IndexOf(';') == 0) continue; // ignore comment

                        string[] words = s.Split(seperator);
                        if (words.Length < 1)  // ignore empty
                            continue;

                        string key = words[0].Trim();
                        if (key == "" || Items.ContainsKey(key)) // ignore empty || aleady exists
                            continue;

                        RecipeItem item = new RecipeItem();

                        // Key, Value, Text, Category, Comment, Symbols 의 순서
                        for (int i = 1; i < words.Length; i++)
                        {
                            switch (i)
                            {
                                case 1: double.TryParse(words[i], out item.Value); break;
                                case 2: item.Text = words[i].Trim(); break;
                                case 3: item.Category = words[i].Trim(); break;
                                case 4: item.Comment = words[i].Trim(); break;
                                case 5: item.Symbols = words[i].Trim(); break;
                            }
                        }
                        Items.Add(key, item);
                    }
            }

            // 데이터 목록을 CSV 파일에 쓴다
            public void SaveCsvSchema(string filename, char seperator = ',')
            {
                using (var w = new StreamWriter(filename, false))
                {
                    w.WriteLine("// " + DataInfo.ToString());
                    w.WriteLine("// " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    string fmt = "{0,-30}" + seperator + " {1,-10}" + seperator + " {2,-10}" + seperator + " {3,-20}" + seperator + " {4}" + seperator + " {5}";
                    w.WriteLine(fmt, "// Key", "Value", "Text", "Category", "Comment", "Symbols (!:Fixed *:Param @:CIM D:PMAC P:PLC A: AJIN B:Boolean #:Integer S:String)");

                    foreach (var kvp in Items)
                        w.WriteLine(string.Format(fmt, kvp.Key, kvp.Value.Value, kvp.Value.Text, kvp.Value.Category, kvp.Value.Comment, kvp.Value.Symbols));
                }
            }

            public virtual void ReadAddtionalBianry(BinaryReader r, int verNo)
            {
            }

            public void LoadFromStream(Stream stream, bool isFixedFile)
            {
                using (var r = new BinaryReader(stream))
                {
                    int verNo = r.ReadInt32();
                    string verTxt = r.ReadString();

                    int cnt = r.ReadInt32();
                    for (int i = 0; i < cnt; i++)
                    {
                        string key = r.ReadString();
                        double val = r.ReadDouble();
                        string txt = r.ReadString();

                        if (Items.ContainsKey(key)) // Schema 에 존재
                        {
                            RecipeItem ri = Items[key];

                            if (isFixedFile && !ri.Kind.HasFlag(RecipeKindFlag.Fixed)) // Fixed 체크
                                continue;

                            ri.Value = val; // Value 는 항시 적용
                            if (ri.Kind.HasFlag(RecipeKindFlag.String)) // TEXT 는 String Type 일때만 적용, TEXT에 ADDRESS 정보도 들어가기 때문
                                Items[key].Text = txt;
                        }
                    }

                    if (isFixedFile) // Fixed Data 를 읽어오는 시점에 백업한다
                        Backup(stream);
                    else
                    {
                        // additional fixed data 는 별도의 파일로 읽어하자
                        // FixedRcpFile 가 아닌 경우만 추가 데이터를 읽는다
                        ReadAddtionalBianry(r, verNo);

                        if (OnReadBinary != null)
                            OnReadBinary(this, new ReadBinaryEventArgs(r, verNo)); // Fixed Data 읽기
                    }
                }
            }

            // RCP 파일 읽기
            public void Load(string filename)
            {
                bool isFixedFile = Utils.SameFileName(filename, FixedRcpFile);

                using (var f = new FileStream(filename, FileMode.Open))
                {
                    LoadFromStream(f, isFixedFile);

                    if (!isFixedFile)
                        Load(FixedRcpFile); // Fixed Data 읽기
                }
            }

            public void WriteAddtionalBianry(BinaryWriter w)
            {
            }

            public void SaveToStream(Stream stream, bool isFixedFile)
            {
                using (var w = new BinaryWriter(stream))
                {
                    w.Write(DataInfo.VersionNo);
                    w.Write(DataInfo.Description);

                    w.Write(Items.Count);
                    foreach (var kvp in Items)
                    {
                        w.Write(kvp.Key);
                        w.Write(kvp.Value.Value);
                        w.Write(kvp.Value.Text);
                    }

                    if (isFixedFile) // Fixed Data 를 저장하는 시점에 백업한다
                        Backup(stream);
                    else
                    {
                        // additional fixed data 는 별도의 파일로 저장하자
                        // FixedRcpFile 가 아닌 경우만 추가 데이터를 파일에 쓴다
                        WriteAddtionalBianry(w);

                        if (OnWriteBinary != null)
                            OnWriteBinary(this, new WriteBinaryEventArgs(w));
                    }
                }
            }

            // RCP 파일 저장, 항상 최신 버전으로 저장된다
            public void Save(string filename)
            {
                bool isFixedFile = Utils.SameFileName(filename, FixedRcpFile);

                using (var f = new FileStream(filename, FileMode.Create))
                {
                    SaveToStream(f, isFixedFile);
                }

                if (!isFixedFile)
                    Save(FixedRcpFile); // Fixed Data 저장
            }

            // 내부 Backup Buffer 로부터 데이터를 읽어온다
            public void Restore()
            {
                _BackupStream.Seek(0, SeekOrigin.Begin);
                using (var ms = new MemoryStream())
                {
                    _BackupStream.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    LoadFromStream(ms, false);
                }
            }

            // 내부 Backup Buffer로 데이터를 저장한다.s
            private void Backup(Stream src)
            {
                _BackupStream = new MemoryStream();
                src.Seek(0, SeekOrigin.Begin);
                src.CopyTo(_BackupStream);
            }

            public void Backup()
            {
                using (var ms = new MemoryStream()) // SaveToStream 호출시마다 자동 백업됨
                    SaveToStream(ms, false);
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

                foreach (KeyValuePair<string, RecipeItem> kvp in Items)
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

                    if (Items.ContainsKey(key))
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

                foreach (KeyValuePair<string, RecipeItem> kvp in Items)
                {
                    if ((kvp.Value.Kind & kind) > 0)
                        rtd.Add(kvp.Key, kvp.Value);
                }

                dstPropGrid.ResumeDrawing();

                dstPropGrid.Refresh();
            }

            /// Control 의 이름에 따라 Key를 추출하고 해당 Key의 item 을 찾는다
            /// <param name="ctrl">검사할 Control</param>
            /// <param name="type">일치해야할 Control Type</param>
            /// <param name="sPrefix">포함 해야할 Control.Name의 Prefix</param>
            /// <param name="kind">포함 해야할 Kind Flag</param>
            /// <param name="item">해당 Item을 넘겨 받을 객체</param>
            /// <returns>KindFlag를 포함할때만 return true</returns>
            private bool FindItem(Control ctrl, Type type, string sPrefix, RecipeKindFlag kind, ref RecipeItem item)
            {
                if ((ctrl.GetType() != type) || (ctrl.Name.IndexOf(sPrefix) != 0))
                    return false;

                string key = ctrl.Name.Substring(sPrefix.Length);

                if (!Items.ContainsKey(key))
                    return false;

                if ((Items[key].Kind & kind) != kind)
                    return false;

                item = Items[key];
                return true;
            }

            /// Recipe -> Child Control
            /// <param name="ctrlParent"></param>
            /// <param name="kind"></param>
            public void AssignToControls(Control ctrlParent, RecipeKindFlag kind = RecipeKindFlag.None)
            {
                foreach (Control ctrl in ctrlParent.Controls)
                {
                    RecipeItem item = null;

                    if (FindItem(ctrl, typeof(ValueEdit), "edt_", kind, ref item))
                    {
                        if (item.IsString())
                            (ctrl as ValueEdit).Text = item.Text;
                        else if (item.IsInt() || item.IsBool())
                            (ctrl as ValueEdit).Value = item.AsInt;
                        else
                            (ctrl as ValueEdit).Value = item.Value;
                    }
                    else if (FindItem(ctrl, typeof(ComboBox), "cb_", kind, ref item))
                        (ctrl as ComboBox).SelectedIndex = item.AsInt;
                    else if (FindItem(ctrl, typeof(RadioGroup), "rg_", kind, ref item))
                        (ctrl as RadioGroup).SelectedIndex = item.AsInt;
                    else if (FindItem(ctrl, typeof(StateButton), "sbtn_", kind, ref item))
                        (ctrl as StateButton).State = item.AsBool;
                    else if (FindItem(ctrl, typeof(CheckBox), "chk_", kind, ref item))
                        (ctrl as CheckBox).Checked = item.AsBool;

                    if (ctrl.HasChildren)
                        AssignToControls(ctrl, kind);
                }
            }

            /// Child Control -> Recipe
            /// <param name="ctrlParent"></param>
            /// <param name="kind"></param>
            public void AssignFromControls(Control ctrlParent, RecipeKindFlag kind = RecipeKindFlag.None)
            {
                foreach (Control ctrl in ctrlParent.Controls)
                {
                    RecipeItem item = null;

                    if (FindItem(ctrl, typeof(ValueEdit), "edt_", kind, ref item))
                    {
                        if (item.IsString())
                            item.Text = (ctrl as ValueEdit).Text;
                        else if (item.IsInt() || item.IsBool())
                            item.AsInt = (ctrl as ValueEdit).AsInt;
                        else
                            item.Value = (ctrl as ValueEdit).Value;
                    }
                    else if (FindItem(ctrl, typeof(ComboBox), "cb_", kind, ref item))
                        item.AsInt = (ctrl as ComboBox).SelectedIndex;
                    else if (FindItem(ctrl, typeof(RadioGroup), "rg_", kind, ref item))
                        item.AsInt = (ctrl as RadioGroup).SelectedIndex;
                    else if (FindItem(ctrl, typeof(StateButton), "sbtn_", kind, ref item))
                        item.AsBool = (ctrl as StateButton).State;
                    else if (FindItem(ctrl, typeof(CheckBox), "chk_", kind, ref item))
                        item.AsBool = (ctrl as CheckBox).Checked;

                    if (ctrl.HasChildren)
                        AssignFromControls(ctrl, kind);
                }
            }
        }
    }

    public class ReadBinaryEventArgs : EventArgs
    {
        public BinaryReader Reader { get; set; }
        public int VerNo { get; set; }

        public ReadBinaryEventArgs(BinaryReader reader, int verNo)
        {
            this.Reader = reader;
            this.VerNo = verNo;
        }
    }

    public delegate void ReadBinaryEventHandler(object sender, ReadBinaryEventArgs e);

    public class WriteBinaryEventArgs : EventArgs
    {
        public BinaryWriter Writer { get; set; }

        public WriteBinaryEventArgs(BinaryWriter writer)
        {
            this.Writer = writer;
        }
    }

    public delegate void WriteBinaryEventHandler(object sender, WriteBinaryEventArgs e);
}