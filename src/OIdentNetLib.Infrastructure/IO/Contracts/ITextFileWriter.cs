namespace OIdentNetLib.Infrastructure.IO.Contracts;

public interface ITextFileWriter
{
    Task WriteAsync(string path, string content);
}