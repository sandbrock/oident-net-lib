namespace OIdentNetLib.Infrastructure.Errors;

public class OIdentErrors
{
    public const string ExpiredAuthorizationCode = "expired_authorization_code";
    public const string InternalServerError = "internal_server_error"; 
    
    public const string InvalidAuthorizationCode = "invalid_authorization_code";
    public const string InvalidClientId = "invalid_client_id";
    public const string InvalidClientSecret = "invalid_client_secret";
    public const string InvalidGrantType = "invalid_grant_type";
    public const string InvalidRedirectUri = "invalid_redirect_uri";
    public const string InvalidRequest = "invalid_request";
    public const string InvalidRefreshToken = "invalid_refresh_token";
    public const string InvalidResourceServer = "invalid_resource_server";
    public const string InvalidResponseType = "invalid_response_type";
    public const string InvalidScope = "invalid_scope";
    public const string InvalidSession = "invalid_session";
    public const string InvalidTenant = "invalid_tenant";
    
    public const string ValidSessionFound = "valid_session_found";
}