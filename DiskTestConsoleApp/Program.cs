using System.Diagnostics;

int threadCount = 10;
const int blockSizeInKiB = 10 * 1024 * 1024; // blockSizeInKiB * queueDepth = file size
const int queueDepth = 100;
Random rand = new Random();
Task[] tasksWrite = new Task[threadCount];
Task[] tasksRead = new Task[threadCount];
string[] fileNames = new string[threadCount];

for (int i = 0; i < threadCount; i++)
{
    fileNames[i] = $"test{i}.bin";
    tasksWrite[i] = WriteAsync(fileNames[i]);
}
Stopwatch sw = Stopwatch.StartNew();
await Task.WhenAll(tasksWrite);
sw.Stop();
Console.WriteLine($"Write: {Convert.ToInt32(blockSizeInKiB * queueDepth / sw.Elapsed.TotalSeconds / 1000000 * 10)}");
Console.WriteLine("*");
for (int i = 0; i < threadCount; i++)
{
    fileNames[i] = $"test{i}.bin";
    tasksRead[i] = ReadAsync(fileNames[i]);
}
sw = Stopwatch.StartNew();
await Task.WhenAll(tasksRead);
sw.Stop();
Console.WriteLine($"Read: {Convert.ToInt32(blockSizeInKiB * queueDepth / sw.Elapsed.TotalSeconds / 1000000 * 10)}");

for (int i = 0; i < threadCount; i++)
{
    File.Delete(fileNames[i]);
}
async Task<int> WriteAsync(string filename)
{
    return await Task.Run(() =>
    {
        byte[] buffer = new byte[blockSizeInKiB];
        Random rand = new Random();
        lock (new object())
        {
            using (FileStream fs = File.Create(filename))
            {
                for (int i = 0; i < queueDepth; i++)
                {
                    rand.NextBytes(buffer);
                    fs.Write(buffer, 0, buffer.Length);
                    if (i % 10 == 0)
                        Console.WriteLine($"{100.0 * (i) / queueDepth}");
                }
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
        lock (new object())
        {
            using (FileStream fs = File.OpenRead(filename))
            {
                for (int i = 0; i < queueDepth; i++)
                {
                    fs.Read(buffer, 0, buffer.Length);
                    if (i % 10 == 0)
                        Console.WriteLine($"{100.0 * (i) / queueDepth}");
                }
            }
        }
        return 0;
    });
}
