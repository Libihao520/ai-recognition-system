using Model.Dto.photo;
using Model.Dto.Yolo;

namespace Interface;

public interface IYoloService
{
    List<YoloPkqRes> getpkqTb();
    Task<string> PutPhoto(PhotoAdd po);
}