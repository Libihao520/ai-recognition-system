using AutoMapper;
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
    public AigcSerevic(MyDbContext context, IMapper mapper,IHttpContextAccessor httpContextAccessor)
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

    public async Task<ApiResult> PutModelService(PutModelReq req)
    {
        // TODO 将传过来的文件Model，（判断是否存在，存在则抛错误（模型名称重复））保存到本地Mdoel文件夹下
        if (req == null)
        {
            return ResultHelper.Error("不能为空");
        }

        if (string.IsNullOrEmpty(req.ModelName))
        {
            return ResultHelper.Error("模型名称不能为空");
        }

        if (string.IsNullOrEmpty(req.ModleCls))
        {
            return ResultHelper.Error("模型类型不能为空");
        }

        if (req.Model == null && req.Model.Count == 0)
        {
            return ResultHelper.Error("模型文件异常");
        }

        // 检查本地Model文件夹是否存在，不存在则创建
        // 获取应用当前目录，并与"Model"文件夹名拼接成完整的文件夹路径
        var modelFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Model");
        // 判断Model文件夹是否不存在
        if (!Directory.Exists(modelFolderPath))
        {
            // 创建Model文件夹
            Directory.CreateDirectory(modelFolderPath);
        }
        // 在数据库中查找是否已存在同名模型（此处原逻辑保留，后续结合唯一约束及事务增强并发处理）
        var existingmodel = _context.AiModels.FirstOrDefault(a => a.ModelName == req.ModelName);
        if (existingmodel != null)
        {
            return ResultHelper.Error("模型已存在，请更换名称后重新添加");
        }

        //获取createId
        var user = _httpContextAccessor.HttpContext.User;
        var createUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "Id").Value);

        // 定义允许的模型文件扩展名数组，这里假设只允许.pt和.onnx格式，可按需调整
        var alloweExtensions = new[] { ".pt", ".onnx" };
        foreach (var formFile in req.Model)
        {
            // 获取当前文件的扩展名，并转换为小写形式，方便后续比较
            var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (!alloweExtensions.Contains(fileExtension))
            {
                return ResultHelper.Error($"不支持文件格式，仅支持{String.Join(",", alloweExtensions)}格式的模型文件");
            }
        }

        /*获取文件名称，扩展名
         用于存储模型文件的相对路径 --先将"Model"文件夹名与模型名称、一个新的唯一标识符（Guid）、文件原始扩展名拼接起来，构成相对路径
           通过Guid生成一个32位的十六进制数字字符串123e4567-e89b转为字符串123e4567e89b---得到相对路径
           */
        var relativeFilePath = Path.Combine("Model",
            req.ModelName + Guid.NewGuid().ToString("N") + Path.GetExtension(req.Model.First().FileName));
        // 再将应用当前目录与相对路径拼接，得到文件的绝对完整路径，用于后续保存文件到磁盘
        var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath);

        try
        {
            // 开启一个数据库事务，确保一系列相关数据库操作的原子性（要么全部成功，要么全部回滚）
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //文件保存到本地
                foreach (var formFile in req.Model)
                {
                    //创建文件流，创建模式打开目标文件，不存在则创建，存在则覆盖
                    using (var stream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        // 将上传的文件内容从请求中的IFormFile流复制到本地创建的文件流中，实现文件保存到磁盘
                        await formFile.CopyToAsync(stream);
                    }
                }   
                var aiModels = new AiModels()
                {
                    ModelName = req.ModelName,
                    ModleCls = req.ModleCls,
                    Id = TimeBasedIdGeneratorUtil.GenerateId(),
                    Path = relativeFilePath,  // 设置文件路径到数据库记录中
                    CreateDate = DateTime.Now,
                    IsDeleted = 0,
                    CreateUserId = createUserId,
                };
                var entityEntry = _context.AiModels.Add(aiModels);
                await _context.SaveChangesAsync();
                // 提交数据库事务，如果前面的数据库操作（如插入模型记录）都成功，则事务提交，更改持久化到数据库
                await transaction.CommitAsync();
                // 返回包含新添加模型关键信息（这里是模型Id和文件保存的相对路径）的成功结果，方便调用方后续使用
                return ResultHelper.Success("模型添加成功", new { ModelId = aiModels.Id, ModelPath = relativeFilePath });
            }
            catch (DbUpdateException ex)when (ex.InnerException is MySqlException mysqlEx && mysqlEx.Number == 10062)
            {
                // 返回相应的错误提示信息告知模型已存在
                await transaction.RollbackAsync();
                // 返回相应的错误提示信息告知模型已存在
                return ResultHelper.Error("模型已存在，请更换名称后重新添加");
            }
            catch (Exception ex)
            {
                // 对于其他在事务执行过程中出现的异常（比如文件保存失败或者其他数据库操作异常等），同样回滚事务
                await transaction.RollbackAsync();
                // 文件保存或数据库操作出现异常，删除已部分保存的文件（如果有的话），防止产生无效的文件残留
                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                }

                return ResultHelper.Error($"模型添加失败，原因：{ex.Message}");
            }
        }
        catch (Exception ex)
        {
            // 如果在开启事务等外层操作出现异常，直接返回错误结果告知调用方模型添加失败及失败原因
            return ResultHelper.Error($"模型添加是吧，原因：{ex.Message}");
        }
    }

    public async Task<ApiResult> DelModelService(long id)
    {
        // TODO 根据id，软删除模型----0未删除；1已删除
        try
        {
            var entity =await _context.AiModels.FindAsync(id);
            if (entity!=null)
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