using OIdentNetLib.Infrastructure.IO.Contracts;

namespace OIdentNetLib.Infrastructure.IO;

public class TextFileReader : ITextFileReader
{
    public async Task<string> ReadAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }
}