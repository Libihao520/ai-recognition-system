namespace Model.Dto.Yolo;

public class YoloPkqReq
{
    /// <summary>
    /// 类别
    /// </summary>
    public string clsName { get; set; }


    /// <summary>
    /// 是否人工审核
    /// </summary>
    public int isaudit { get; set; }
}