using AutoMapper;
using EFCoreMigrations;
using Interface;
using Model;
using Model.Dto.photo;
using Model.Dto.TestPaperManage;
using Model.Other;

namespace Service;

public class TestPaperManageServic : ITestPaperManageService
{
    private MyDbContext _context;
    private IMapper _mapper;
    
    public TestPaperManageServic(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ApiResult> GetBankRole(TestPaperManageReq req)
    {
    
    
        return ResultHelper.Error("失败");
    }

}