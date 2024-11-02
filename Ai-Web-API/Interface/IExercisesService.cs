using Model.Dto.TestPapers;
using Model.Entitys;
using Model.Other;

namespace Interface;

public interface IExercisesService
{
    public Task<ApiResult> GetmMthematics();


    public Task<ApiResult> checkSubmit(SubmitExercisesReq req);

    public Task<ApiResult> SaveReportCard(ReportCard reportCard);
}