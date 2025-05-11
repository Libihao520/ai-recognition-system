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
    private IAiRequestStrategy? _requestStrategy;
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
        var userId = _informationUtil.GetCurrentUserId();

        var messages = new List<message>();
        if (CacheManager.Exist(string.Format(RedisKey.UserActiveCode, userId)))
        {
            messages = CacheManager.Get<List<message>>(string.Format(RedisKey.UserActiveCode, userId));
        }

        messages.Add(new message() { content = q });
        CacheManager.Set(string.Format(RedisKey.UserActiveCode, userId), messages, TimeSpan.FromMinutes(30));

        var sparkRequestData = new SparkRequestData
        {
            messages = messages
        };
        _requestStrategy = _requestStrategyFactory.Create("Spark");
        return await _requestStrategy.RequestAsync(sparkRequestData, cancellationToken);
    }

    public IAsyncEnumerable<string> SparkProcessStreamAsync(string q, CancellationToken cancellationToken)
    {
        var userId = _informationUtil.GetCurrentUserId();

        var messages = new List<message>();
        if (CacheManager.Exist(string.Format(RedisKey.UserActiveCode, userId)))
        {
            messages = CacheManager.Get<List<message>>(string.Format(RedisKey.UserActiveCode, userId));
        }

        messages.Add(new message() { content = q });
        CacheManager.Set(string.Format(RedisKey.UserActiveCode, userId), messages, TimeSpan.FromMinutes(30));

        var sparkRequestData = new SparkRequestData
        {
            messages = messages,
            stream = true
        };
        _requestStrategy = _requestStrategyFactory.Create("Spark");
        return _requestStrategy.RequestStreamAsync<SparkRequestData>(sparkRequestData, cancellationToken);
    }
}