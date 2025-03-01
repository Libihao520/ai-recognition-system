using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.AiModel;
using Model.Other;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class AigcController : ControllerBase
{
    private readonly IAiGcService _aiGcService;

    public AigcController(IAiGcService aiGcService)
    {
        _aiGcService = aiGcService;
    }

    [HttpGet]
    public Task<ApiResult> GetModelService([FromQuery] GetModelReq req)
    {
        return _aiGcService.GetModelService(req);
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 262144000)] // 设置表单数据的最大长度为250MB
    [RequestSizeLimit(262144000)] // 设置整个HTTP请求的最大大小为250MB
    public async Task<ApiResult> PutModelService([FromForm] PutModelReq req)
    {
        return await _aiGcService.PutModelService(req);
    }

    [HttpDelete]
    public Task<ApiResult> DelModelService(long id)
    {
        return _aiGcService.DelModelService(id);
    }

    /// <summary>
    ///  在线问答
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<ApiResult> QuestionsAndAnswers([FromQuery] string q)
    {
        return _aiGcService.QuestionsAndAnswers(q);
    }

    /// <summary>
    /// 在线问答（流式传输）
    /// </summary>
    /// <param name="q"></param>
    [HttpGet]
    public async Task QuestionsAndAnswersStream([FromQuery] string q)
    {
        var response = Response;
        response.Headers.Add("Content-Type", "text/event-stream");

        // 这里的 _aiGcService.QuestionsAndAnswersSSE 是一个假设的方法，需要您根据实际需求实现
        await foreach (var message in _aiGcService.QuestionsAndAnswersStream(q))
        {
            // SSE 的消息格式是 "data: <message>\n\n"
            await response.WriteAsync($"data: {message}\n\n");
            await response.Body.FlushAsync(); // 确保消息被立即发送
        }
    }
}