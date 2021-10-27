using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using ComponentFactory.Krypton.Toolkit;


namespace Benchmark
{
    public partial class ProgressForm : Form
    {
        BackgroundWorker worker;
        long elapsedMs = -1;
        Label score = new Label();
        int cores = -1;
        KryptonButton b1;
        KryptonButton b2;
        bool stop = false;
        Color o = new Color();
        ToolStripMenuItem tool = new ToolStripMenuItem();
        DateTime StartTime;
        ToolStripMenuItem tool1 = new ToolStripMenuItem();
        Process Proc = Process.GetCurrentProcess();
        public int Score { get; set; }

        //bool benching;
        //BackgroundWorker worker2;
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!stop)
            {
                score.Text = $"Your score is {100000000 / elapsedMs}";
                score.Font = new Font("Segoe UI", 20);
                Score = 100000000 / (int)elapsedMs;
            }
            b1.Enabled = true;
            b2.Enabled = true;
            if (BackColor == Color.DimGray)
            {
                b1.BackColor = Color.Black;
                b2.BackColor = Color.Black;
            }
            tool.Enabled = true;
            tool1.Enabled = true;
            stop = false;
            //benching = false;
            //worker2.RunWorkerAsync();
            Proc.ProcessorAffinity = (IntPtr)((1 << Environment.ProcessorCount) - 1);
            Dispose();
            Close();
        }
        public static bool isPrime(int number)
        {
            int counter = 0;
            for (int j = 2; j < number; j++)
            {
                if (number % j == 0)
                {
                    counter = 1;
                    break;
                }
            }
            if (counter == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (stop)
                return;
            Benchmark b = new Benchmark(cores);
            int N = 10000000;
            //int testint = 0;
            //float testfloat = 0;
            var integerList = Enumerable.Range(1, N).ToList();
            integerList.Sort();
            integerList.Reverse();
            var floatList = new List<float>();
            for (float i = 1; i <= N; i++)
            {
                if (stop)
                    return;
                floatList.Add(i);
            }
            List<string> l1 = new List<string>();
            for (int i = 0; i < N; i++)
            {
                if (stop)
                    return;
                l1.Add(i.ToString());
            }
            List<List<string>> L = new List<List<string>>();
            for (int i = 0; i < 100; i++)
            {
                if (stop)
                    return;
                L.Add(l1);
            }
            var watch = new Stopwatch();
            int count = 0;
            if (Text == "Single-Core")
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 1,
                };
                long AffinityMask = (long)Proc.ProcessorAffinity;
                AffinityMask &= 0x0001;
                Proc.ProcessorAffinity = (IntPtr)AffinityMask;
                watch.Start();
                Parallel.ForEach(L, options, i =>
                 {
                     count += 1;
                     foreach (var item2 in i)
                     {
                         if (item2 == "-1")
                             break;
                         if (stop)
                             return;
                     }
                     integerList.Sort();
                     integerList.Reverse();
                     if (count % 1 == 0 && count < 100)
                         worker.ReportProgress((int)count / 1, "Doing calculations..");
                 });
                watch.Stop();
            }
            if (Text == "Multi-Core")
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount(),
                };
                Proc.ProcessorAffinity = (IntPtr)((1 << Environment.ProcessorCount) - 1);
                watch.Start();
                Parallel.ForEach(L, options, i =>
                 {
                     count += 1;
                     foreach (var item2 in i)
                     {
                         if (item2 == "-1")
                             break;
                         if (stop)
                             return;
                     }
                     integerList.Sort();
                     integerList.Reverse();
                     if (count % 1 == 0 && count < 100)
                         worker.ReportProgress((int)count / 1, "Doing calculations..");
                 });
                watch.Stop();
            }
            elapsedMs = watch.ElapsedMilliseconds;
            if (stop)
                return;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > progressBar1.Value)
            {
                progressBar1.Value = e.ProgressPercentage;
            }
            label1.Text = e.UserState.ToString();
        }
        public ProgressForm(int s, ToolStripMenuItem t1, ToolStripMenuItem t, string n, Label l, int c, KryptonButton b1, KryptonButton b2, Color outter)
        {
            InitializeComponent();
            //benching = b;
            Score = s;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            Text = n;
            score = l;
            cores = c;
            tool = t;
            tool1 = t1;
            o = outter;
            //worker2 = w;
            this.b1 = b1;
            this.b2 = b2;
            if (outter == Color.DimGray)
            {
                BackColor = Color.DimGray;
                ForeColor = Color.White;
                button2.BackColor = Color.Black;
                button2.ForeColor = Color.White;
            }
            if (outter == Color.White)
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
                button2.BackColor = Color.Gainsboro;
                button2.ForeColor = Color.Black;
            }
            worker.RunWorkerAsync();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            stop = true;
        }

        private void ProgressForm_Load_1(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - StartTime;
            string text = "";
            if (elapsed.Days > 0)
                text += elapsed.Days.ToString() + ".";

            // Преобразование миллисекунд в десятые доли секунды.
            int tenths = elapsed.Milliseconds / 100;

            // Запишите оставшееся время.
            text +=
                elapsed.Minutes.ToString("00") + ":" +
                elapsed.Seconds.ToString("00");

            label1.Text = text;
        }
    }
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
    public class Benchmark
    {
        public int N = 10000000;
        public int testint { get; set; }
        public float testfloat { get; set; }
        public List<int> integerList { get; set; }
        public List<float> floatList { get; set; }
        public int cores { get; set; }
        public Benchmark(int c)
        {
            testint = 0;
            testfloat = 0;
            integerList = Enumerable.Range(1, N).ToList();
            floatList = new List<float>();
            for (float i = 1; i <= N; i++)
            {
                floatList.Add(i);
            }
            cores = c;
        }
        public void int_calculation(string type, bool stop, int percent)
        {
            if (type == "Single-Core")
            {
                for (int i = 0; i < 100; i++)
                {
                    if (i % 10 == 0)
                        percent = i;
                    //testint = 0;
                    //testint += 1;
                    //testint -= 1;
                    //testint *= 1;
                    //testint /= 1;
                    //testint %= 1;
                    integerList.Sort();
                    integerList.Reverse();
                    floatList.Sort();
                    floatList.Reverse();
                }
            }
            if (type == "Multi-Core")
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount()
                };
                Parallel.For(0, 2000, i =>
                {
                    if (stop)
                        return;
                    //testint = 0;
                    //testint = 0;
                    //testint += 1;
                    //testint -= 1;
                    //testint *= 1;
                    //testint /= 1;
                    //testint %= 1;
                    integerList.Sort();
                    integerList.Reverse();
                    floatList.Sort();
                    floatList.Reverse();
                });
            }
        }
        public void float_calculation(string type, bool stop)
        {
            if (type == "Single-Core")
            {
                foreach (var i in floatList)
                {
                    if (stop)
                        return;
                    testfloat = 0;
                    testfloat += i;
                    testfloat -= i;
                    testfloat *= i;
                    testfloat /= i;
                    testfloat %= i;
                    //Thread.Sleep(1);

                }
            }
            if (type == "Multi-Core")
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount()
                };
                Parallel.ForEach(floatList, options, i =>
                {
                    if (stop)
                        return;
                    testfloat = 0;
                    testfloat += i;
                    testfloat -= i;
                    testfloat *= i;
                    testfloat /= i;
                    testfloat %= i;
                    Thread.Sleep(1);
                });
            }

        }
        public void sorting_calculation(string type, bool stop)
        {
            if (type == "Single-Core")
            {
                for (int i = 0; i < N / 1000000; i++)
                {
                    if (stop)
                        return;
                    integerList.Sort();
                    integerList.Reverse();
                    floatList.Sort();
                    floatList.Reverse();
                    //Thread.Sleep(1);
                }
            }
            if (type == "Multi-Core")
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount()
                };
                Parallel.For(0, N, options, i =>
                 {
                     if (stop)
                         return;
                     integerList.Sort();
                     integerList.Reverse();
                     floatList.Sort();
                     floatList.Reverse();
                     Thread.Sleep(1);
                 });
            }
        }
        public static bool isPrime(int number)
        {
            int counter = 0;
            for (int j = 2; j < number; j++)
            {
                if (number % j == 0)
                {
                    counter = 1;
                    break;
                }
            }
            if (counter == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void findingprime_calculation(string type, bool stop)
        {
            if (type == "Single-Core")
            {
                for (int i = 0; i < N / 100; i++)
                {
                    if (stop)
                        return;
                    var prime = isPrime(i);
                    //Thread.Sleep(1);
                }
            }
            if (type == "Multi-Core")
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount()
                };
                Parallel.For(0, N, options, i =>
                 {
                     if (stop)
                         return;
                     var prime = isPrime(i);
                     Thread.Sleep(1);
                 });
            }
        }
    }
}
