using System.Text;
using Microsoft.Extensions.Options;
using Model;
using Model.Options;
using Model.UtilData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonUtil.AiGcUtil;

public class SparkRequestUtil
{
    private readonly AiGcOptions _aiGcOptions;

    public SparkRequestUtil(IOptionsMonitor<AiGcOptions> aiGcOptions)
    {
        _aiGcOptions = aiGcOptions.CurrentValue;
    }

    public async Task<string> RequestAsync(string q)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _aiGcOptions.token);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var requestData = new
            {
                model = "generalv3.5",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = q
                    }
                },
                stream = false
            };

            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_aiGcOptions.url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<SparkResponseData>(responseString);
                return responseData.Choices[0].Message.Content;
            }
            else
            {
                return "业务繁忙，请稍后再试！";
            }
        }
    }
}