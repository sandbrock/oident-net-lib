namespace OIdentNetLib.Infrastructure.Database.DataTransferObjects;

public class PagedResponse<T>
{
    public IList<T>? Data { get; set; }
    public int PageNumber { get; set; }
    public int TotalRowCount { get; set; }
}