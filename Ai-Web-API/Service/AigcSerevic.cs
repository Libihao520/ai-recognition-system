using AutoMapper;
using Azure.Core;
using CommonUtil.RequesUtil;
using EFCoreMigrations;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;
using Model;
using Model.Dto.AiModel;
using Model.Entitys;
using Model.Other;
using MySqlConnector;
using Service.Common;

namespace Service;

public class AigcSerevic : IAigcSerevice
{
    private MyDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AigcSerevic(MyDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
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

            if (!string.IsNullOrEmpty(req.ModleCls) && req.ModleCls.ToUpper() != "全部")
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

    public async Task<ApiResult> PutModelService(PutModelReq req)
    {
        //数据校验
        if (string.IsNullOrEmpty(req.ModelName))
        {
            return ResultHelper.Error("模型名称不能为空");
        }

        if (string.IsNullOrEmpty(req.ModleCls))
        {
            return ResultHelper.Error("模型类型不能为空");
        }

        var models = _context.AiModels.FirstOrDefault(a => a.ModelName == req.ModelName && a.IsDeleted == 0);
        if (models != null)
        {
            return ResultHelper.Error("模型已存在，请更换名称后重新添加");
        }

        // 流式上传
        if (!MultipartRequestHelper.HasFormFileContentDisposition(_httpContextAccessor.HttpContext.Request.ContentType))
        {
            return ResultHelper.Error("没有文件上传");
        }

        var formFile = _httpContextAccessor.HttpContext.Request.Form.Files[0];

        //缓冲方式
        // var formFile = req.Model;

        if (formFile.Length == 0)
        {
            return ResultHelper.Error("没有文件上传");
        }

        //判断后缀是否符合
        var allowExtensions = new[] { ".pt", ".onnx" };
        var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
        if (!allowExtensions.Contains(fileExtension))
        {
            return ResultHelper.Error($"不支持文件格式，仅支持{String.Join(",", allowExtensions)}格式的模型文件");
        }

        //相对路径
        var relativeFilePath = Path.Combine("Model",
            req.ModelName + "_" + TimeBasedIdGeneratorUtil.GenerateId() + "_" + formFile.FileName);
        // 绝对路径
        var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath);

        var user = _httpContextAccessor?.HttpContext?.User;
        var createUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        var fileSizeInMb = (int)Math.Round(formFile.Length / (1024.0 * 1024.0));
        var aiModels = new AiModels()
        {
            ModelName = req.ModelName,
            ModleCls = req.ModleCls,
            Id = TimeBasedIdGeneratorUtil.GenerateId(),
            Path = relativeFilePath, // 设置文件路径到数据库记录中
            ModelSizee = fileSizeInMb,
            CreateDate = DateTime.Now,
            IsDeleted = 0,
            CreateUserId = createUserId,
        };

        // 开启数据库事务
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            //创建文件流，创建模式打开目标文件，不存在则创建，存在则覆盖
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            var entityEntry = _context.AiModels.Add(aiModels);
            await _context.SaveChangesAsync();
            // 提交数据库事务
            await transaction.CommitAsync();
            return ResultHelper.Success("请求成功!", "模型添加成功");
        }
        catch (Exception ex)
        {
            // 回滚事务
            await transaction.RollbackAsync();
            // 异常，删除已部分保存的文件（如果有的话）
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            return ResultHelper.Error($"模型添加失败，原因：{ex.Message}");
        }
    }

    public async Task<ApiResult> DelModelService(long id)
    {
        // TODO 根据id，软删除模型----0未删除；1已删除
        try
        {
            var entity = await _context.AiModels.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = 1;
                await _context.SaveChangesAsync();
            }

            return ResultHelper.Success("请求成功！", "数据已删除");
        }
        catch (Exception e)
        {
            return ResultHelper.Error("删除模型出现异常，请稍后重试");
        }
    }
}