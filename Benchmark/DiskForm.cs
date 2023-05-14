using Guna.UI2.WinForms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace ToroBench
{
    public partial class DiskForm : Form
    {
        public Guna2Button button1 { get; set; }
        public Guna2Button button2 { get; set; }
        public ToolStripMenuItem toolstrip1 { get; set; }
        public ToolStripMenuItem toolstrip2 { get; set; }
        public bool stop { get; set; }
        public Color color { get; set; }
        public BackgroundWorker worker { get; set; }
        public bool IsReadNow { get; set; }
        public DiskForm(Guna2Button b1, Guna2Button b2, ToolStripMenuItem t1, ToolStripMenuItem t2, Color c)
        {
            InitializeComponent();
            button1 = b1;
            button2 = b2;
            toolstrip1 = t1;
            toolstrip2 = t2;
            color = c;
            Load += Form1_Load;
            IsReadNow = false;
            if (color == Color.DimGray)
            {
                BackColor = Color.DimGray;
                guna2CircleProgressBar1.InnerColor = Color.DimGray;
                guna2CircleProgressBar4.InnerColor = Color.DimGray;
                ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label1.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
            }
            if (color == Color.White)
            {
                guna2Button1.ShadowDecoration.Color = Color.Black;
                guna2CircleProgressBar1.InnerColor = Color.White;
                guna2CircleProgressBar4.InnerColor = Color.White;
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string pathToConsoleApp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DiskTestConsoleApp\\bin\\Debug\\net7.0\\DiskTestConsoleApp.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pathToConsoleApp,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };
            Process process = new Process { StartInfo = startInfo };
            process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            IsReadNow = false;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data == "*")
                {
                    IsReadNow = true;
                }
                int num;
                if (int.TryParse(e.Data, out num))
                {
                    if (!IsReadNow && num > guna2CircleProgressBar4.Value)
                    {
                        BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar4.Value = num; });
                        BeginInvoke((MethodInvoker)delegate { label10.Text = $"{guna2CircleProgressBar4.Value}%"; });
                    }
                    if (IsReadNow && num > guna2CircleProgressBar1.Value)
                    {
                        BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar1.Value = num; });
                        BeginInvoke((MethodInvoker)delegate { label7.Text = $"{guna2CircleProgressBar1.Value}%"; });
                    }
                }
                if (e.Data.StartsWith("Write:"))
                {
                    BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar4.Value = 100; });
                    BeginInvoke((MethodInvoker)delegate { label10.Text = $"{e.Data.Split()[1]} MB/s"; });
                }
                if (e.Data.StartsWith("Read:"))
                {
                    BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar1.Value = 100; });
                    BeginInvoke((MethodInvoker)delegate { label7.Text = $"{e.Data.Split()[1]} MB/s"; });
                }
            }
        }

        private void guna2GradientCircleButton1_Click(object sender, System.EventArgs e)
        {
            stop = true;
            Close();
        }

        private void guna2Button1_Click(object sender, System.EventArgs e)
        {
            stop = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public string GetDeviceName()
        {
            string deviceName = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                deviceName = $"{queryObj["Model"]}";
                break;
            }

            return deviceName;
        }

        private void DiskForm_Load(object sender, EventArgs e)
        {
            guna2HtmlLabel1.Text = GetDeviceName();
        }

        private void DiskForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            toolstrip1.Enabled = true;
            toolstrip2.Enabled = true;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
        }
    }
}
