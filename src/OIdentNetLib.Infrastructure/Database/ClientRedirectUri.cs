namespace OIdentNetLib.Infrastructure.Database;

public class ClientRedirectUri : BaseModel
{
    public Guid? ClientRedirectUriId { get; set; }
    public Uri? Uri { get; set; }
    
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
}