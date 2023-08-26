using Model.Other;

namespace WebApi.Config;

public class ResultHelper
{
    public static ApiResult Success(object res)
    {
        return new ApiResult() { code = 1, data = res };
    }

    public static ApiResult Error(string message)
    {
        return new ApiResult() { code = 2, message = message };
    }
}