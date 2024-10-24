using System.Text.Json;
using OIdentNetLib.Infrastructure.IO.Contracts;

namespace OIdentNetLib.Infrastructure.IO;

public class JsonSerializer : IJsonSerializer
{
    public async Task<string> SerializeAsync(object value)
    {
        var options = new JsonSerializerOptions() { WriteIndented = true };
        using var streamWriter = new MemoryStream();
        await System.Text.Json.JsonSerializer.SerializeAsync(streamWriter, value, options);

        streamWriter.Position = 0;
        using var streamReader = new StreamReader(streamWriter);
        var result = await streamReader.ReadToEndAsync();
        return result;
    }
}