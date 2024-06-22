using AutoMapper;
using EFCoreMigrations;
using Interface;
using Microsoft.EntityFrameworkCore;
using Model.Dto.photo;
using Model.Dto.Yolo;
using Model.Entitys;
using Model.Other;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using WebApi.Config;
using YoloDotNet;
using YoloDotNet.Extensions;

namespace Service;

public class YoloService : IYoloService
{
    private readonly IMapper _mapper;
    private MyDbContext _context;

    private static readonly string? BasePath =
        Path.GetDirectoryName(typeof(YoloService).Assembly.Location);

    public YoloService(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<YoloPkqRes>> getpkqTb()
    {
        var yolotb = await _context.yolotbs.Where(p=>p.IsDeleted ==0).ToListAsync();
        var yoloPkqResList = _mapper.Map<List<YoloPkqRes>>(yolotb);
        return yoloPkqResList;
    }

    public async Task<string> PutPhoto(PhotoAdd po, CancellationToken cancellationToken)
    {
        string base64 = po.photo.Substring(po.photo.IndexOf(',') + 1);
        byte[] data = Convert.FromBase64String(base64);

        using (MemoryStream ms = new MemoryStream(data))
        {
            using (var image = Image.Load<Rgba32>(ms))
            {
                using var yolo = new Yolo(Path.Combine(BasePath, "best.onnx"), false);
                var results = yolo.RunObjectDetection(image, 0.3);

                image.Draw(results);

                using (MemoryStream outputMs = new MemoryStream())
                {
                    image.Save(outputMs, new JpegEncoder());
                    byte[] imageBytes = outputMs.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);

                    var yolotbs = new Yolotbs()
                    {
                        Cls = po.name,
                        sbjgCount = results.Count,
                        IsManualReview = false,
                        sbzqCount = 0,
                        rgmsCount = 0,
                        zql = 0,
                        zhl = 0,
                        CreateDate = DateTime.Now,
                        CreateUserId = 0,
                        IsDeleted = 0
                    };
                    yolotbs.Photos = new Photos() { Photobase64 = "data:image/jpeg;base64," + base64Image };
                    _context.yolotbs.Add(yolotbs);
                    _context.SaveChanges();
                    return "data:image/jpeg;base64," + base64Image;
                }
            }
        }
    }

    public async Task<YoloPkqEditRes> GetPkqEdtTb(long id)
    {
        var yoloTb = await _context.yolotbs.FindAsync(id);
        if (yoloTb == null)
        {
            return null;
        }

        var yoloPkqEditRes = _mapper.Map<YoloPkqEditRes>(yoloTb);
        var photos = _context.Photos.Where(u => u.PhotosId == yoloTb.PhotosId).FirstOrDefault();
        yoloPkqEditRes.Photo = photos?.Photobase64;
        return yoloPkqEditRes;
    }

    public async Task<YoloSjdpRes> Getsjdp()
    {
        var userCount = await _context.Users.CountAsync();
        var sbcsCount = await _context.yolotbs.CountAsync();
        var mbslCount = await _context.yolotbs.SumAsync(x => x.sbjgCount);
        var yoloSjdpRes = new YoloSjdpRes()
        {
            userCount = userCount,
            sbcsCount = sbcsCount,
            mbslCount = mbslCount,
            yxslCount = mbslCount,
        };
        return yoloSjdpRes;
    }

    #region 添加

    public async Task<Yolotbs> AddAsync(Yolotbs yolotbs)
    {
        _context.yolotbs.Add(yolotbs);
        await _context.SaveChangesAsync();
        return yolotbs;
    }

    #endregion

    #region 删除

    public async Task<ApiResult> DeleteAsync(long id)
    {
        try
        {
            // 根据id查找yoloTb对象
            var yoloTb = await _context.yolotbs.FindAsync(id);
            // 不为空则执行软删除并且保存到数据库中
            if (yoloTb != null)
            {
                yoloTb.IsDeleted = 1;
                await _context.SaveChangesAsync();
            }

            return ResultHelper.Success("请求成功！", "数据已删除");
        }
        catch (Exception e)
        {
            return ResultHelper.Error("yolo数据删除失败");
        }
    }

    #endregion

    #region 查询

    public async Task<Yolotbs?> GetByIdAsync(int id)
    {
        var findAsync = _context.yolotbs.FindAsync(id);
        return await findAsync;
    }

    #endregion
    
    
    
    
}