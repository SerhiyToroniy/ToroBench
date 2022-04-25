using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Benchmark
{
    public partial class RankForm : Form
    {
        public List<Scores> l { get; set; }
        public Guna.UI2.WinForms.Guna2Button button2 { get; set; }

        public Guna.UI2.WinForms.Guna2Button button1 { get; set; }
        public ToolStripMenuItem file { get; set; }
        public ToolStripMenuItem settings { get; set; }
        public Color backcolor { get; set; }

        public RankForm(Guna.UI2.WinForms.Guna2Button b1, Guna.UI2.WinForms.Guna2Button b2, ToolStripMenuItem f, ToolStripMenuItem s, Color b, List<Scores> list)
        {
            InitializeComponent();
            button1 = b1;
            button2 = b2;
            file = f;
            settings = s;
            dataGridView1.Update();
            dataGridView1.Refresh();
            dataGridView1.DataSource = list;
            l = list;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            backcolor = b;
            if (b == Color.DimGray)
            {
                textBox1.BackColor = Color.Black;
                BackColor = Color.DimGray;
                dataGridView1.DefaultCellStyle.BackColor = Color.Black;
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Black;
                dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Black;
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Black;
                dataGridView1.RowHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                ForeColor = Color.White;
            }
            if (b == Color.White)
            {
                textBox1.BackColor = Color.White;
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
            textBox1.KeyDown += new KeyEventHandler(tb_KeyDown);
        }

        public void tb_KeyDown(object sender, KeyEventArgs f)
        {
            if (f.KeyCode == Keys.Enter)
            {
                f.Handled = f.SuppressKeyPress = true;
                dataGridView1.Update();
                dataGridView1.Refresh();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = l.Where(e => (e.Cores.ToUpper().Contains(textBox1.Text.ToUpper())) || (e.CPU.ToUpper().Contains(textBox1.Text.ToUpper())) || (e.CPUSpeed.ToUpper().Contains(textBox1.Text.ToUpper())) || ($"{e.MultiCore}".ToUpper().Contains(textBox1.Text.ToUpper())) || (e.OS.ToUpper().Contains(textBox1.Text.ToUpper())) || (e.RAM.ToUpper().Contains(textBox1.Text.ToUpper())) || ($"{e.SingleCore}".ToUpper().Contains(textBox1.Text.ToUpper()))).ToList();
                dataGridView1.Update();
                dataGridView1.Refresh();
            }
        }
        private void RankForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (backcolor == Color.White)
            {
                textBox1.ForeColor = Color.Black;
                textBox1.Text = "";
            }
            if (backcolor == Color.DimGray)
            {
                textBox1.ForeColor = Color.White;
                textBox1.Text = "";
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

        }

        private void RankForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            file.Enabled = true;
            settings.Enabled = true;
            if (BackColor == Color.DimGray)
            {
                button1.BackColor = Color.Black;
                button2.BackColor = Color.Black;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.ForeColor = Color.LightGray;
                textBox1.Text = "CPU name, score result or anything else..";
            }
        }
    }
}
