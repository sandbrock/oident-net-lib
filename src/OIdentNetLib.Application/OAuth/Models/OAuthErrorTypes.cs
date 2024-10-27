namespace OIdentNetLib.Application.OAuth.Models;

public static class OAuthErrorTypes
{
    public const string InvalidRequest = "invalid_request";
    public const string UnauthorizedClient = "unauthorized_client";
    public const string AccessDenied = "access_denied";
    public const string UnsupportedResponseType = "unsupported_response_type";
    public const string InvalidScope = "invalid_scope";
    public const string ServerError = "server_error";
    public const string TemporarilyUnavailable = "temporarily_unavailable";
}