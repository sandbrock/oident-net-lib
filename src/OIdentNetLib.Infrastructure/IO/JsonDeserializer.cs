using OIdentNetLib.Infrastructure.IO.Contracts;

namespace OIdentNetLib.Infrastructure.IO;

public class JsonDeserializer : IJsonDeserializer
{
    public async Task<T?> DeserializeAsync<T>(string value)
    {
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(value);
        using var streamReader = new MemoryStream(byteArray);
        var result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(streamReader);
        return result;
    }
}