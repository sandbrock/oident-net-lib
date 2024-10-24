namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IAudienceWriter
{
    Task WriteAsync(Audience audience);
}