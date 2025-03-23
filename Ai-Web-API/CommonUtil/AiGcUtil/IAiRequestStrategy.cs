namespace CommonUtil.AiGcUtil;

public interface IAiRequestStrategy
{
    public Task<string> RequestAsync(string q,CancellationToken cancellationToken);
    public IAsyncEnumerable<string> RequestStreamAsync(string q,CancellationToken cancellationToken);
}