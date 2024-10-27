using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IJwtValidator
{
    Task<ValidateJwtResponse> ValidateAsync(ValidateJwtRequest request);
}