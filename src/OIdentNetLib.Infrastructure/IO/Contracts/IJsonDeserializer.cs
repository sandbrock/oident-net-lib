namespace OIdentNetLib.Infrastructure.IO.Contracts;

public interface IJsonDeserializer
{
    Task<T?> DeserializeAsync<T>(string json);
}