using Interface;
using Microsoft.AspNetCore.Mvc;
using Model.Other;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ExercisesController:ControllerBase

{
    
    private readonly IExercisesService _exercisesService;
    public ExercisesController(IExercisesService exercisesService)
    {
        _exercisesService = exercisesService;
    }
    
    [HttpGet]
    public Task<ApiResult> GetmMthematics()
    {
        return _exercisesService.GetmMthematics();
    }
}