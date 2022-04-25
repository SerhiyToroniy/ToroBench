using System;
using System.Drawing;
using System.Windows.Forms;

namespace Benchmark
{
    public partial class ErrorForm : Form
    {
        Guna.UI2.WinForms.Guna2Button b1;
        Guna.UI2.WinForms.Guna2Button b2;
        ToolStripMenuItem t1;
        ToolStripMenuItem t2;
        Form m;
        string errortext;

        public ErrorForm(Form main, Guna.UI2.WinForms.Guna2Button button1, Guna.UI2.WinForms.Guna2Button button2, ToolStripMenuItem tool1, ToolStripMenuItem tool2, Color b, string error_text, string title, string img)
        {
            InitializeComponent();
            b1 = button1;
            b2 = button2;
            t1 = tool1;
            t2 = tool2;
            m = main;
            Text = title;
            errortext = error_text;
            label1.Text = error_text;
            pictureBox1.Image = Image.FromFile($"img/{img}");
            if (img == "Downloaded.png")
            {
                label1.Font = new Font("Segoe UI", 8);
            }
            if (b == Color.DimGray)
            {
                BackColor = Color.DimGray;
                ForeColor = Color.White;
            }
            if (b == Color.White)
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {

        }

        private void ErrorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BackColor == Color.DimGray)
            {
                b1.BackColor = Color.Black;
                b2.BackColor = Color.Black;
            }
            b1.Enabled = true;
            b2.Enabled = true;
            t1.Enabled = true;
            t2.Enabled = true;
            if (errortext.Contains("CPU"))
                m.Close();
        }
    }
}
