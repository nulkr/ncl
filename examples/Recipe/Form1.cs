using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using j2k;

namespace j2kTestRecipe
{
    public partial class Form1 : Form
    {
        public TestRecipe Recipe = new TestRecipe();

        public Form1()
        {
            InitializeComponent();

            Recipe.RegisterGrid(dataGridView1);
        }

        private string GetGrid_CurrentKey(int rowIndex)
        {
            return dataGridView1.Rows[rowIndex].Cells[1].FormattedValue.ToString();
        }

        private void btnPmacVar_Click(object sender, EventArgs e)
        {
            Recipe.AddFromPMAC(@"..\DEFINE_VAR.h", RecipeKindFlag.Param);

            Recipe.AssignToGrid(dataGridView1, RecipeKindFlag.Param);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Recipe.Dictionary.Clear();
            dataGridView1.RowCount = 1;
        }

        private void btnLoadCSV_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                d.DefaultExt = ".csv";
                d.Filter = "CSV Files (*.csv)|*.csv";
                d.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                if (d.ShowDialog() == DialogResult.OK)
                {
                    Recipe.LoadFromCSV(d.FileName);
                    Recipe.AssignToGrid(dataGridView1, RecipeKindFlag.None);
                }
            }
        }

        private void btnSaveCSV_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog d = new SaveFileDialog())
            {
                d.DefaultExt = ".csv";
                d.Filter = "CSV Files (*.csv)|*.csv";
                d.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                if (d.ShowDialog() == DialogResult.OK)
                {
                    Recipe.SaveToCSV(d.FileName);
                }
            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 2) return;

            string key = GetGrid_CurrentKey(e.RowIndex);

            if (key == "") return;

            if (!Recipe[key].IsBool()) return;

            Recipe[key].AsBool = !Recipe[key].AsBool;
            if (Recipe[key].AsBool)
                dataGridView1.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Lime;
            else
                dataGridView1.Rows[e.RowIndex].Cells[2].Style.BackColor = SystemColors.Control;

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                d.DefaultExt = ".rcp";
                d.Filter = "Recipe Files (*.rcp)|*.rcp";
                d.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                if (d.ShowDialog() == DialogResult.OK)
                {
                    Recipe.LoadFromFile(d.FileName);
                    Recipe.AssignToGrid(dataGridView1, RecipeKindFlag.None);
                }
            } 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog d = new SaveFileDialog())
            {
                d.DefaultExt = ".rcp";
                d.Filter = "Recipe Files (*.rcp)|*.rcp";
                d.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                if (d.ShowDialog() == DialogResult.OK)
                {
                    Recipe.SaveToFile(d.FileName);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnPmacIO_Click(object sender, EventArgs e)
        {
            Recipe.AddFromPMAC(@"..\DEFINE_IO.h", RecipeKindFlag.IOX | RecipeKindFlag.IOY);

            Recipe.AssignToGrid(dataGridView1, RecipeKindFlag.None);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Recipe.RegisterProperty(pgIO, RecipeKindFlag.Param);
        }

        private void btnFixed_Click(object sender, EventArgs e)
        {
            Recipe.RegisterProperty(pgIO, RecipeKindFlag.Fixed);
        }

        private void btnInputIO_Click(object sender, EventArgs e)
        {
            Recipe.RegisterProperty(pgIO, RecipeKindFlag.IOX);
        }

        private void btnOutputIO_Click(object sender, EventArgs e)
        {
            Recipe.RegisterProperty(pgIO, RecipeKindFlag.IOY);
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            Recipe.Backup();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            Recipe.Restore();
            Recipe.AssignToGrid(dataGridView1, RecipeKindFlag.None);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // RegisterGrid를 썼다면 이런 기능은 필요가 없음
            Recipe.AssignFromGrid(dataGridView1);
        }

        private void btnAssignFromControl_Click(object sender, EventArgs e)
        {
            Recipe.AssignFromControls(tabPageControl);    
        }

        private void btnAssignToControl_Click(object sender, EventArgs e)
        {
            Recipe.AssignToControls(tabPageControl);
        }
    }

    public class TestRecipe : Recipe
    {
        protected override void ReadBinary(System.IO.BinaryReader reader, ref int ver)
        {
            base.ReadBinary(reader, ref ver);

            // TODO : additional data reading
        }

        protected override void WriteBinary(System.IO.BinaryWriter writer)
        {
            base.WriteBinary(writer);

            // TODO : additional data writing
        }
    }
}
