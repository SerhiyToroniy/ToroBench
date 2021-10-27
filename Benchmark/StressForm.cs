using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using OpenHardwareMonitor.Hardware;
using ComponentFactory.Krypton.Toolkit;


namespace Benchmark
{
    public partial class StressForm : Form
    {
        public System.Windows.Forms.Timer _timer;
        public DateTime _startTime = DateTime.MinValue;
        public TimeSpan _currentElapsedTime = TimeSpan.Zero;
        public TimeSpan _totalElapsedTime = TimeSpan.Zero;
        public bool _timerRunnig = false;

        BackgroundWorker worker;
        public List<int> Scores { get; set; }
        public List<int> Loads { get; set; }
        public List<float> floatList { get; set; }
        public List<int> Temps { get; set; }
        public float testfloat { get; set; }
        bool stop = false;
        int N = 10000000;
        PerformanceCounter cpuCounter;
        bool clear = false;
        KryptonButton button1_;
        KryptonButton button2_;
        ToolStripMenuItem tool = new ToolStripMenuItem();
        ToolStripMenuItem tool1 = new ToolStripMenuItem();

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clear = false;
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
            Dispose();
        }

        public int getCurrentCpuUsage()
        {
            return Convert.ToInt32(cpuCounter.NextValue());
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Stress> l = new List<Stress>();
            var cpu_usage = new Stress("CPU usage", $"", $"", $"");
            var realtime_score = new Stress("Real-time score", $"", $"", $"");
            var temperature = new Stress("Temperature", $"", $"", $"");
            int N = 10000000;

            //int testint = 0;
            //float testfloat = 0;
            //var integerList = Enumerable.Range(1, N).ToList();
            //integerList.Sort();
            //integerList.Reverse();
            l.Add(cpu_usage);
            l.Add(realtime_score);
            l.Add(temperature);
            worker.ReportProgress(0, l);
            List<string> l1 = new List<string>();
            for (int i = 0; i < N; i++)
            {
                if (stop)
                    return;
                l1.Add(i.ToString());
            }
            List<List<string>> L = new List<List<string>>();
            for (int i = 0; i < 256; i++)
            {
                if (stop)
                    return;
                L.Add(l1);
            }
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount(),
            };
            long elapsedMs = -1;
            while (!stop)
            {
                var watch = new Stopwatch();
                int load = getCurrentCpuUsage();
                int temp = -1;
                watch.Start();
                Parallel.ForEach(L, options, i =>
                {
                    foreach (var item2 in i)
                    {
                        if (item2 == "-1")
                            break;
                        if (stop)
                            return;
                    }
                });
                watch.Stop();
                temp = HardwareInfo.GetCurrentCPUTemperature();
                load = getCurrentCpuUsage();
                elapsedMs = watch.ElapsedMilliseconds;
                int score = Convert.ToInt32((1000000 / elapsedMs));
                Loads.Add(load);
                Temps.Add(temp);
                Scores.Add(score);
                Loads.Distinct();
                Temps.Distinct();
                Scores.Distinct();
                if (clear)
                {
                    Loads.Clear();
                    Scores.Clear();
                    Temps.Clear();
                }
                l.Clear();
                cpu_usage = new Stress("CPU usage", $"{Loads.Min()}%", $"{Loads.Max()}%", $"{load}%");
                realtime_score = new Stress("Real-time score", $"{Scores.Min()}", $"{Scores.Max()}", $"{score}");
                temperature = new Stress("Temperature", $"{Temps.Min()}°С", $"{Temps.Max()}°С", $"{temp}°С");
                l.Add(cpu_usage);
                l.Add(realtime_score);
                l.Add(temperature);
                worker.ReportProgress(1, l);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dataGridView1.Update();
            dataGridView1.Refresh();
            dataGridView1.DataSource = e.UserState;
            if (e.ProgressPercentage == 0)
            {
                dataGridView1.Rows[0].HeaderCell.Value = "CPU usage";
                dataGridView1.Rows[1].HeaderCell.Value = "Real-time score";
                dataGridView1.Rows[2].HeaderCell.Value = "Temperature";
            }
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
        public StressForm(Color b, KryptonButton b1, KryptonButton b2, ToolStripMenuItem t, ToolStripMenuItem t1)
        {
            InitializeComponent();
            floatList = new List<float>();
            for (float i = 1; i <= N; i++)
            {
                floatList.Add(i);
            }
            Scores = new List<int>();
            Loads = new List<int>();
            Temps = new List<int>();
            worker = new BackgroundWorker();
            tool = t;
            tool1 = t1;
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 10;
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
                button2.BackColor = Color.Black;
                button2.ForeColor = Color.White;
            }
            if (b == Color.White)
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
                button2.BackColor = Color.Gainsboro;
                button2.ForeColor = Color.Black;
            }
            worker.RunWorkerAsync();
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

        private void StressForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            stop = true;
            Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear = true;
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
            dataGridView1.ClearSelection();
        }
    }

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
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
        public string Sensor { get; set; }
        public string Current { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }

        public Stress(string s, string mi, string ma, string c)
        {
            Sensor = s;
            Min = mi;
            Max = ma;
            Current = c;
        }
    }
}
