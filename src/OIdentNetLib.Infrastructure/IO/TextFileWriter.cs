using System.Diagnostics.CodeAnalysis;
using OIdentNetLib.Infrastructure.IO.Contracts;

namespace OIdentNetLib.Infrastructure.IO;

[ExcludeFromCodeCoverage]
public class TextFileWriter : ITextFileWriter
{
    public async Task WriteAsync(string path, string content)
    {
        await File.WriteAllTextAsync(path, content);
    }
}