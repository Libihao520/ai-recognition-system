using System.Text.Json;
using System.Text.Json.Serialization;

namespace Model.Enum;

public class EnumJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, System.Enum  
{  
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)  
    {  
        if (reader.TokenType != JsonTokenType.String)  
        {  
            throw new JsonException();  
        }  
  
        var enumString = reader.GetString();  
        if (System.Enum.TryParse<TEnum>(enumString, true, out TEnum result))  
        {  
            return result;  
        }  
  
        throw new JsonException($"Could not convert string '{enumString}' to enum {typeof(TEnum).Name}.");  
    }  
  
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)  
    {  
        writer.WriteStringValue(value.ToString());  
    }  
}