namespace OIdentNetLib.Infrastructure.IO.Contracts;

public interface ITextFileReader
{
    Task<string> ReadAsync(string path);
}