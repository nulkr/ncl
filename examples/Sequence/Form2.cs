using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using j2k;

namespace j2kTestSequence
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            Program.Recipe = new Recipe();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            RecipeItem i = new RecipeItem();
            Program.Recipe.Dictionary.Add("aaa", i);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            RecipeItem i = new RecipeItem();
            Program.Recipe.Dictionary.Add("aaa", i);
        }
    }
}
