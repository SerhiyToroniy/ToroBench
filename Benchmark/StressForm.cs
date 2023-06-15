using OpenCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using LibreHardwareMonitor.Hardware;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Benchmark
{
    public partial class StressForm : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public System.Windows.Forms.Timer _timer;
        public DateTime _startTime = DateTime.MinValue;
        public TimeSpan _currentElapsedTime = TimeSpan.Zero;
        public TimeSpan _totalElapsedTime = TimeSpan.Zero;
        public bool _timerRunnig = false;
        public bool IsReadNow { get; set; }

        BackgroundWorker worker;

        public List<float> floatList { get; set; }

        public List<int> LoadsGPU { get; set; }
        public List<int> LoadsCPU { get; set; }
        public List<int> TempsCPU { get; set; }
        public List<int> TempsGPU { get; set; }
        public List<int> ScoresCPU { get; set; }
        public List<int> ScoresGPU { get; set; }
        public List<int> LoadsDisk { get; set; }
        public List<int> TempsDisk { get; set; }
        public List<int> ScoresDisk { get; set; }
        public float testfloat { get; set; }
        bool stop = false;
        int size = 1;
        OpenCL.OpenCL cl;
        PerformanceCounter cpuCounter;
        Guna.UI2.WinForms.Guna2Button button1_;
        Guna.UI2.WinForms.Guna2Button button2_;
        ToolStripMenuItem tool = new ToolStripMenuItem();
        ToolStripMenuItem tool1 = new ToolStripMenuItem();
        int loadCPU = -1;
        int loadGPU = -1;
        int tempCPU = -1;
        int tempGPU = -1;
        int scoreCPU = -1;
        int scoreGPU = -1;
        List<string> l1 = new List<string>();
        char[] a;
        char[] B;
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stop = false;
            button1_.Enabled = true;
            button2_.Enabled = true;
            if (BackColor == Color.DimGray)
            {
                button1_.BackColor = Color.Black;
                button2_.BackColor = Color.Black;
            }
            tool.Enabled = true;
            tool1.Enabled = true;
            _timer.Stop();
            guna2GradientCircleButton1.Visible = true;

            //Dispose();
        }

        public int getCurrentCpuUsage()
        {
            return Convert.ToInt32(cpuCounter.NextValue());
        }

        public async Task<int> WhileCPU()
        {
            await Task.Run(() =>
            {
                var L = GetListL();
                while (true)
                {
                    if (stop)
                    {
                        break;
                    }
                    _ = Parallel.ForEach(L, i =>
                    {
                        if (stop)
                        {
                            return;
                        }
                        _ = Parallel.For(0, 1000000, j =>
                        {
                            if (stop)
                            {
                                return;
                            }
                        });
                    });
                }
            });

            return 0;
        }
        public async Task<int> WhileGPU()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (stop)
                    {
                        break;
                    }
                    cl.Execute(1);
                }
            });
            return 0;
        }

        private List<List<string>> GetListL()
        {
            List<List<string>> L = new List<List<string>>();
            for (int i = 0; i < 128; i++)
            {
                if (stop)
                    break;
                L.Add(l1);
            }
            return L;
        }

        void worker_DoWork2(object sende, DoWorkEventArgs e)
        {
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var L = GetListL();

            cl.Execute(1);


            while (!stop)
            {
                if (stop)
                    break;

                string pathToConsoleApp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DiskTestConsoleApp\\bin\\Debug\\net7.0\\DiskTestConsoleApp.exe");

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pathToConsoleApp,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = $"{5 * 1024 * 1024}" //20 disk test
                };
                Process process = new Process { StartInfo = startInfo };
                process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                IsReadNow = false;
                if (stop)
                    break;
                UpdateVisitor updateVisitor = new UpdateVisitor();
                OpenHardwareMonitor.Hardware.Computer computer = new OpenHardwareMonitor.Hardware.Computer();
                computer.Open();
                computer.CPUEnabled = true;
                computer.GPUEnabled = true;
                computer.Accept((OpenHardwareMonitor.Hardware.IVisitor)updateVisitor);
                computer.Hardware[0].Update();
                if (stop)
                    break;
                for (int i = 0; i < computer.Hardware.Length; i++)
                {
                    if (computer.Hardware[i].HardwareType == OpenHardwareMonitor.Hardware.HardwareType.CPU)
                    {
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (computer.Hardware[i].Sensors[j].SensorType == OpenHardwareMonitor.Hardware.SensorType.Temperature)
                            {
                                tempCPU = Convert.ToInt32(computer.Hardware[i].Sensors[j].Value);
                            }
                            if (computer.Hardware[i].Sensors[j].SensorType == OpenHardwareMonitor.Hardware.SensorType.Load)
                            {
                                loadCPU = Convert.ToInt32(computer.Hardware[i].Sensors[j].Value);
                            }
                        }
                        break;
                    }
                }
                if (stop)
                    break;
                // Get the GPU hardware
                foreach (var hardware in computer.Hardware)
                {
                    if (hardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.GpuNvidia || hardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.GpuAti)
                    {
                        hardware.Update();
                        // Get the temperature sensor for the GPU
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == OpenHardwareMonitor.Hardware.SensorType.Temperature && sensor.Name.Contains("GPU Core"))
                            {
                                // Get the GPU temperature
                                tempGPU = (int)sensor.Value;
                            }
                        }
                    }
                }
                loadGPU = 100;
                if (stop)
                    break;
                Stopwatch stopWatchCPU = new Stopwatch();
                stopWatchCPU.Start();
                _ = Parallel.ForEach(L, i =>
                {
                    if (stop)
                    {
                        return;
                    }
                    _ = Parallel.For(0, 10000000, j =>
                    {
                        if (stop)
                        {
                            return;
                        }
                    });
                });
                stopWatchCPU.Stop();
                scoreCPU = Convert.ToInt32(10000000 / stopWatchCPU.ElapsedMilliseconds);
                Stopwatch stopWatchGPU = new Stopwatch();
                if (stop)
                {
                    return;
                }
                stopWatchGPU.Start();
                cl.Execute(1);
                stopWatchGPU.Stop();
                scoreGPU = Convert.ToInt32(100000000 / (stopWatchGPU.ElapsedMilliseconds));
                computer.Close();




                if (stop)
                    break;
                LoadsCPU.Add(loadCPU);
                LoadsGPU.Add(loadGPU);
                TempsCPU.Add(tempCPU);
                TempsGPU.Add(tempGPU);
                ScoresCPU.Add(scoreCPU);
                ScoresGPU.Add(scoreGPU);
                var lInt = new StressINT(loadCPU, LoadsCPU.Max(), tempCPU, TempsCPU.Max(), loadGPU, LoadsGPU.Max(), tempGPU, TempsGPU.Max(), scoreCPU, ScoresCPU.Max(), scoreGPU, ScoresGPU.Max());
                var l = new Stress($"{loadCPU}%", $"Макс: {LoadsCPU.Max()}%", $"{tempCPU}℃", $"Макс: {TempsCPU.Max()}℃", $"{loadGPU}%", $"Макс: {LoadsGPU.Max()}%", $"{tempGPU}℃", $"Макс: {TempsGPU.Max()}℃", $"{scoreCPU}", $"Макс: {ScoresCPU.Max()}", $"{scoreGPU}", $"Макс: {ScoresGPU.Max()}", lInt);
                worker.ReportProgress(1, l);
            }

        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data == "*")
                {
                    IsReadNow = true;
                }
                if (e.Data.StartsWith("Read:"))
                {
                    float temperatureDisk = 0.0f;
                    LibreHardwareMonitor.Hardware.Computer computer1 = new LibreHardwareMonitor.Hardware.Computer();
                    computer1.Open();
                    computer1.IsStorageEnabled = true;

                    foreach (var hardwareItem in computer1.Hardware)
                    {
                        if (hardwareItem.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.Storage)
                        {
                            hardwareItem.Update();

                            foreach (var sensor in hardwareItem.Sensors)
                            {
                                if (sensor.SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature)
                                {
                                    temperatureDisk = sensor.Value ?? 0;
                                    break;
                                }
                            }
                        }
                    }
                    computer1.Close();
                    TempsDisk.Add(Convert.ToInt32(temperatureDisk));

                    BeginInvoke((MethodInvoker)delegate { label19.Text = $"{Convert.ToInt32(temperatureDisk)}℃"; });
                    BeginInvoke((MethodInvoker)delegate { label18.Text = $"Макс: {TempsDisk.Max()}℃"; });

                    if (Convert.ToInt32(temperatureDisk) < 100)
                        BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar8.Value = Convert.ToInt32(temperatureDisk); });
                    else
                        BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar8.Value = 100; });

                    ScoresDisk.Add(Convert.ToInt32(e.Data.Split()[1]));
                    BeginInvoke((MethodInvoker)delegate { label16.Text = $"{e.Data.Split()[1]}"; });
                    BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar7.Value = Convert.ToInt32(Convert.ToInt32(e.Data.Split()[1]) / (5000 / 100)); });
                    BeginInvoke((MethodInvoker)delegate { label17.Text = $"Мін: {ScoresDisk.Min()}"; });

                }
                if (e.Data.StartsWith("Load:"))
                {
                    LoadsDisk.Add(Convert.ToInt32(e.Data.Split()[1]));
                    BeginInvoke((MethodInvoker)delegate { label20.Text = $"{e.Data.Split()[1]}%"; });
                    BeginInvoke((MethodInvoker)delegate { guna2CircleProgressBar9.Value = Convert.ToInt32(e.Data.Split()[1]); });
                    BeginInvoke((MethodInvoker)delegate { label21.Text = $"Макс: {LoadsDisk.Max()}%"; });

                }
            }
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var l = e.UserState as Stress;
            label12.Text = l.CPUload;
            label5.Text = l.CPUloadMax;
            label13.Text = l.CPUtemp;
            label6.Text = l.CPUtempMax;
            guna2CircleProgressBar5.Value = l.StressINT.CPUload;
            if (l.StressINT.CPUtemp <= 100)
                guna2CircleProgressBar3.Value = l.StressINT.CPUtemp;
            guna2CircleProgressBar4.Value = 100;
            if (l.StressINT.CPUScore <= 100)
                guna2CircleProgressBar4.Value = l.StressINT.CPUScore;


            label4.Text = l.GPUload;
            label7.Text = l.GPUloadMax;
            label9.Text = l.GPUtemp;
            label8.Text = l.GPUtempMax;
            guna2CircleProgressBar1.Value = l.StressINT.GPUload;
            if (l.StressINT.GPUtemp <= 100)
                guna2CircleProgressBar2.Value = l.StressINT.GPUtemp;
            guna2CircleProgressBar6.Value = 100;
            if (l.StressINT.GPUScore <= 100)
                guna2CircleProgressBar6.Value = l.StressINT.GPUScore;

            label10.Text = l.CPUScore;
            label11.Text = l.CPUScoreMin;

            label14.Text = l.GPUScore;
            label15.Text = l.GPUScoreMin;

            label1.Visible = true;
        }
        void _timer_Tick(object sender, EventArgs e)
        {
            var TimeSinceStartTime = DateTime.Now - _startTime;
            TimeSinceStartTime = new TimeSpan(TimeSinceStartTime.Hours, TimeSinceStartTime.Minutes, TimeSinceStartTime.Seconds);
            _currentElapsedTime = TimeSinceStartTime + _totalElapsedTime;
            if (label1.Text.Length >= 14)
            {
                label1.Text = label1.Text.Substring(0, 14) + $"{TimeSinceStartTime}";
            }
        }
        public StressForm(Color b, Guna.UI2.WinForms.Guna2Button b1, Guna.UI2.WinForms.Guna2Button b2, ToolStripMenuItem t, ToolStripMenuItem t1, string _CPUname, string _GPUname, List<string> _l1, char[] _a, char[] _B)
        {
            InitializeComponent();
            l1 = _l1;
            a = _a;
            B = _B;

            LoadsCPU = new List<int>();
            LoadsGPU = new List<int>();
            LoadsDisk = new List<int>();
            TempsCPU = new List<int>();
            TempsGPU = new List<int>();
            TempsDisk = new List<int>();
            ScoresCPU = new List<int>();
            ScoresGPU = new List<int>();
            ScoresDisk = new List<int>();

            worker = new BackgroundWorker();
            tool = t;
            tool1 = t1;
            label2.Text = _CPUname;
            label3.Text = _GPUname;
            label22.Text = $"Disk: {GetDiskName()}";
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(_timer_Tick);
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            button1_ = b1;
            button2_ = b2;

            if (b == Color.DimGray)
            {
                BackColor = Color.DimGray;
                ForeColor = Color.White;
                guna2GroupBox1.BorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox1.CustomBorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox1.FillColor = Color.FromArgb(56, 56, 56);
                guna2CircleProgressBar5.InnerColor = Color.FromArgb(56, 56, 56);
                guna2CircleProgressBar3.InnerColor = Color.FromArgb(56, 56, 56);
                guna2CircleProgressBar1.InnerColor = Color.FromArgb(56, 56, 56);
                guna2CircleProgressBar2.InnerColor = Color.FromArgb(56, 56, 56);
                guna2CircleProgressBar4.InnerColor = Color.FromArgb(56, 56, 56);
                guna2CircleProgressBar6.InnerColor = Color.FromArgb(56, 56, 56);

                label5.ForeColor = Color.White;
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label12.ForeColor = Color.White;
                label13.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label14.ForeColor = Color.White;
                label15.ForeColor = Color.White;
            }
            if (b == Color.White)
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
                guna2GroupBox1.BorderColor = SystemColors.Control;
                guna2GroupBox1.CustomBorderColor = SystemColors.Control;
                guna2GroupBox1.FillColor = SystemColors.Control;
                guna2CircleProgressBar5.InnerColor = SystemColors.Control;
                guna2CircleProgressBar3.InnerColor = SystemColors.Control;
                guna2CircleProgressBar1.InnerColor = SystemColors.Control;
                guna2CircleProgressBar2.InnerColor = SystemColors.Control;
                guna2CircleProgressBar4.InnerColor = SystemColors.Control;
                guna2CircleProgressBar6.InnerColor = SystemColors.Control;

                label5.ForeColor = Color.DimGray;
                label1.ForeColor = Color.DimGray;
                label2.ForeColor = Color.DimGray;
                label3.ForeColor = Color.DimGray;
                label12.ForeColor = Color.DimGray;
                label13.ForeColor = Color.DimGray;
                label6.ForeColor = Color.DimGray;
                label4.ForeColor = Color.DimGray;
                label7.ForeColor = Color.DimGray;
                label9.ForeColor = Color.DimGray;
                label8.ForeColor = Color.DimGray;
                label10.ForeColor = Color.DimGray;
                label11.ForeColor = Color.DimGray;
                label14.ForeColor = Color.DimGray;
                label15.ForeColor = Color.DimGray;
            }

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

            cl = new OpenCL.OpenCL
            {
                Accelerator = AcceleratorDevice.GPU
            };

            string kernel = "kernel void MatrixMulti(global int * dimension, global char * a, global char * b, global char * c){int id = get_global_id(0);for (int i = 0; i < dimension; i++){a[id]+=a[id];}}";
            size = 10;
            char[] c = new char[size * size];
            int[] dimensions = new int[3] { size, size, size };
            cl.SetKernel(kernel, "MatrixMulti");
            cl.SetParameter(dimensions, a, B, c);

            worker.RunWorkerAsync();

        }

        private string GetDiskName()
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

        private async void StressForm_Load(object sender, EventArgs e)
        {
            //cl = new OpenCL.OpenCL();
            //cl.Accelerator = AcceleratorDevice.GPU;

            //string kernel = "kernel void MatrixMulti(global int * dimension, global char * a, global char * b, global char * c){int id = get_global_id(0);for (int i = 0; i < dimension; i++){c[id] = a[id] * b[id];}}";

            //char[] c = new char[size * size];
            //int[] dimensions = new int[3] { size, size, size };
            //cl.SetKernel(kernel, "MatrixMulti");
            //cl.SetParameter(dimensions, a, B, c);


            _ = await RunStressTestAsync();
        }

        private async Task<int> RunStressTestAsync()
        {
            var taskCPU = WhileCPU();
            var taskGPU = WhileGPU();

            _ = await taskCPU;
            _ = await taskGPU;

            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stop = true;
            //Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void StressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            stop = true;
            guna2Button1.Enabled = false;
            DisplayExit();
            //Close();
        }

        private async void DisplayExit()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(10000);
                BeginInvoke((MethodInvoker)delegate { guna2GradientCircleButton1.Visible = true; });
            });
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click_2(object sender, EventArgs e)
        {

        }

        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            stop = true;
            guna2Button1.Enabled = false;
            Close();
        }

        private void guna2GradientCircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2GroupBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void guna2GroupBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void guna2GroupBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void guna2GroupBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void guna2GroupBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            mouseDown = false;

        }

        private void guna2GroupBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }
    }

    public class UpdateVisitor : OpenHardwareMonitor.Hardware.IVisitor
    {
        public void VisitComputer(OpenHardwareMonitor.Hardware.IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(OpenHardwareMonitor.Hardware.IHardware hardware)
        {
            hardware.Update();
            foreach (OpenHardwareMonitor.Hardware.IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(OpenHardwareMonitor.Hardware.ISensor sensor) { }

        public void VisitParameter(OpenHardwareMonitor.Hardware.IParameter parameter)
        {
        }
    }
    public class Temperature
    {
        public double CurrentValue { get; set; }
        public string InstanceName { get; set; }
        public static List<Temperature> Temperatures
        {
            get
            {
                List<Temperature> result = new List<Temperature>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                foreach (ManagementObject obj in searcher.Get())
                {
                    Double temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    temperature = (temperature - 2732) / 10.0;
                    result.Add(new Temperature { CurrentValue = temperature, InstanceName = obj["InstanceName"].ToString() });
                }
                return result;
            }
        }
    }

    [Serializable]
    public class Stress
    {
        public string CPUload { get; set; }
        public string CPUloadMax { get; set; }
        public string CPUtemp { get; set; }
        public string CPUtempMax { get; set; }

        public string GPUload { get; set; }
        public string GPUloadMax { get; set; }
        public string GPUtemp { get; set; }
        public string GPUtempMax { get; set; }


        public string CPUScore { get; set; }
        public string CPUScoreMin { get; set; }
        public string GPUScore { get; set; }
        public string GPUScoreMin { get; set; }

        public StressINT StressINT { get; set; }

        public Stress(string Cl, string ClM, string Ct, string CtM, string Gl, string GlM, string Gt, string GtM, string CS, string CSM, string GS, string GSM, StressINT SI)
        {
            CPUload = Cl;
            CPUloadMax = ClM;
            CPUtemp = Ct;
            CPUtempMax = CtM;

            GPUload = Gl;
            GPUloadMax = GlM;
            GPUtemp = Gt;
            GPUtempMax = GtM;

            CPUScore = CS;
            CPUScoreMin = CSM;
            GPUScore = GS;
            GPUScoreMin = GSM;

            StressINT = SI;
        }
    }

    [Serializable]
    public class StressINT
    {
        public int CPUload { get; set; }
        public int CPUloadMax { get; set; }
        public int CPUtemp { get; set; }
        public int CPUtempMax { get; set; }
        public int GPUload { get; set; }
        public int GPUloadMax { get; set; }
        public int GPUtemp { get; set; }
        public int GPUtempMax { get; set; }

        public int CPUScore { get; set; }
        public int CPUScoreMin { get; set; }
        public int GPUScore { get; set; }
        public int GPUScoreMin { get; set; }


        public StressINT(int Cl, int ClM, int Ct, int CtM, int Gl, int GlM, int Gt, int GtM, int CS, int CSM, int GS, int GSM)
        {
            CPUload = Cl;
            CPUloadMax = ClM;
            CPUtemp = Ct;
            CPUtempMax = CtM;

            GPUload = Gl;
            GPUloadMax = GlM;
            GPUtemp = Gt;
            GPUtempMax = GtM;

            CPUScore = CS;
            CPUScoreMin = CSM;
            GPUScore = GS;
            GPUScoreMin = GSM;
        }
    }
}