using Model.Dto.photo;
using Model.Dto.Yolo;

namespace Interface;

public interface IYoloService
{
    /// <summary>
    /// 获取表单
    /// </summary>
    /// <returns></returns>
    List<YoloPkqRes> getpkqTb();

    /// <summary>
    /// 上传照片识别
    /// </summary>
    /// <param name="po"></param>
    /// <returns></returns>
    Task<string> PutPhoto(PhotoAdd po);

    /// <summary>
    /// 获取表单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<YoloPkqEditRes> GetPkqEdtTb(long id);
}