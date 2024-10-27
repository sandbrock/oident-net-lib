using System.Net;
using Microsoft.AspNetCore.Http;

namespace OIdentNetLib.Application.Common;

public class GenericHttpResponse<T>
{
    /// <summary>
    /// The HTTP status code to include in the HTTP response
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// An error to include in the body of the HTTP response
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// An description of the error to include in the body of the HTTP response
    /// </summary>
    public string? ErrorDescription { get; set; }

    /// <summary>
    /// Data to include in the body of the HTTP response
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// Uri to include in the HTTP response
    /// </summary>
    public Uri? Uri { get; set; }
    
    public GenericHttpResponse()
    {
        StatusCode = HttpStatusCode.OK;
    }

    /// <summary>
    /// Returns true if the response is successful
    /// </summary>
    public bool IsSuccess =>
        StatusCode >= HttpStatusCode.OK &&
        StatusCode < HttpStatusCode.Ambiguous;

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
                return Results.Conflict(new ErrorResponse(Error, ErrorDescription));
            case HttpStatusCode.BadRequest:
                return Results.BadRequest(new ErrorResponse(Error, ErrorDescription));
            case HttpStatusCode.NotFound:
                return Results.NotFound(new ErrorResponse(Error, ErrorDescription));
            case HttpStatusCode.Unauthorized:
                var results = Results.Json(
                    data: new ErrorResponse(Error, ErrorDescription),
                    statusCode: (int)HttpStatusCode.Unauthorized);
                return results;
            default:
                return Results.Json(
                    data: new ErrorResponse(Error, ErrorDescription),
                    statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    public static GenericHttpResponse<T> CreateSuccessResponse(HttpStatusCode statusCode)
    {
        return new GenericHttpResponse<T>
        {
            StatusCode = statusCode
        };
    }
    
    public static GenericHttpResponse<T> CreateErrorResponse(HttpStatusCode statusCode, string? error, string? errorDescription)
    {
        return new GenericHttpResponse<T>
        {
            StatusCode = statusCode,
            Error = error,
            ErrorDescription = errorDescription
        };
    }

    public static GenericHttpResponse<T> CreateRedirectResponse(Uri uri)
    {
        return new GenericHttpResponse<T>
        {
            StatusCode = HttpStatusCode.Redirect,
            Uri = uri
        };
    }

    public static GenericHttpResponse<T> CreateSuccessResponseWithData(HttpStatusCode statusCode, T? data)
    {
        return new GenericHttpResponse<T>
        {
            StatusCode = statusCode,
            Data = data
        };
    }
}