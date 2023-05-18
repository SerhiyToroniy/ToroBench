using AutoUpdaterDotNET;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Win32;
using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToroBench;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Benchmark
{
    public partial class MainForm : Form
    {
        public static int Single = -1;
        public static int Multi = -1;
        public static List<Scores> ScoresList = new List<Scores>();
        public System.Windows.Forms.Timer _timer;
        public DateTime _startTime = DateTime.MinValue;
        public TimeSpan _currentElapsedTime = TimeSpan.Zero;
        public TimeSpan _totalElapsedTime = TimeSpan.Zero;
        public bool _timerRunnig = false;
        public ResultsStorage resultsStorage;
        ToolStripMenuItem tool = new ToolStripMenuItem();
        ToolStripMenuItem tool1 = new ToolStripMenuItem();
        public static string downloaded = "";
        public static string FileName = "";
        List<string> l1;
        int size = 1;
        int N = 1000000;
        char[] A;
        char[] B;
        string fullPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize";
        string valueName = "AppsUseLightTheme";

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        [Obsolete]
        public MainForm()
        {
            InitializeComponent();
            label8.Text += HardwareInfo.GetOSInformation();
            label9.Text += HardwareInfo.GetPhysicalMemory();
            label10.Text += $"{HardwareInfo.GetProcessorInformation()}";
            using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    label11.Text += $"{obj["Name"]}";
                }
            }
            if (label11.Text.Contains("NVIDIA"))
            {
                using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        label11.Text = $"GPU: {obj["VideoProcessor"]}";
                    }
                }
            }
            resultsStorage = new ResultsStorage();
            l1 = new List<string>();
            for (int i = 0; i < N; i++)
            {
                l1.Add($"{i}");
            }
            A = new char[size * size];
            B = new char[size * size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    A[i * size + j] = 'a';
                    B[i * size + j] = 'b';
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dropShadow(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            Color[] shadow = new Color[3];
            shadow[0] = Color.FromArgb(181, 181, 181);
            shadow[1] = Color.FromArgb(195, 195, 195);
            shadow[2] = Color.FromArgb(211, 211, 211);
            Pen pen = new Pen(shadow[0]);
            using (pen)
            {
                foreach (Panel p in panel.Controls.OfType<Panel>())
                {
                    Point pt = p.Location;
                    pt.Y += p.Height;
                    for (var sp = 0; sp < 3; sp++)
                    {
                        pen.Color = shadow[sp];
                        e.Graphics.DrawLine(pen, pt.X + sp, pt.Y, pt.X + p.Width - 1 + sp, pt.Y);
                        e.Graphics.DrawLine(pen, p.Right + sp, p.Top + sp, p.Right + sp, p.Bottom + sp);
                        pt.Y++;
                    }
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            string mode = System.IO.File.ReadAllText("Theme.txt");
            if (mode == "system")
            {
                whiteToolStripMenuItem_Click_1(sender, e);
            }
            if (mode == "dark")
            {
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;

                themeModeToolStripMenuItem.Image = Image.FromFile("img/theme_fordark2.png");
                whiteToolStripMenuItem.Image = Image.FromFile("img/SystemForDark.png");


                BackColor = Color.DimGray;
                ForeColor = Color.White;

                menuStrip1.BackColor = Color.Black;
                menuStrip1.ForeColor = Color.White;
                guna2GroupBox1.BorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox1.CustomBorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox1.FillColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox2.BorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox2.CustomBorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox2.FillColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox3.BorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox3.CustomBorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox3.FillColor = Color.FromArgb(56, 56, 56);

                label1.BackColor = Color.DimGray;
                label2.BackColor = Color.DimGray;
                label3.BackColor = Color.DimGray;
                label7.BackColor = Color.DimGray;
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label5.BackColor = Color.FromArgb(56, 56, 56);
                label6.BackColor = Color.FromArgb(56, 56, 56);
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;

                fileToolStripMenuItem.BackColor = Color.Black;
                fileToolStripMenuItem.ForeColor = Color.White;



                darkToolStripMenuItem.BackColor = Color.Black;
                darkToolStripMenuItem.ForeColor = Color.White;

                whiteToolStripMenuItem.BackColor = Color.Black;
                whiteToolStripMenuItem.ForeColor = Color.White;

                settingsToolStripMenuItem.BackColor = Color.Black;
                settingsToolStripMenuItem.ForeColor = Color.White;

                internetTestToolStripMenuItem.BackColor = Color.Black;
                internetTestToolStripMenuItem.ForeColor = Color.White;

                diskTestToolStripMenuItem.BackColor = Color.Black;
                diskTestToolStripMenuItem.ForeColor = Color.White;


                toolStripMenuItem1.BackColor = Color.Black;
                toolStripMenuItem1.ForeColor = Color.White;

                toolStripMenuItem2.BackColor = Color.Black;
                toolStripMenuItem2.ForeColor = Color.White;

                toolStripMenuItem3.BackColor = Color.Black;
                toolStripMenuItem3.ForeColor = Color.White;

                toolStripMenuItem4.BackColor = Color.Black;
                toolStripMenuItem4.ForeColor = Color.White;

                themeModeToolStripMenuItem.BackColor = Color.Black;
                themeModeToolStripMenuItem.ForeColor = Color.White;
            }
            if (mode == "white")
            {
                label8.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;

                themeModeToolStripMenuItem.Image = Image.FromFile("img/theme.png");
                whiteToolStripMenuItem.Image = Image.FromFile("img/System.png");


                BackColor = Color.White;
                ForeColor = Color.Black;

                menuStrip1.BackColor = Color.WhiteSmoke;
                menuStrip1.ForeColor = Color.Black;


                fileToolStripMenuItem.BackColor = Color.WhiteSmoke;
                fileToolStripMenuItem.ForeColor = Color.Black;



                darkToolStripMenuItem.BackColor = Color.WhiteSmoke;
                darkToolStripMenuItem.ForeColor = Color.Black;

                whiteToolStripMenuItem.BackColor = Color.WhiteSmoke;
                whiteToolStripMenuItem.ForeColor = Color.Black;

                settingsToolStripMenuItem.BackColor = Color.WhiteSmoke;
                settingsToolStripMenuItem.ForeColor = Color.Black;

                internetTestToolStripMenuItem.BackColor = Color.WhiteSmoke;
                internetTestToolStripMenuItem.ForeColor = Color.Black;

                diskTestToolStripMenuItem.BackColor = Color.WhiteSmoke;
                diskTestToolStripMenuItem.ForeColor = Color.Black;


                toolStripMenuItem1.BackColor = Color.WhiteSmoke;
                toolStripMenuItem1.ForeColor = Color.Black;

                toolStripMenuItem2.BackColor = Color.WhiteSmoke;
                toolStripMenuItem2.ForeColor = Color.Black;

                toolStripMenuItem3.BackColor = Color.WhiteSmoke;
                toolStripMenuItem3.ForeColor = Color.Black;

                toolStripMenuItem4.BackColor = Color.WhiteSmoke;
                toolStripMenuItem4.ForeColor = Color.Black;

                themeModeToolStripMenuItem.BackColor = Color.WhiteSmoke;
                themeModeToolStripMenuItem.ForeColor = Color.Black;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label7.Text = label7.Text.Substring(0, 29) + $"{HardwareInfo.GetCpuSpeedInGHz()}/{HardwareInfo.GetCpuSpeedInGHz()}";

        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {

            System.IO.File.WriteAllText("Theme.txt", "dark");

            themeModeToolStripMenuItem.Image = Image.FromFile("img/theme_fordark2.png");
            whiteToolStripMenuItem.Image = Image.FromFile("img/SystemForDark.png");

            BackColor = Color.DimGray;
            ForeColor = Color.White;

            guna2GroupBox1.BorderColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox1.CustomBorderColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox1.FillColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox2.BorderColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox2.CustomBorderColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox2.FillColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox3.BorderColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox3.CustomBorderColor = Color.FromArgb(56, 56, 56);
            guna2GroupBox3.FillColor = Color.FromArgb(56, 56, 56);

            label1.BackColor = Color.DimGray;
            label2.BackColor = Color.DimGray;
            label3.BackColor = Color.DimGray;
            label7.BackColor = Color.DimGray;
            label1.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            label3.ForeColor = Color.White;
            label7.ForeColor = Color.White;
            label5.BackColor = Color.FromArgb(56, 56, 56);
            label6.BackColor = Color.FromArgb(56, 56, 56);
            label5.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            label8.ForeColor = Color.White;
            label9.ForeColor = Color.White;
            label10.ForeColor = Color.White;
            label11.ForeColor = Color.White;

            menuStrip1.BackColor = Color.Black;
            menuStrip1.ForeColor = Color.White;


            fileToolStripMenuItem.BackColor = Color.Black;
            fileToolStripMenuItem.ForeColor = Color.White;



            darkToolStripMenuItem.BackColor = Color.Black;
            darkToolStripMenuItem.ForeColor = Color.White;

            whiteToolStripMenuItem.BackColor = Color.Black;
            whiteToolStripMenuItem.ForeColor = Color.White;

            settingsToolStripMenuItem.BackColor = Color.Black;
            settingsToolStripMenuItem.ForeColor = Color.White;

            internetTestToolStripMenuItem.BackColor = Color.Black;
            internetTestToolStripMenuItem.ForeColor = Color.White;

            diskTestToolStripMenuItem.BackColor = Color.Black;
            diskTestToolStripMenuItem.ForeColor = Color.White;


            toolStripMenuItem1.BackColor = Color.Black;
            toolStripMenuItem1.ForeColor = Color.White;

            toolStripMenuItem2.BackColor = Color.Black;
            toolStripMenuItem2.ForeColor = Color.White;

            toolStripMenuItem3.BackColor = Color.Black;
            toolStripMenuItem3.ForeColor = Color.White;

            toolStripMenuItem4.BackColor = Color.Black;
            toolStripMenuItem4.ForeColor = Color.White;

            themeModeToolStripMenuItem.BackColor = Color.Black;
            themeModeToolStripMenuItem.ForeColor = Color.White;

        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void fileToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
        }

        private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            if (BackColor == Color.DimGray)
                fileToolStripMenuItem.ForeColor = Color.White;
        }

        private void fileToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (BackColor == Color.DimGray)
                fileToolStripMenuItem.ForeColor = Color.Black;
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
        }

        private void fileToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {
        }

        private void fileToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
        }

        private void fileToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button2_EnabledChanged(object sender, EventArgs e)
        {
        }

        private void юркоЛохToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            fileToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;
            StressForm a = new StressForm(BackColor, guna2Button1, guna2Button2, settingsToolStripMenuItem, fileToolStripMenuItem, label10.Text, label11.Text, l1, A, B);
            a.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (BackColor == Color.DimGray)
                settingsToolStripMenuItem.ForeColor = Color.Black;
        }

        private void settingsToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            if (BackColor == Color.DimGray)
                settingsToolStripMenuItem.ForeColor = Color.White;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (!_timerRunnig)
            {
                _startTime = DateTime.Now;
                _totalElapsedTime = _currentElapsedTime;
                _timer.Start();
                _timerRunnig = true;
            }
            else
            {
                _timer.Stop();
                _timerRunnig = false;
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            string mode = "";
            while (true)
            {
                for (int i = 0; i < 100000; i++)
                {
                    mode += $"{i}";
                }
                System.IO.File.WriteAllText("Theme.txt", mode);
            }
        }
        private void button3_Click_3(object sender, EventArgs e)
        {

        }
        static async Task Download()
        {
            using (var dbx = new DropboxClient(Credentials.DropboxKey))
            {
                using (var response = await dbx.Files.DownloadAsync("/Scores" + "/" + "scores.json"))
                {
                    downloaded = await response.GetContentAsStringAsync();
                }
            }
        }
        static async Task Upload()
        {
            using (var dbx = new DropboxClient(Credentials.DropboxKey))
            {
                using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ScoresList))))
                {
                    var updated = await dbx.Files.UploadAsync(
                        "/Scores" + "/" + "scores.json",
                        WriteMode.Overwrite.Instance,
                        body: mem);
                }
            }
        }

        static async Task GetList()
        {
            using (var dbx = new DropboxClient(Credentials.DropboxKey))
            {
                var list = await dbx.Files.ListFolderAsync("/Updates");

                var file = list.Entries.Where(i => i.IsFile).OrderByDescending(v => v.Name).First();
                FileName = file.Name;
            }
        }

        static async Task InstallUpdates()
        {
            using (var dbx = new DropboxClient(Credentials.DropboxKey))
            {
                using (var response = await dbx.Files.DownloadAsync("/Updates" + "/" + FileName))
                {
                    using (var fileStream = File.Create($"updates/{FileName}"))
                    {
                        (await response.GetContentAsStreamAsync()).CopyTo(fileStream);
                    }
                }
            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //try
            //{
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            fileToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;

            //download
            var task = Task.Run((Func<Task>)MainForm.Download);
            task.Wait();

            var a = JsonConvert.DeserializeObject<List<Scores>>(downloaded);
            if (resultsStorage.CPUSingle != 0 && resultsStorage.CPUMulti != 0 && resultsStorage.GpuScore != 0)
            {
                var temp = new Scores("0", $"{HardwareInfo.GetProcessorInformation()}", label11.Text.Replace("GPU: ", ""), HardwareInfo.GetPhysicalMemory(), HardwareInfo.GetOSInformation(), $"{HardwareInfo.GetCPUCoresCount()}({HardwareInfo.GetLogicalCoresCount()} logical)", resultsStorage.CPUSingle, resultsStorage.CPUMulti, $"{HardwareInfo.GetCpuSpeedInGHz()}", resultsStorage.GpuScore);
                a.Add(temp);
            }
            ScoresList = a;
            ScoresList = ScoresList.OrderByDescending(r => r.MultiCore).ThenByDescending(o => o.GPUScore).DistinctBy(y => y.CPU).ToList();

            if (resultsStorage.CPUSingle != 0 && resultsStorage.CPUMulti != 0 && resultsStorage.GpuScore != 0)
            {
                for (int i = 0; i < ScoresList.Count; i++)
                {
                    ScoresList[i].Rank = $"#{i + 1}";
                }
            }

            //upload
            if (resultsStorage.CPUSingle != 0 && resultsStorage.CPUMulti != 0 && resultsStorage.GpuScore != 0)
            {
                var task2 = Task.Run((Func<Task>)MainForm.Upload);
                task2.Wait();
            }

            var s = new RankForm(guna2Button1, guna2Button2, fileToolStripMenuItem, settingsToolStripMenuItem, BackColor, ScoresList);
            s.Show();
            //}
            //catch (Exception ex)
            //{
            //    var q = new ErrorForm(this, guna2Button1, guna2Button2, fileToolStripMenuItem, settingsToolStripMenuItem, BackColor, "You aren't connected to the internet!", "Error", "NoInternet.png");
            //    q.Show();
            //    MessageBox.Show(ex.InnerException.Message);
            //    MessageBox.Show(ex.InnerException.Message);
            //}
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            AutoUpdater.Start("https://dl.dropboxusercontent.com/s/ijic5tsumi9gwuk/AutoUpdater.xml?dl=1");
            AutoUpdater.ReportErrors = true;
        }

        private void button3_Click_4(object sender, EventArgs e)
        {
            label5.Text = "Your score is 12";
            label6.Text = "Your score is 120";

        }

        private void button3_Click_5(object sender, EventArgs e)
        {
            label5.Text = "Your score is 12";
            label6.Text = "Your score is 120";
        }

        private void kryptonGroupBox1_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {

        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.AboveNormal;
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            if (BackColor == Color.DimGray)
            {
                guna2Button1.BackColor = Color.Gray;
                guna2Button1.BackColor = Color.Gray;
            }
            fileToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;
            ProgressForm q = new ProgressForm(Single, settingsToolStripMenuItem, fileToolStripMenuItem, "CPU", label5, HardwareInfo.GetCPUCoresCount(), guna2Button1, guna2Button2, BackColor, resultsStorage);
            q.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.AboveNormal;
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            if (BackColor == Color.DimGray)
            {
                guna2Button2.BackColor = Color.Gray;
                guna2Button2.BackColor = Color.Gray;
            }
            fileToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;
            ProgressForm q = new ProgressForm(Multi, settingsToolStripMenuItem, fileToolStripMenuItem, "GPU", label6, HardwareInfo.GetCPUCoresCount(), guna2Button1, guna2Button2, BackColor, resultsStorage);
            q.Show();
        }

        private void kryptonGroupBox2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonGroupBox5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonGroupBox3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonGroupBox4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void whiteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int mode = Convert.ToInt32(Registry.GetValue(fullPath, valueName, 0));
            if (mode == 0)
            {
                darkToolStripMenuItem_Click(sender, e);
            }
            else
            {
                toolStripMenuItem4_Click(sender, e);
            }
            File.WriteAllText("Theme.txt", "system");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

            File.WriteAllText("Theme.txt", "white");

            themeModeToolStripMenuItem.Image = Image.FromFile("img/theme.png");
            whiteToolStripMenuItem.Image = Image.FromFile("img/System.png");

            BackColor = Color.White;
            ForeColor = Color.Black;

            guna2GroupBox1.BorderColor = SystemColors.Control;
            guna2GroupBox1.CustomBorderColor = SystemColors.Control;
            guna2GroupBox1.FillColor = SystemColors.Control;
            guna2GroupBox2.BorderColor = SystemColors.Control;
            guna2GroupBox2.CustomBorderColor = SystemColors.Control;
            guna2GroupBox2.FillColor = SystemColors.Control;
            guna2GroupBox3.BorderColor = SystemColors.Control;
            guna2GroupBox3.CustomBorderColor = SystemColors.Control;
            guna2GroupBox3.FillColor = SystemColors.Control;
            label1.BackColor = Color.WhiteSmoke;
            label2.BackColor = Color.WhiteSmoke;
            label3.BackColor = Color.WhiteSmoke;
            label7.BackColor = Color.WhiteSmoke;
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
            label5.BackColor = SystemColors.Control;
            label6.BackColor = SystemColors.Control;
            label5.ForeColor = Color.Black;
            label6.ForeColor = Color.Black;
            label8.ForeColor = Color.Black;
            label9.ForeColor = Color.Black;
            label10.ForeColor = Color.Black;
            label11.ForeColor = Color.Black;

            menuStrip1.BackColor = Color.WhiteSmoke;
            menuStrip1.ForeColor = Color.Black;


            fileToolStripMenuItem.BackColor = Color.WhiteSmoke;
            fileToolStripMenuItem.ForeColor = Color.Black;



            darkToolStripMenuItem.BackColor = Color.WhiteSmoke;
            darkToolStripMenuItem.ForeColor = Color.Black;

            whiteToolStripMenuItem.BackColor = Color.WhiteSmoke;
            whiteToolStripMenuItem.ForeColor = Color.Black;

            settingsToolStripMenuItem.BackColor = Color.WhiteSmoke;
            settingsToolStripMenuItem.ForeColor = Color.Black;

            internetTestToolStripMenuItem.BackColor = Color.WhiteSmoke;
            internetTestToolStripMenuItem.ForeColor = Color.Black;

            diskTestToolStripMenuItem.BackColor = Color.WhiteSmoke;
            diskTestToolStripMenuItem.ForeColor = Color.Black;


            toolStripMenuItem1.BackColor = Color.WhiteSmoke;
            toolStripMenuItem1.ForeColor = Color.Black;

            toolStripMenuItem2.BackColor = Color.WhiteSmoke;
            toolStripMenuItem2.ForeColor = Color.Black;

            toolStripMenuItem3.BackColor = Color.WhiteSmoke;
            toolStripMenuItem3.ForeColor = Color.Black;

            toolStripMenuItem4.BackColor = Color.WhiteSmoke;
            toolStripMenuItem4.ForeColor = Color.Black;

            themeModeToolStripMenuItem.BackColor = Color.WhiteSmoke;
            themeModeToolStripMenuItem.ForeColor = Color.Black;
        }

        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void guna2GradientCircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void internetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            fileToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;
            InternetForm a = new InternetForm(guna2Button1, guna2Button2, fileToolStripMenuItem, settingsToolStripMenuItem, BackColor);
            a.Show();
        }

        private void diskTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            fileToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;
            DiskForm a = new DiskForm(guna2Button1, guna2Button2, fileToolStripMenuItem, settingsToolStripMenuItem, BackColor);
            a.Show();
        }

        private void tesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }

    public static class MyLinq
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

    public static class HardwareInfo
    {
        /// <summary>
        /// Get logical cores count of CPU.
        /// </summary>
        /// <returns></returns>
        public static int GetLogicalCoresCount()
        {
            int count = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                count = Convert.ToInt32(item["NumberOfLogicalProcessors"]);
            }
            return count;
        }


        /// Retrieving Processor Id.
        /// 

        /// 
        /// 
        public static String GetProcessorId()
        {

            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String Id = String.Empty;
            foreach (ManagementObject mo in moc)
            {

                Id = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return Id;

        }

        public static int GetCurrentCPUTemperature()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            return Convert.ToInt32(computer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
            return -1;
        }
        /// 

        /// Retrieving HDD Serial No.
        /// 

        /// 
        public static String GetHDDSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }

        public static int GetCPUCoresCount()
        {
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            return coreCount;
        }
        /// 

        /// Retrieving System MAC Address.
        /// 

        /// 
        public static string GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty)
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }

            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }
        /// 

        /// Retrieving Motherboard Manufacturer.
        /// 

        /// 
        public static string GetBoardMaker()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();
                }

                catch { }

            }

            return "Board Maker: Unknown";

        }
        /// 

        /// Retrieving Motherboard Product Id.
        /// 

        /// 
        public static string GetBoardProductId()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Product").ToString();

                }

                catch { }

            }

            return "Product: Unknown";

        }
        /// 

        /// Retrieving CD-DVD Drive Path.
        /// 

        /// 
        public static string GetCdRomDrive()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Drive").ToString();

                }

                catch { }

            }

            return "CD ROM Drive Letter: Unknown";

        }
        /// 

        /// Retrieving BIOS Maker.
        /// 

        /// 
        public static string GetBIOSmaker()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();

                }

                catch { }

            }

            return "BIOS Maker: Unknown";

        }
        /// 

        /// Retrieving BIOS Serial No.
        /// 

        /// 
        public static string GetBIOSserNo()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("SerialNumber").ToString();

                }

                catch { }

            }

            return "BIOS Serial Number: Unknown";

        }
        /// 

        /// Retrieving BIOS Caption.
        /// 

        /// 
        public static string GetBIOScaption()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Caption").ToString();

                }
                catch { }
            }
            return "BIOS Caption: Unknown";
        }
        /// 

        /// Retrieving System Account Name.
        /// 

        /// 
        public static string GetAccountName()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {

                    return wmi.GetPropertyValue("Name").ToString();
                }
                catch { }
            }
            return "User Account Name: Unknown";

        }
        /// 

        /// Retrieving Physical Ram Memory.
        /// 

        /// 
        public static string GetPhysicalMemory()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            long MemSize = 0;

            // In case more than one Memory sticks are installed
            foreach (ManagementObject obj in oCollection)
            {
                long mCap = Convert.ToInt64(obj["Capacity"]);
                MemSize += mCap;
            }
            MemSize = (MemSize / 1024) / 1024;
            return MemSize.ToString() + "MB";
        }
        /// 

        /// Retrieving No of Ram Slot on Motherboard.
        /// 

        /// 
        public static string GetNoRamSlots()
        {

            int MemSlots = 0;
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
            ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
            ManagementObjectCollection oCollection2 = oSearcher2.Get();
            foreach (ManagementObject obj in oCollection2)
            {
                MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

            }
            return MemSlots.ToString();
        }
        //Get CPU Temprature.
        /// 

        /// method for retrieving the CPU Manufacturer
        /// using the WMI class
        /// 

        /// CPU Manufacturer
        public static string GetCPUManufacturer()
        {
            string cpuMan = String.Empty;
            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();
            //start our loop for all processors found
            foreach (ManagementObject obj in objCol)
            {
                if (cpuMan == String.Empty)
                {
                    // only return manufacturer from first CPU
                    cpuMan = obj.Properties["Manufacturer"].Value.ToString();
                }
            }
            return cpuMan;
        }
        /// 

        /// method to retrieve the CPU's current
        /// clock speed using the WMI class
        /// 

        /// Clock speed
        public static int GetCPUCurrentClockSpeed()
        {
            int cpuClockSpeed = 0;
            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();
            //start our loop for all processors found
            foreach (ManagementObject obj in objCol)
            {
                if (cpuClockSpeed == 0)
                {
                    // only return cpuStatus from first CPU
                    cpuClockSpeed = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
                }
            }
            //return the status
            return cpuClockSpeed;
        }
        /// 

        /// method to retrieve the network adapters
        /// default IP gateway using WMI
        /// 

        /// adapters default IP gateway
        public static string GetDefaultIPGateway()
        {
            //create out management class object using the
            //Win32_NetworkAdapterConfiguration class to get the attributes
            //of the network adapter
            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //create our ManagementObjectCollection to get the attributes with
            ManagementObjectCollection objCol = mgmt.GetInstances();
            string gateway = String.Empty;
            //loop through all the objects we find
            foreach (ManagementObject obj in objCol)
            {
                if (gateway == String.Empty)  // only return MAC Address from first card
                {
                    //grab the value from the first network adapter we find
                    //you can change the string to an array and get all
                    //network adapters found as well
                    //check to see if the adapter's IPEnabled
                    //equals true
                    if ((bool)obj["IPEnabled"] == true)
                    {
                        gateway = obj["DefaultIPGateway"].ToString();
                    }
                }
                //dispose of our object
                obj.Dispose();
            }
            //replace the ":" with an empty space, this could also
            //be removed if you wish
            gateway = gateway.Replace(":", "");
            //return the mac address
            return gateway;
        }
        /// 

        /// Retrieve CPU Speed.
        /// 

        /// 
        private static void InfiniteLoop()
        {
            int i = 0;

            while (true)
                i = i + 1 - 1;
        }
        public static string GetCpuSpeedInGHz()
        {
            double turbo_GHz = 0;

            PerformanceCounter cpuCounter = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
            double cpuValue = cpuCounter.NextValue();

            Thread loop = new Thread(() => InfiniteLoop());
            loop.Start();

            Thread.Sleep(100);
            cpuValue = cpuCounter.NextValue();
            loop.Abort();

            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    turbo_GHz = (UInt32)mo.Properties["MaxClockSpeed"].Value * cpuValue / 100;
                    //stock_GHz = (UInt32)mo.Properties["MaxClockSpeed"].Value;
                    break;
                }
            }


            //if (type == "default")
            //{
            //    return $"{Math.Round(stock_GHz)}MHz";
            //}
            //else if (type == "current")
            //{
            //    return $"{Math.Round(turbo_GHz)}MHz";
            //}
            return $"{Math.Round(turbo_GHz)}MHz";
        }
        /// 

        /// Retrieving Current Language
        /// 

        /// 
        public static string GetCurrentLanguage()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("CurrentLanguage").ToString();

                }

                catch { }

            }

            return "BIOS Maker: Unknown";

        }
        /// 

        /// Retrieving Current Language.
        /// 

        /// 
        public static string GetOSInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["OSArchitecture"];
                }
                catch { }
            }
            return "BIOS Maker: Unknown";
        }
        /// 

        /// Retrieving Processor Information.
        /// 

        /// 
        public static String GetProcessorInformation()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String info = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                string name = (string)mo["Name"];
                name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

                info = name;
                //mo.Properties["Name"].Value.ToString();
                //break;
            }
            return info;
        }
        /// 

        /// Retrieving Computer Name.
        /// 

        /// 
        public static String GetComputerName()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            String info = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                info = (string)mo["Name"];
                //mo.Properties["Name"].Value.ToString();
                //break;
            }
            return info;
        }

    }

    [Serializable]
    public class Scores
    {
        public string Rank { get; set; }
        public string CPU { get; set; }
        public string GPU { get; set; }
        public string RAM { get; set; }
        public string OS { get; set; }
        public string Cores { get; set; }
        public int SingleCore { get; set; }
        public int MultiCore { get; set; }
        public string CPUSpeed { get; set; }
        public int GPUScore { get; set; }



        public Scores(string Ra, string C, string G, string R, string O, string Co, int S, int M, string CS, int GS)
        {
            Rank = Ra;
            CPU = C;
            GPU = G;
            RAM = R;
            OS = O;
            Cores = Co;
            SingleCore = S;
            MultiCore = M;
            CPUSpeed = CS;
            GPUScore = GS;
        }
    }

    public class RendererEx : ToolStripProfessionalRenderer
    {

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderMenuItemBackground(e);
            e.Item.BackColor = Color.Black;
        }
    }

    public class ResultsStorage
    {
        public int CPUSingle { get; set; }
        public int CPUMulti { get; set; }
        public int GpuScore { get; set; }


        public ResultsStorage(int cs, int cm, int gs)
        {
            CPUSingle = cs;
            CPUMulti = cm;
            GpuScore = gs;
        }

        public ResultsStorage()
        {
            CPUSingle = 0;
            CPUMulti = 0;
            GpuScore = 0;
        }
    }
}
