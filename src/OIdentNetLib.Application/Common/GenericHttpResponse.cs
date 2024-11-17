using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace OIdentNetLib.Application.Common;

/// <summary>
/// Represents a generic response that is not tied to the HTTP
/// protocol libraries. This allows other clients to consume
/// the Application layer, including APIs and console applications.
/// </summary>
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

    public string? OIdentError { get; set; }

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
    
    public static GenericHttpResponse<T> CreateErrorResponse(
        HttpStatusCode statusCode,
        string? oidentError,
        string? error,
        string? errorDescription)
    {
        return new GenericHttpResponse<T>
        {
            StatusCode = statusCode,
            OIdentError = oidentError,
            Error = error,
            ErrorDescription = errorDescription
        };
    }

    public static GenericHttpResponse<T> CreateRedirectResponse(
        Uri uri, 
        string? oidentError,
        string? error, 
        string? errorDescription)
    {
        var url = new StringBuilder();
        url.Append(uri);
        
        if (!string.IsNullOrEmpty(error))
        {
            url.Append($"?error={error}");
        }
        
        if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(errorDescription))
        {
            url.Append($"&error_description={errorDescription}");
        }
        
        return new GenericHttpResponse<T>
        {
            StatusCode = HttpStatusCode.Redirect,
            OIdentError = oidentError,
            Error = error,
            ErrorDescription = errorDescription,
            Uri = new Uri(url.ToString())
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