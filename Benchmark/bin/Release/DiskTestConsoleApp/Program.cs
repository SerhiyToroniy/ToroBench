using System.Diagnostics;


//////////////////////////SS

int threadCount = 10;
const int blockSizeInKiB = 20 * 1024 * 1024; // blockSizeInKiB * queueDepth = files sizes overall
const int queueDepth = 100;
bool stopMonitoring = false;
Random rand = new();
Task[] tasksWrite = new Task[threadCount];
Task[] tasksRead = new Task[threadCount];
Task[] tasksMonitor = new Task[1];
string[] fileNames = new string[threadCount];
tasksMonitor[0] = GetDiskLoading();
for (int i = 0; i < threadCount; i++)
{
    fileNames[i] = $"test{i}.bin";
    tasksWrite[i] = WriteAsync(fileNames[i]);
}
Stopwatch sw = Stopwatch.StartNew();
await Task.WhenAll(tasksWrite);
sw.Stop();
Console.WriteLine("100");
Console.WriteLine($"Write: {blockSizeInKiB * queueDepth / sw.ElapsedMilliseconds * threadCount / 1000}");

Console.WriteLine("*");

for (int i = 0; i < threadCount; i++)
{
    fileNames[i] = $"test{i}.bin";
    tasksRead[i] = ReadAsync(fileNames[i]);
}
sw = Stopwatch.StartNew();
await Task.WhenAll(tasksRead);
sw.Stop();
Console.WriteLine($"Read: {blockSizeInKiB * queueDepth / sw.ElapsedMilliseconds * threadCount / 1000}");
stopMonitoring = true;
for (int i = 0; i < threadCount; i++)
{
    File.Delete(fileNames[i]);
}
stopMonitoring = true;
await Task.WhenAll(tasksMonitor);

async Task<int> GetDiskLoading()
{
    return await Task.Run(() =>
    {
        while (!stopMonitoring)
        {
            Thread.Sleep(1000);
            var counter = new PerformanceCounter("PhysicalDisk", "% Idle Time", "_Total");
            Console.WriteLine($"Load: {100 - counter.NextValue()}");
        }
        return 0;
    });
}

async Task<int> WriteAsync(string filename)
{
    return await Task.Run(() =>
    {
        byte[] buffer = new byte[blockSizeInKiB];
        Random rand = new();
        using (FileStream fs = File.Create(filename))
        {
            for (int i = 0; i < queueDepth; i++)
            {
                rand.NextBytes(buffer);
                fs.Write(buffer, 0, buffer.Length);
                if (i % 10 == 0)
                    Console.WriteLine($"{Math.Round(100.0 * (i) / queueDepth, 0)}");
            }
        }
        return 0;
    });
}

async Task<int> ReadAsync(string filename)
{
    return await Task.Run(() =>
    {
        byte[] buffer = new byte[blockSizeInKiB];

        using (FileStream fs = File.OpenRead(filename))
        {
            for (int i = 0; i < queueDepth; i++)
            {
                fs.Read(buffer, 0, buffer.Length);
                if (i % 10 == 0)
                    Console.WriteLine($"{Math.Round(100.0 * (i) / queueDepth, 0)}");
            }
        }
        return 0;
    });
}
