using Microsoft.Extensions.DependencyInjection;
using OIdentNetLib.Application.Authentication;
using OIdentNetLib.Application.Authentication.Contracts;
using OIdentNetLib.Application.OAuth;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Infrastructure.Encryption;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.IO;
using OIdentNetLib.Infrastructure.IO.Contracts;

namespace OIdentNetLib;

public static class ServiceRegistration
{
    public static IServiceCollection AddOIdentNetLibServices(this IServiceCollection services)
    {
        // Infrastructure - IO
        services.AddScoped<IJsonDeserializer, JsonDeserializer>();
        services.AddScoped<IJsonSerializer, JsonSerializer>();
        services.AddScoped<ITextFileReader, TextFileReader>();
        services.AddScoped<ITextFileWriter, TextFileWriter>();
        
        // Infrastructure - Encryption
        services.AddScoped<IAuthorizationCodeCreator, AuthorizationCodeCreator>();
        services.AddScoped<IDecryptor, Decryptor>();
        services.AddScoped<IEncryptor, Encryptor>();
        services.AddScoped<IJwkCreator, JwkCreator>();
        services.AddScoped<IJwtCreator, JwtCreator>();
        services.AddScoped<IJwtValidator, JwtValidator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        // Application - Authentication
        services.AddScoped<IEmailAddressVerifier, IEmailAddressVerifier>();
        services.AddScoped<ILoginProcessor, LoginProcessor>();
        services.AddScoped<ILogoutProcessor, LogoutProcessor>();
        services.AddScoped<IPhoneNumberVerifier, PhoneNumberVerifier>();
        services.AddScoped<IRegistrationProcessor, RegistrationProcessor>();
        
        // Application - OAuth
        services.AddScoped<IAuthorizationCodeCreator, AuthorizationCodeCreator>();
        services.AddScoped<IAuthorizationProcessor, AuthorizationProcessor>();
        services.AddScoped<IAuthorizationSessionValidator, AuthorizationSessionValidator>();
        services.AddScoped<ClientCredentialsProcessor, ClientCredentialsProcessor>();
        services.AddScoped<IClientValidator, ClientValidator>();
        services.AddScoped<IRefreshTokenProcessor, RefreshTokenProcessor>();
        services.AddScoped<IScopeValidator, ScopeValidator>();
        services.AddScoped<ISessionValidator, SessionValidator>();
        services.AddScoped<ITokenProcessor, TokenProcessor>();
        services.AddScoped<ITokenSessionCreator, TokenSessionCreator>();
        services.AddScoped<ITokenSessionProcessor, TokenSessionProcessor>();
        services.AddScoped<ITokenSessionValidator, TokenSessionValidator>();

        return services;
    }
}