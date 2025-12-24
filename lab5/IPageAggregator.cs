namespace PL_Lab5;

public interface IPageAggregator
{
    Task<PagePayload> LoadPageDataSequentialAsync(int userId);
    Task<PagePayload> LoadPageDataParallelAsync(int userId);
}