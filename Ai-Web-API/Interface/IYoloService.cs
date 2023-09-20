using Model.Dto.photo;
using Model.Dto.Yolo;

namespace Interface;

public interface IYoloService
{
    /// <summary>
    /// 获取表单
    /// </summary>
    /// <returns></returns>
    Task<List<YoloPkqRes>> getpkqTb();

    /// <summary>
    /// 上传照片识别
    /// </summary>
    /// <param name="po"></param>
    /// <returns></returns>
    Task<string> PutPhoto(PhotoAdd po,CancellationToken cancellationToken);

    /// <summary>
    /// 获取表单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<YoloPkqEditRes> GetPkqEdtTb(long id);

    /// <summary>
    /// 获取数据大屏数据
    /// </summary>
    /// <returns></returns>
    Task<YoloSjdpRes> Getsjdp();
}