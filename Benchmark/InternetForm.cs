using Benchmark;
using Guna.UI2.WinForms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToroBench
{
    public partial class InternetForm : Form
    {
        public Guna2Button button1 { get; set; }
        public Guna2Button button2 { get; set; }
        public ToolStripMenuItem toolstrip1 { get; set; }
        public ToolStripMenuItem toolstrip2 { get; set; }
        public bool stop { get; set; }
        public Color color { get; set; }

        public InternetForm(Guna2Button b1, Guna2Button b2, ToolStripMenuItem t1, ToolStripMenuItem t2, Color c)
        {
            InitializeComponent();
            button1 = b1;
            button2 = b2;
            toolstrip1 = t1;
            toolstrip2 = t2;
            Load += Form1_Load;
            stop = false;
            color = c;
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
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Call GetNetworkStatistics when the form is loaded
            _ = await GetNetworkStatistics();
        }

        private void InternetTest_Load(object sender, EventArgs e)
        {
            guna2HtmlLabel1.Text = GetDeviceName();
        }

        public string GetDeviceName()
        {
            string deviceName = "";

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in interfaces)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up && adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    if (properties.GatewayAddresses.Count > 0)
                    {
                        foreach (GatewayIPAddressInformation gateway in properties.GatewayAddresses)
                        {
                            if (gateway.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                deviceName = adapter.Description;
                                break;
                            }
                        }
                    }
                }
            }

            return deviceName;
        }

        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            stop = true;
            Close();
        }

        private void InternetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            toolstrip1.Enabled = true;
            toolstrip2.Enabled = true;
        }

        private void guna2CircleProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            stop = true;
            Close();
        }
        public async Task<NetworkStatistics> GetNetworkStatistics()
        {
            NetworkStatistics stats = new NetworkStatistics();

            // Measure ping time and jitter
            try
            {
                _ = new Ping();
                if (!stop)
                    label3.Text = $"{await GetPingTime()} ms";
                if (!stop)
                    label4.Text = $"{await GetPingJitter()} ms";
                if (!stop)
                    // Measure download speed
                    stats.DownloadSpeed = await GetDownloadSpeed();
                if (!stop)
                    // Measure upload speed
                    stats.UploadSpeed = await GetUploadSpeed();
            }
            catch
            {
                var q = new ErrorForm(this, button1, button2, toolstrip1, toolstrip2, color, "You aren't connected to the internet!", "Error", "NoInternet.png");
                q.Show();
            }
            guna2Button1.Enabled = false;
            return stats;
        }

        public async Task<double> GetDownloadSpeed()
        {
            double downloadSpeed = -1;
            const int timeMs = 10000;
            float minAnimationSpeedTime = 0.1f;
            float maxAnimationSpeedTime = 10f;
            guna2CircleProgressBar4.AnimationSpeed = minAnimationSpeedTime; // 0.1 min -> 10 max
            WebClient client = new WebClient();
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch progressStopwatch = new Stopwatch();
            System.Timers.Timer timer = new System.Timers.Timer(timeMs);

            timer.Elapsed += (sender, e) =>
            {
                client.CancelAsync();
                timer.Stop();
                guna2CircleProgressBar4.Value = 100;
            };
            progressStopwatch.Start();
            client.DownloadProgressChanged += (sender, e) =>
            {
                if (stop)
                {
                    client.CancelAsync();
                    timer.Stop();
                    guna2CircleProgressBar4.Value = 100;
                }
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                double speed = bytesIn / stopwatch.Elapsed.TotalSeconds;
                downloadSpeed = speed / 1024 / 1024 * 10; // convert to Mbps
                if (downloadSpeed < 1)
                {
                    label10.Text = $"{Math.Round(downloadSpeed * 1024, 0)} Kbps";
                    guna2CircleProgressBar4.AnimationSpeed = minAnimationSpeedTime;
                }
                if (downloadSpeed > 1 && downloadSpeed < 1000)
                {
                    label10.Text = $"{Math.Round(downloadSpeed, 0)} Mbps";
                    guna2CircleProgressBar4.AnimationSpeed = (float)(maxAnimationSpeedTime * downloadSpeed / 100);
                }
                if (downloadSpeed >= 1000 && downloadSpeed < 1000000)
                {
                    label10.Text = $"{Math.Round(downloadSpeed / 1024, 0)} Gbps";
                    guna2CircleProgressBar4.AnimationSpeed = maxAnimationSpeedTime;
                }
                guna2CircleProgressBar4.Value = Convert.ToInt32(progressStopwatch.ElapsedMilliseconds / (timeMs / 100));
            };

            try
            {
                stopwatch.Start();
                timer.Start();
                _ = await client.DownloadDataTaskAsync("https://speed.hetzner.de/100MB.bin");
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.RequestCanceled)
                {
                    // handle other exceptions
                }
            }
            finally
            {
                stopwatch.Stop();
                progressStopwatch.Stop();
                timer.Stop();
            }

            return downloadSpeed;
        }

        public async Task<double> GetUploadSpeed()
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/octet-stream");

            // Create a random 10 MB buffer
            const int timeMs = 10000;
            byte[] buffer = new byte[1024 * 1024 * 10];
            new Random().NextBytes(buffer);
            System.Timers.Timer timer = new System.Timers.Timer(timeMs);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeMs);

            float minAnimationSpeedTime = 0.1f;
            float maxAnimationSpeedTime = 10f;
            guna2CircleProgressBar4.AnimationSpeed = minAnimationSpeedTime; // 0.1 min -> 10 max

            // Start the upload test and track progress
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var progress = new Progress<double>(percent =>
            {
                // Update progress UI
                if (percent > 0)
                {
                    //guna2CircleProgressBar1.Value = (int)percent;
                }
                else
                {
                    percent *= -1;
                    if (percent < 1)
                    {
                        label7.Text = $"{Math.Round(percent * 1024, 0)} Kbps";
                        guna2CircleProgressBar1.AnimationSpeed = minAnimationSpeedTime;
                    }
                    if (percent > 1 && percent < 1000)
                    {
                        label7.Text = $"{Math.Round(percent, 0)} Mbps";
                        guna2CircleProgressBar1.AnimationSpeed = (float)(maxAnimationSpeedTime * percent / 100);
                    }
                    if (percent >= 1000 && percent < 1000000)
                    {
                        label7.Text = $"{Math.Round(percent / 1024, 0)} Gbps";
                        guna2CircleProgressBar1.AnimationSpeed = maxAnimationSpeedTime;
                    }
                }
            });
            try
            {
                await UploadDataAsync(client, "http://www.example.com/upload", buffer, progress, cts.Token, timeMs);
            }
            catch (OperationCanceledException)
            {
                // Upload was canceled due to timer expiration
            }
            sw.Stop();



            // Compute upload speed
            double speed = buffer.Length / sw.Elapsed.TotalSeconds;

            return speed;
        }

        private async Task UploadDataAsync(WebClient client, string url, byte[] data, IProgress<double> progress, CancellationToken cts, int timeMs)
        {
            try
            {
                // Set up the progress tracking
                long uploadedBytes = 0;
                int chunkSize = 4096;
                int totalChunks = (int)Math.Ceiling((double)data.Length / chunkSize);

                // Upload data in chunks and track progress
                Stopwatch sw = new Stopwatch();
                Stopwatch progressStopwatch = new Stopwatch();
                sw.Start();
                for (int i = 0; i < totalChunks; i++)
                {
                    int offset = i * chunkSize;
                    int size = Math.Min(chunkSize, data.Length - offset);

                    using (MemoryStream ms = new MemoryStream(data, offset, size))
                    {
                        progressStopwatch.Start();

                        client.UploadProgressChanged += (s, e) =>
                        {
                            if (stop || cts.IsCancellationRequested)
                            {
                                client.CancelAsync();
                                guna2CircleProgressBar1.Value = 100;
                                return;
                            }
                            // Compute progress percentage and report it to the progress object
                            double percent = (double)(uploadedBytes + e.BytesSent) / data.Length * 100;
                            progress.Report(percent);

                            // Compute upload speed and report it to the progress object
                            double speed = ((uploadedBytes + e.BytesSent) / sw.Elapsed.TotalSeconds) * 8 / 1000000;
                            progress.Report(-speed); // Use a negative value to distinguish upload speed updates from progress updates

                            guna2CircleProgressBar1.Value = Convert.ToInt32(progressStopwatch.ElapsedMilliseconds / (timeMs / 100));

                        };
                        _ = await client.UploadDataTaskAsync(url, data);
                        uploadedBytes += size;
                    }
                }
                sw.Stop();
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch { }
        }

        public async Task<long> GetPingTime()
        {
            long pingTime = -1;

            using (Ping ping = new Ping())
            {
                PingOptions options = new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 64
                };

                byte[] buffer = new byte[32];
                PingReply reply = await ping.SendPingAsync("www.google.com", 5000, buffer, options);

                if (reply.Status == IPStatus.Success)
                {
                    pingTime = reply.RoundtripTime;
                }
            }

            return pingTime;
        }

        public async Task<long> GetPingJitter()
        {
            using (Ping ping = new Ping())
            {
                PingOptions options = new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 64
                };

                byte[] buffer = new byte[32];
                int pingCount = 5;
                long[] pingTimes = new long[pingCount];

                for (int i = 0; i < pingCount; i++)
                {
                    PingReply reply = await ping.SendPingAsync("www.google.com", 5000, buffer, options);

                    if (reply.Status == IPStatus.Success)
                    {
                        pingTimes[i] = reply.RoundtripTime;
                    }
                    else
                    {
                        pingTimes[i] = -1;
                    }

                    // Wait a short time before sending the next ping
                    await Task.Delay(100);
                }

                // Calculate the average round-trip time
                long averagePingTime = (long)pingTimes.Where(t => t >= 0).Average();

                // Calculate the sum of the differences between each ping time and the average
                double jitter = pingTimes.Where(t => t >= 0).Sum(t => Math.Abs(t - averagePingTime)) / (pingCount - 1);

                // Return the jitter in milliseconds
                return (long)jitter;
            }
        }



        public class NetworkStatistics
        {
            public long PingTime { get; set; } // in milliseconds
            public long Jitter { get; set; } // in milliseconds
            public double DownloadSpeed { get; set; } // in Mbps
            public double UploadSpeed { get; set; } // in Mbps
        }
    }
}
