namespace CommonUtil.AiGcUtil;

public interface IAiRequestStrategy
{
    public Task<string> RequestAsync(string q);
    public IAsyncEnumerable<string> RequestStreamAsync(string q);
}