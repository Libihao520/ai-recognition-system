namespace CommonUtil.AiGcUtil;

/// <summary>
/// AI请求策略-上下文类
/// </summary>
public class AiRequestProcessor
{
    private IAiRequestStrategy? _requestStrategy;
    private readonly AiRequestStrategyFactory _requestStrategyFactory;

    public AiRequestProcessor(AiRequestStrategyFactory requestStrategyFactory)
    {
        _requestStrategyFactory = requestStrategyFactory;
    }

    public async Task<string> SparkProcess(string q, CancellationToken cancellationToken)
    {
        _requestStrategy = _requestStrategyFactory.Create("Spark");
        return await _requestStrategy.RequestAsync(q, cancellationToken);
    }

    public IAsyncEnumerable<string> SparkProcessStreamAsync(string q,CancellationToken cancellationToken)
    {
        _requestStrategy = _requestStrategyFactory.Create("Spark");
        return _requestStrategy.RequestStreamAsync(q,cancellationToken);
    }
}