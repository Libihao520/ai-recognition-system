namespace Model.Dto.Yolo;

public class YoloDetectionQueryReq
{
    /// <summary>
    /// 当前页码
    /// </summary>
    public int pagenum { get; set; }

    /// <summary>
    /// 每页条数
    /// </summary>
    public int pagesize { get; set; }

    /// <summary>
    /// 类别
    /// </summary>
    public string clsName { get; set; }


    /// <summary>
    /// 是否人工审核
    /// </summary>
    public int isaudit { get; set; }
}