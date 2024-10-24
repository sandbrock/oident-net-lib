using Microsoft.IdentityModel.Tokens;

namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IJwkCreator
{
    JsonWebKey Create();
}