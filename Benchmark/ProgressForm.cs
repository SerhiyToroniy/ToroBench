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
using OpenCL;


namespace Benchmark
{
    public partial class ProgressForm : Form
    {
        BackgroundWorker worker;
        long elapsedMs = -1;
        Label score = new Label();
        int cores = -1;
        Guna.UI2.WinForms.Guna2Button b1;
        Guna.UI2.WinForms.Guna2Button b2;
        bool stop = false;
        Color o = new Color();
        ToolStripMenuItem tool = new ToolStripMenuItem();
        //DateTime StartTime;
        ToolStripMenuItem tool1 = new ToolStripMenuItem();
        Process Proc = Process.GetCurrentProcess();
        int iterationGPU = 0;
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
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.Normal;
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
        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (stop)
                return;
            int N = 5000;
            int percent_mod = Convert.ToInt32(N / 100) * 2;
            var floatList = new List<float>();
            for (float i = 1; i <= N; i++)
            {
                if (stop)
                    return;
                floatList.Add(i);
            }
            var intList = new List<int>();
            for (int i = 1; i <= N; i++)
            {
                if (stop)
                    return;
                intList.Add(i);
            }
            List<string> stringList = new List<string>();
            for (int i = 0; i < N; i++)
            {
                if (stop)
                    return;
                stringList.Add(i.ToString());
            }
            var watch = new Stopwatch();
            int count = 0;
            int percent = 0;
            if (Text == "CPU")
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 1,
                };
                long AffinityMask = (long)Proc.ProcessorAffinity;
                AffinityMask &= 0x0001;
                Proc.ProcessorAffinity = (IntPtr)AffinityMask;
                watch.Start();
                Parallel.ForEach(floatList, options, i =>
                {
                    count += 1;
                    for (int j = 0; j < floatList.Count(); j++)
                    {
                        if (stop)
                            return;
                        floatList[j] /= 1;
                        floatList[j] *= 1;
                        floatList[j] += 1;
                        floatList[j] -= 1;
                        intList[j] /= 1;
                        intList[j] *= 1;
                        intList[j] += 1;
                        intList[j] -= 1;
                        IsPrime(intList[j]);
                    }
                    stringList.Sort();
                    if (count % percent_mod == 0)
                        percent++;
                    worker.ReportProgress(percent);
                });
                count = 0;
                options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount(),
                };
                Proc.ProcessorAffinity = (IntPtr)((1 << Environment.ProcessorCount) - 1);
                Parallel.ForEach(floatList, options, i =>
                {
                    count += 1;
                    for (int j = 0; j < floatList.Count(); j++)
                    {
                        if (stop)
                            return;
                        floatList[j] /= 1;
                        floatList[j] *= 1;
                        floatList[j] += 1;
                        floatList[j] -= 1;
                        intList[j] /= 1;
                        intList[j] *= 1;
                        intList[j] += 1;
                        intList[j] -= 1;
                        IsPrime(intList[j]);
                    }
                    stringList.Sort();
                    if (count % percent_mod == 0)
                        percent++;
                    worker.ReportProgress(percent);
                });
                watch.Stop();
            }
            if (Text == "GPU")
            {
                watch.Start();
                string kernel = @"__kernel void concat_kernel(__global float *D,__global int *I, const int Size)
                                    {
                                        int gid = get_global_id(0);

                                        int i;
                                        int w;
                                        for(i=gid; i< Size; i++){
                                            D[i] = D[i]/1;
                                            D[i] = D[i]*1;
                                            D[i] = D[i]+1;
                                            D[i] = D[i]-1;
                                            I[i] = I[i]/1;
                                            I[i] = I[i]*1;
                                            I[i] = I[i]+1;
                                            I[i] = I[i]-1;
                                        }
                                    }";
                MultiCL cl = new MultiCL();
                int size = 10000;
                int size_for = 2000;
                int size_for_mod = size_for / 100;
                percent = 0;
                int work_size = 1;
                double[] doubleList = new double[size];
                int[] iList = new int[size];
                for (int i = 0; i < size; i++)
                {
                    doubleList[i] = 1.1;
                    iList[i] = i;
                }
                cl.SetKernel(kernel, "concat_kernel");
                cl.SetParameter(doubleList, iList, size);

                var thread1 = new Thread(() =>
                {
                    for (int i = 0; i < size_for; i++)
                    {
                        if (stop)
                            return;
                        iterationGPU++;
                        cl.Invoke(0, size, work_size);
                        if (i % size_for_mod == 0)
                            percent++;
                            worker.ReportProgress(percent);
                    }
                });

                thread1.Start();
                thread1.Join();
                watch.Stop();
            }
            elapsedMs = watch.ElapsedMilliseconds;
            if (stop)
                return;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > guna2CircleProgressBar1.Value)
            {
                guna2CircleProgressBar1.Value = e.ProgressPercentage;
            }
        }
        public ProgressForm(int s, ToolStripMenuItem t1, ToolStripMenuItem t, string n, Label l, int c, Guna.UI2.WinForms.Guna2Button b1, Guna.UI2.WinForms.Guna2Button b2, Color outter)
        {
            InitializeComponent();
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
            this.b1 = b1;
            this.b2 = b2;
            if (outter == Color.DimGray)
            {
                BackColor = Color.DimGray;
                ForeColor = Color.White;
                guna2GroupBox1.BorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox1.CustomBorderColor = Color.FromArgb(56, 56, 56);
                guna2GroupBox1.FillColor = Color.FromArgb(56, 56, 56);
                guna2CircleProgressBar1.ForeColor = Color.White;
                guna2CircleProgressBar1.InnerColor = Color.FromArgb(56, 56, 56);
            }
            if (outter == Color.White)
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
                guna2GroupBox1.BorderColor = SystemColors.Control;
                guna2GroupBox1.CustomBorderColor = SystemColors.Control;
                guna2GroupBox1.FillColor = SystemColors.Control;
                guna2CircleProgressBar1.ForeColor = Color.DimGray;
                guna2CircleProgressBar1.InnerColor = SystemColors.Control;
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

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void ProgressForm_Shown(object sender, EventArgs e)
        {

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
        public int N = 100000000;
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
            if (type == "CPU")
            {
                for (int i = 0; i < 100; i++)
                {
                    if (i % 10 == 0)
                        percent = i;

                    integerList.Sort();
                    integerList.Reverse();
                    floatList.Sort();
                    floatList.Reverse();
                }
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = HardwareInfo.GetLogicalCoresCount()
                };
                Parallel.For(0, 2000, i =>
                {
                    if (stop)
                        return;

                    integerList.Sort();
                    integerList.Reverse();
                    floatList.Sort();
                    floatList.Reverse();
                });
            }
            if (type == "GPU")
            {

            }
        }
        public void float_calculation(string type, bool stop)
        {
            if (type == "CPU")
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

                }
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
            if (type == "GPU")
            {

            }

        }
        public void sorting_calculation(string type, bool stop)
        {
            if (type == "CPU")
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
            if (type == "GPU")
            {

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
            if (type == "CPU")
            {
                for (int i = 0; i < N / 100; i++)
                {
                    if (stop)
                        return;
                    var prime = isPrime(i);
                    //Thread.Sleep(1);
                }
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
            if (type == "GPU")
            {

            }
        }
    }
}
