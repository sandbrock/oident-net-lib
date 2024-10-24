using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IJwtCreator
{
    MakeJwtResponse Create(MakeJwtRequest request);
}