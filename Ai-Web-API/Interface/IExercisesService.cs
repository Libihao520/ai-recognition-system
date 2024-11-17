using Model.Dto.TestPapers;
using Model.Entitys;
using Model.Other;

namespace Interface;

public interface IExercisesService
{
    /// <summary>
    /// 获取题目
    /// </summary>
    /// <returns></returns>
    public Task<ApiResult> GetmMthematics();

    /// <summary>
    /// 成绩提交
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<ApiResult> checkSubmit(SubmitExercisesReq req);

    /// <summary>
    /// 成绩中心获取列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<ApiResult> AchievementCenter(AchievementCenterReq req);

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ApiResult> DeleteService(long id);

    /// <summary>
    /// 下载wrod
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<byte[]> DownloadWord(long id);
}