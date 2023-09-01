using AutoMapper;
using EFCoreMigrations;
using Interface;
using Model.Dto.photo;
using Model.Dto.Yolo;
using Model.Entitys;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Service;

public class YoloService : IYoloService
{
    private readonly IMapper _mapper;
    private MyDbContext _context;

    public YoloService(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<YoloPkqRes> getpkqTb()
    {
        var yolotb = _context.yolotbs.ToList();
        var yoloPkqResList = _mapper.Map<List<YoloPkqRes>>(yolotb);
        return yoloPkqResList;
    }

    public async Task<string> PutPhoto(PhotoAdd po)
    {
        string base64 = po.photo.Substring(po.photo.IndexOf(',') + 1);
        byte[] data = Convert.FromBase64String(base64);
        ByteArrayContent bytes = new ByteArrayContent(data);
        MemoryStream stream = new MemoryStream(data);


        var client = new RestClient("http://127.0.0.1:8005/detect");
        client.UseNewtonsoftJson();
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddFile("file_list", stream.ToArray(), "filename.png");
        request.AddParameter("model_name", po.name);
        request.AddParameter("download_image", "True");
        if (po.name == "皮卡丘")
        {
            var response = await client.ExecuteAsync<PhotoRes>(request);
            if (response.Data.Code == 200)
            {
                var yolotbs = new Yolotbs()
                {
                    Cls = po.name,
                    sbjgCount = response.Data.result.Count,
                    IsManualReview = false,
                    Photo = response.Data.Image_Base64,
                    sbzqCount = 0,
                    rgmsCount = 0,
                    zql = 0,
                    zhl = 0,
                    CreateDate = DateTime.Now,
                    CreateUserId = 0,
                    IsDeleted = 0
                };
                _context.yolotbs.Add(yolotbs);
                _context.SaveChanges();
            }

            return response.Data.Image_Base64;
        }
        else
        {
            var response = await client.ExecuteAsync<PhotoCarRes>(request);
            return response.Data.Image_Base64;
        }
    }
}