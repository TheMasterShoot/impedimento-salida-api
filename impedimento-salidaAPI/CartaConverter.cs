using Newtonsoft.Json;

public class CartaConverter : JsonConverter<byte[]>
{
    public override byte[] ReadJson(JsonReader reader, Type objectType, byte[] existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string base64String = (string)reader.Value;
            return Convert.FromBase64String(base64String);
        }
        else if (reader.TokenType == JsonToken.StartObject)
        {
            // Handle empty object scenario
            return new byte[0]; // or return null, based on how you want to handle it
        }

        throw new JsonSerializationException("Unexpected token type for byte array.");
    }

    public override void WriteJson(JsonWriter writer, byte[] value, JsonSerializer serializer)
    {
        writer.WriteValue(Convert.ToBase64String(value));
    }
}
