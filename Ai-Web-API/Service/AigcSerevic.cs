using AutoMapper;
using EFCoreMigrations;
using Interface;
using Microsoft.CodeAnalysis.FlowAnalysis;
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
        if (req == null)
        {
            return ResultHelper.Error("不能为空");
        }

        try
        {
            var query = _context.AiModels.Where(q => q.IsDeleted == 0).AsQueryable();

            if (!string.IsNullOrEmpty(req.ModelName))
            {
                query = query.Where(m => m.ModelName.Contains(req.ModelName));
            }
            if (!string.IsNullOrEmpty(req.ModleCls) && req.ModleCls.ToUpper()!="全部")
            {
                query = query.Where(m => m.ModleCls == req.ModleCls);
            }
            

            var totalCount = await query.CountAsync();

            var paginatedResult = await query
                .Skip((req.pagenum - 1) * req.pagesize)
                .Take(req.pagesize)
                .ToListAsync();

            var resultList = _mapper.Map<List<GetModelRes>>(paginatedResult);

            return ResultHelper.Success("查询成功", resultList, totalCount);
        }
        catch (Exception e)
        {
            Console.WriteLine("$发生异常");
            return ResultHelper.Error("查询异常，请稍后重试");
        }
    }

    public Task<ApiResult> PutModelService(PutModelReq req)
    {
        // TODO 将传过来的文件Model，（判断是否存在，存在则抛错误（模型名称重复））保存到本地Mdoel文件夹下
        throw new NotImplementedException();
    }

    public async Task<ApiResult> DelModelService(long id)
    {
        // TODO 根据id，软删除模型
        if (id==null)
        {
            return ResultHelper.Error("不能为空");
        }
        var asQueryable = _context.AiModels
            .Where(q => q.IsDeleted == 0 && q.Id==id)
            .AsQueryable();
        try
        {
            if (asQueryable!=null)
            {
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        throw new NotImplementedException();
    }
}