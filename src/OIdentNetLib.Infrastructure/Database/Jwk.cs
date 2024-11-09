namespace OIdentNetLib.Infrastructure.Database;

public class Jwk : BaseModel
{
    public Guid? JwkId { get; set; }
    public string? KeyId { get; set; }
    public string? EncryptedKey { get; set; }
}