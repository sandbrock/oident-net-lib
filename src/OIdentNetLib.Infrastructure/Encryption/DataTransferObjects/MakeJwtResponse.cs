namespace OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

public class MakeJwtResponse
{
    public string? JwtId { get; set; }
    public string? Jwt { get; set; }
    public DateTime? Expires { get; set; }
}