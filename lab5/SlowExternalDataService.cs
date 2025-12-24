namespace PL_Lab5;

public class SlowExternalDataService : IExternalDataService
{
    public async Task<string> GetUserDataAsync(int userId)
    {
        Console.WriteLine($"[GetUserDataAsync] START (userId = {userId})");
        await Task.Delay(2000);
        Console.WriteLine($"[GetUserDataAsync] END (userId = {userId})");
        return $"User#{userId}";
    }

    public async Task<string> GetUserOrdersAsync(int userId)
    {
        Console.WriteLine($"[GetUserOrdersAsync] START (userId = {userId})");
        await Task.Delay(3000);
        Console.WriteLine($"[GetUserOrdersAsync] ERROR (userId = {userId})");
        throw new Exception("Orders API failed");
    }

    public async Task<string> GetAdsAsync()
    {
        Console.WriteLine("[GetAdsAsync] START");
        await Task.Delay(1000);
        Console.WriteLine("[GetAdsAsync] END");
        return "Some really cool ads";
    }
}