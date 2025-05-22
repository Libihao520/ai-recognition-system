using CommonUtil.RedisUtil;
using Microsoft.AspNetCore.Http;
using Model.Consts;
using Model.UtilData;

namespace CommonUtil.AiGcUtil;

/// <summary>
/// AI请求策略-上下文类
/// </summary>
public class AiRequestProcessor
{
    private readonly AiRequestStrategyFactory _requestStrategyFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserInformationUtil _informationUtil;

    public AiRequestProcessor(AiRequestStrategyFactory requestStrategyFactory, IHttpContextAccessor httpContextAccessor,
        UserInformationUtil informationUtil)
    {
        _requestStrategyFactory = requestStrategyFactory;
        _httpContextAccessor = httpContextAccessor;
        _informationUtil = informationUtil;
    }

    public async Task<string> SparkProcess(string q, CancellationToken cancellationToken)
    {
        var messages = GetAndUpdateMessages(q);
        var sparkRequestData = new SparkRequestData { messages = messages };

        var requestStrategy = _requestStrategyFactory.Create("Spark");
        return await requestStrategy.RequestAsync(sparkRequestData, cancellationToken);
    }

    public IAsyncEnumerable<string> SparkProcessStreamAsync(string q, CancellationToken cancellationToken)
    {
        var messages = GetAndUpdateMessages(q);
        var sparkRequestData = new SparkRequestData
        {
            messages = messages,
            stream = true
        };
        var requestStrategy = _requestStrategyFactory.Create("Spark");
        return requestStrategy.RequestStreamAsync<SparkRequestData>(sparkRequestData, cancellationToken);
    }

    private List<message> GetAndUpdateMessages(string newMessage)
    {
        var userId = _informationUtil.GetCurrentUserId();
        var cacheKey = string.Format(RedisKey.UserAiRecentDialogs, userId);

        var messages = CacheManager.Exist(cacheKey)
            ? CacheManager.Get<List<message>>(cacheKey)
            : new List<message>();

        messages.Add(new message() { content = newMessage });
        CacheManager.Set(cacheKey, messages, TimeSpan.FromMinutes(30));

        return messages;
    }
}