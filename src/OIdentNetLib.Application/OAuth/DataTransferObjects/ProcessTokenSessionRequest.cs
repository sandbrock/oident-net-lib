namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ProcessTokenSessionRequest
{
   public Guid? SessionId { get; set; }
   public Guid? ClientId { get; set; }
   public Guid? UserId { get; set; }
   public string? Scope { get; set; }
}