namespace PL_Lab5;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        int userId = 42;

        IExternalDataService externalService = new SlowExternalDataService();
        IPageAggregator aggregator = new PageAggregatorService(externalService);

        var sw = new Stopwatch();

        sw.Start();
        var sequentialResult = await aggregator.LoadPageDataSequentialAsync(userId);
        sw.Stop();

        Console.WriteLine(sequentialResult);
        Console.WriteLine($"Время последовательной загрузки: {sw.ElapsedMilliseconds} мс\n");

        sw.Restart();
        var parallelResult = await aggregator.LoadPageDataParallelAsync(userId);
        sw.Stop();

        Console.WriteLine(parallelResult);
        Console.WriteLine($"Время параллельной загрузки: {sw.ElapsedMilliseconds} мс");
    }
}