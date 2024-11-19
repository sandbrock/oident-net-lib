using System.Text.Json;
using OIdentNetLib.Infrastructure.IO.Contracts;

namespace OIdentNetLib.Infrastructure.IO;

public class JsonSerializer : IJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    
    public async Task<string> SerializeAsync(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }
        
        using var streamWriter = new MemoryStream();
        await System.Text.Json.JsonSerializer.SerializeAsync(streamWriter, value, Options);

        streamWriter.Position = 0;
        using var streamReader = new StreamReader(streamWriter);
        var result = await streamReader.ReadToEndAsync();
        return result;
    }
}