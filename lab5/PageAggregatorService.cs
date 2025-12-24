namespace PL_Lab5;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class PageAggregatorService : IPageAggregator
{
    private readonly IExternalDataService _externalDataService;

    public PageAggregatorService(IExternalDataService externalDataService)
    {
        _externalDataService = externalDataService ?? throw new ArgumentNullException(nameof(externalDataService));
    }

    public async Task<PagePayload> LoadPageDataSequentialAsync(int userId)
    {
        Console.WriteLine("=== Последовательная загрузка ===");
        var payload = new PagePayload();

        try
        {
            payload.UserData = await _externalDataService.GetUserDataAsync(userId);
            Console.WriteLine("[Sequential] Задача UserData завершена");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Sequential] Задача GetUserDataAsync завершена с ошибкой: {ex.Message}");
        }

        try
        {
            payload.OrderData = await _externalDataService.GetUserOrdersAsync(userId);
            Console.WriteLine("[Sequential] Задача OrderData завершена");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Sequential] Задача GetUserOrdersAsync завершена с ошибкой: {ex.Message}");
        }

        try
        {
            payload.AdData = await _externalDataService.GetAdsAsync();
            Console.WriteLine("[Sequential] Задача AdData завершена");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Sequential] Задача GetAdsAsync завершена с ошибкой: {ex.Message}");
        }

        return payload;
    }

    public async Task<PagePayload> LoadPageDataParallelAsync(int userId)
    {
        Console.WriteLine("=== Параллельная загрузка ===");
        var ppl = new PagePayload();

        var userTask = _externalDataService.GetUserDataAsync(userId);
        var ordersTask = _externalDataService.GetUserOrdersAsync(userId);
        var adsTask = _externalDataService.GetAdsAsync();

        var tasks = new List<Task<string>> { userTask, ordersTask, adsTask };

        while (tasks.Any())
        {
            Task<string> finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);

            try
            {
                string result = await finishedTask;

                // Прямое присваивание вместо рефлексии
                if (finishedTask == userTask)
                    ppl.UserData = result;
                else if (finishedTask == ordersTask)
                    ppl.OrderData = result;
                else if (finishedTask == adsTask)
                    ppl.AdData = result;

                Console.WriteLine($"Завершилась задача");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Задача завершилась с ошибкой: {e.Message}");
            }
        }

        return ppl;
    }
}