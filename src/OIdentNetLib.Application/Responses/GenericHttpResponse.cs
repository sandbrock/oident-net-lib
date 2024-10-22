using System.Net;
using Microsoft.AspNetCore.Http;

namespace OIdentNetLib.Application.Responses;

public class GenericHttpResponse<T>
{
    /// <summary>
    /// The HTTP status code to include in the HTTP response
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// A message to include in the body of the HTTP response
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Data to include in the body of the HTTP response
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// Uri to include in the HTTP response
    /// </summary>
    public Uri? Uri { get; set; }
    
    /// <summary>
    /// Returns true if the response is successful
    /// </summary>
    public bool IsSuccess()
    {
        return StatusCode >= HttpStatusCode.OK && 
               StatusCode < HttpStatusCode.Ambiguous;
    }
    
    /// <summary>
    /// Converts the response to a Result object for easier handling
    /// </summary>
    public IResult ToHttpResult()
    {
        switch (StatusCode)
        {
            case HttpStatusCode.OK:
                return Results.Ok(Data);
            case HttpStatusCode.Created:
                return Results.Created(Uri, Data);
            case HttpStatusCode.Conflict:
                return Results.Conflict(Message);
            case HttpStatusCode.BadRequest:
                return Results.BadRequest(Message);
            case HttpStatusCode.NotFound:
                return Results.NotFound(Message);
            case HttpStatusCode.Unauthorized:
                var results = Results.Text(
                    content: Message,
                    statusCode: 401);
                return results;
            default:
                return Results.Problem(Message);
        }
    }
}