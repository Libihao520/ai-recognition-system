namespace CommonUtil.YoloUtil;

public class ExcelDataParser
{
    /// <summary>
    /// 1,2,3    =====>[1,2,3]
    /// 4,空格5,6 =====>[4,5,6]
    /// 7,a,8    ====>[7,8]
    /// </summary>
    /// <param name="cellValue"></param>
    /// <returns></returns>
    public static List<int> ParseAnswerFromCellValue(object cellValue)
    {
        var result = new List<int>();
        // 判断传入的单元格值cellValue是否为null，如果不为null，才进行后续的解析操作，避免对null值进行操作导致异常
        if (cellValue != null)
        {
            // 将传入的object类型的单元格值转换为字符串类型，方便后续按照特定格式进行解析，因为后续要按照逗号分隔等文本处理方式来提取数据
            var valueStr = cellValue.ToString();
            // 判断转换后的字符串valueStr是否为空字符串，如果不为空字符串，说明有实际的数据内容可以进行解析
            if (!string.IsNullOrEmpty(valueStr))
            {
                // 使用string.Split(',')方法按照逗号对字符串进行分割，将其拆分成多个子字符串，存储在parts数组中，
                // 假设Excel单元格中的数据是以逗号分隔的整数形式存储，这里就是按照这个规则进行拆分
                var parts = valueStr.Split(',');
                // 开始遍历拆分后得到的每个子字符串（每个部分），目的是逐个尝试将它们转换为整数并添加到结果列表中
                foreach (var part in parts)
                {
                    // 使用int.TryParse方法尝试将当前子字符串part转换为整数，该方法会返回一个布尔值表示是否转换成功，
                    // 如果转换成功，会将转换后的整数值赋给out参数parsedValue
                    if (int.TryParse(part.Trim(), out int parsedValue))
                    {
                        // 如果成功转换为整数，将该整数添加到result列表中，逐步构建最终的整数列表
                        result.Add(parsedValue);
                    }
                }
            }
        }

        // 返回解析完成后的整数列表result，该列表包含了从Excel单元格值中成功解析出的所有整数元素
        return result;
    }
}