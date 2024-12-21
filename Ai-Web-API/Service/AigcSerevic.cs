using AutoMapper;
using EFCoreMigrations;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Dto.AiModel;
using Model.Other;

namespace Service;

public class AigcSerevic : IAigcSerevice
{
    private MyDbContext _context;
    private readonly IMapper _mapper;

    public AigcSerevic(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResult> GetModelService(GetModelReq req)
    {
        // TODO 查询实体类用automap映射到GetModelRes，返回给前端（req如果有传类型和模型名称，就过滤，类型精准过滤，模型是模糊查询）
        //需要分页，参考YoloService 的 getpkqTb
        var aiModelAsync = _context.AiModels.Where(p => p.IsDeleted == 0).ToListAsync();
        var yoloPkqResList = _mapper.Map<List<GetModelRes>>(aiModelAsync);

        return ResultHelper.Success("获取成功！", yoloPkqResList);
    }

    public Task<ApiResult> PutModelService(PutModelReq req)
    {
        // TODO 将传过来的文件Model，（判断是否存在，存在则抛错误（模型名称重复））保存到本地Mdoel文件夹下
        throw new NotImplementedException();
    }

    public Task<ApiResult> DelModelService(long id)
    {
        // TODO 根据id，软删除模型
        throw new NotImplementedException();
    }
}