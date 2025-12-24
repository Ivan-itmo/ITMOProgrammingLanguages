namespace PL_Lab5;

public interface IExternalDataService
{
    Task<string> GetUserDataAsync(int userId);
    Task<string> GetUserOrdersAsync(int userId);
    Task<string> GetAdsAsync();
}