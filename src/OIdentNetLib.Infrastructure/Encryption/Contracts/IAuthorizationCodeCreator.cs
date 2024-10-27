namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IAuthorizationCodeCreator
{
    public const int DefaultLength = 32;

    public string Create(int length = DefaultLength);
}