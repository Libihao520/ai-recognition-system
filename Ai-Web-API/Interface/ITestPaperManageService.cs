using Model.Dto.TestPaperManage;
using Model.Other;

namespace Interface;

public interface ITestPaperManageService
{
    Task<ApiResult> GetBankRole(TestPaperManageReq req);
}