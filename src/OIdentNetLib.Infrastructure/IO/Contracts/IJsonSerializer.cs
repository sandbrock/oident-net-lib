namespace OIdentNetLib.Infrastructure.IO.Contracts;

public interface IJsonSerializer
{
    Task<string> SerializeAsync(object value);
}