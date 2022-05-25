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



        private bool mouseDown;
        private Point lastLocation;
        public List<Scores> l { get; set; }
        public Guna.UI2.WinForms.Guna2Button button2 { get; set; }

        public Guna.UI2.WinForms.Guna2Button button1 { get; set; }
        public ToolStripMenuItem file { get; set; }
        public ToolStripMenuItem settings { get; set; }
        public Color backcolor { get; set; }
        private bool sortAscending = false;

        public RankForm(Guna.UI2.WinForms.Guna2Button b1, Guna.UI2.WinForms.Guna2Button b2, ToolStripMenuItem f, ToolStripMenuItem s, Color b, List<Scores> list)
        {
            InitializeComponent();
            button1 = b1;
            button2 = b2;
            file = f;
            settings = s;
            guna2DataGridView1.Update();
            guna2DataGridView1.Refresh();
            guna2DataGridView1.DataSource = list;
            l = list;
            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            backcolor = b;
            if (b == Color.DimGray)
            {
                guna2TextBox1.ShadowDecoration.Color = Color.White;
                BackColor = Color.DimGray;

                guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                guna2DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Black;
                guna2DataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;

                ForeColor = Color.White;
            }
            if (b == Color.White)
            {
                guna2Button1.ShadowDecoration.Color = Color.Black;
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
            guna2TextBox1.KeyDown += new KeyEventHandler(tb_KeyDown);
        }

        public void tb_KeyDown(object sender, KeyEventArgs f)
        {
            if (f.KeyCode == Keys.Enter)
            {
                f.Handled = f.SuppressKeyPress = true;
                guna2DataGridView1.Update();
                guna2DataGridView1.Refresh();
                guna2DataGridView1.DataSource = null;
                guna2DataGridView1.DataSource = l.Where(e => (e.Cores.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.CPU.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.CPUSpeed.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || ($"{e.MultiCore}".ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.OS.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.RAM.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || ($"{e.SingleCore}".ToUpper().Contains(guna2TextBox1.Text.ToUpper()))).ToList();
                guna2DataGridView1.Update();
                guna2DataGridView1.Refresh();
            }
        }
        private void RankForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (backcolor == Color.White)
            {
                guna2TextBox1.Text = "";
            }
            if (backcolor == Color.DimGray)
            {
                guna2TextBox1.Text = "";
            }
        }

        private void textBox1_Enter(object sender, EventArgs a)
        {
            guna2DataGridView1.Update();
            guna2DataGridView1.Refresh();
            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = l.Where(e => (e.Cores.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.CPU.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.CPUSpeed.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || ($"{e.MultiCore}".ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.OS.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.RAM.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || ($"{e.SingleCore}".ToUpper().Contains(guna2TextBox1.Text.ToUpper()))).ToList();
            guna2DataGridView1.Update();
            guna2DataGridView1.Refresh();
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

        private void textBox1_Leave(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs a)
        {
            guna2DataGridView1.Update();
            guna2DataGridView1.Refresh();
            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = l.Where(e => (e.Cores.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.CPU.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.CPUSpeed.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || ($"{e.MultiCore}".ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.OS.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || (e.RAM.ToUpper().Contains(guna2TextBox1.Text.ToUpper())) || ($"{e.SingleCore}".ToUpper().Contains(guna2TextBox1.Text.ToUpper()))).ToList();
            guna2DataGridView1.Update();
            guna2DataGridView1.Refresh();
        }

        private void guna2DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (sortAscending)
                guna2DataGridView1.DataSource = l.OrderBy(c => guna2DataGridView1.Columns[e.ColumnIndex].DataPropertyName).ToList();
            else
                guna2DataGridView1.DataSource = l.OrderBy(c => guna2DataGridView1.Columns[e.ColumnIndex].DataPropertyName).Reverse().ToList();
            sortAscending = !sortAscending;
        }

        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            guna2DataGridView1.ClearSelection();
        }

        private void RankForm_MouseDown(object sender, MouseEventArgs e)
        {


            mouseDown = true;
            lastLocation = e.Location;
        }

        private void RankForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void RankForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
