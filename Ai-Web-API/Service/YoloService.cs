using AutoMapper;
using EFCoreMigrations;
using Interface;
using Model.Dto.Yolo;

namespace Service;

public class YoloService:IYoloService
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
        var  yoloPkqResList = _mapper.Map<List<YoloPkqRes>>(yolotb);
        return yoloPkqResList;
    }
}