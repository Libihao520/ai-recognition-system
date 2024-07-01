namespace Service.Common;

public class TimeBasedIdGenerator  
{  
    public static int GenerateId()  
    {  
        // 注意：这里为了简单起见，我们使用了DateTime.Now，但它不够精确。  
        // 在生产环境中，你可能会考虑使用更精确的时间测量方法，如Stopwatch或DateTime.UtcNow.Ticks。  
        // 然而，Ticks是一个long类型，你可能需要对其进行某种转换才能使用int。  
        // 这里只是演示概念。  
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;  
    }  
 
    // 注意：由于int的容量有限，上述方法在实际应用中可能会导致ID冲突，  
    // 特别是当两个ID在同一秒内生成时。  
}