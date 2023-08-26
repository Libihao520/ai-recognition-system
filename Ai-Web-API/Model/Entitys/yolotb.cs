using System.ComponentModel.DataAnnotations;
using Model.Common;

namespace Model.Entitys;

public class yolotb : IEntity
{
    /// <summary>
    /// 类别
    /// </summary>
    [Required]
    public string Cls { get; set; }

    /// <summary>
    /// 识别结果数量
    /// </summary>
    [Required]
    public int sbjgCount { get; set; }

    /// <summary>
    /// 是否人工审核
    /// </summary>
    [Required]
    public bool IsManualReview { get; set; }

    /// <summary>
    /// 识别正确数量
    /// </summary>
    [Required]
    public int sbzqCount { get; set; }

    /// <summary>
    /// 人工目视数量
    /// </summary>
    [Required]
    public int rgmsCount { get; set; }

    /// <summary>
    /// 准确率
    /// </summary>
    [Required]
    public double zql { get; set; }

    /// <summary>
    /// 召回率
    /// </summary>
    [Required]
    public double zhl { get; set; }
}