using System.Net;
using FluentAssertions;
using Moq;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.Tests.OAuth;

[Trait("Category", "Unit")]
public class AuthorizationCodeProcessorTests
{
    [Fact]
    public async Task ProcessAsync_InvalidInput_ReturnsError()
    {
        // Arrange
        var clientValidator = new Mock<IClientValidator>();
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var tokenSessionCreator = new Mock<ITokenSessionCreator>();
        var requestMetadata = new RequestMetadata();
        var authorizationCodeProcessor = new AuthorizationCodeProcessor(
            clientValidator.Object, 
            authorizationSessionValidator.Object, 
            authorizationSessionWriter.Object,
            tokenSessionCreator.Object);
        var processTokenRequest = new ProcessTokenRequest();
        
        // Act
        var processTokenResponse = await authorizationCodeProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);

        // Assert
        processTokenResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidGranteType_ReturnsError()
    {
        // Arrange
        var clientValidator = new Mock<IClientValidator>();
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var tokenSessionCreator = new Mock<ITokenSessionCreator>();
        var requestMetadata = new RequestMetadata();
        var authorizationCodeProcessor = new AuthorizationCodeProcessor(
            clientValidator.Object, 
            authorizationSessionValidator.Object, 
            authorizationSessionWriter.Object,
            tokenSessionCreator.Object);
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "invalid",
            ClientId = "client-id",
            ClientSecret = "client-secret",
            Code = "code",
            RedirectUri = new Uri("https://example.com/callback"),
        };
        
        // Act
        var processTokenResponse = await authorizationCodeProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);

        // Assert
        processTokenResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        processTokenResponse.OIdentError.Should().Be(OIdentErrors.InvalidResponseType);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidClientId_ReturnsError()
    {
        // Arrange
        var clientValidator = new Mock<IClientValidator>();
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var tokenSessionCreator = new Mock<ITokenSessionCreator>();
        var requestMetadata = new RequestMetadata();
        var authorizationCodeProcessor = new AuthorizationCodeProcessor(
            clientValidator.Object, 
            authorizationSessionValidator.Object, 
            authorizationSessionWriter.Object,
            tokenSessionCreator.Object);
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "authorization_code",
            ClientId = "client-id",
            ClientSecret = "client-secret",
            Code = "code",
            RedirectUri = new Uri("https://example.com/callback"),
        };
        
        // Act
        var processTokenResponse = await authorizationCodeProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);

        // Assert
        processTokenResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        processTokenResponse.OIdentError.Should().Be(OIdentErrors.InvalidClientId);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidClient_ReturnsError()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidClientId,
                OAuthErrorTypes.InvalidClient,
                null));
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var tokenSessionCreator = new Mock<ITokenSessionCreator>();
        var requestMetadata = new RequestMetadata();
        var authorizationCodeProcessor = new AuthorizationCodeProcessor(
            clientValidator.Object, 
            authorizationSessionValidator.Object, 
            authorizationSessionWriter.Object,
            tokenSessionCreator.Object);
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "authorization_code",
            ClientId = clientId.ToString(),
            ClientSecret = "client-secret",
            Code = "code",
            RedirectUri = new Uri("https://example.com/callback"),
        };
        
        // Act
        var processTokenResponse = await authorizationCodeProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);

        // Assert
        processTokenResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        processTokenResponse.OIdentError.Should().Be(OIdentErrors.InvalidClientId);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidClientSecret_ReturnsError()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                OIdentErrors.InvalidClientSecret,
                OAuthErrorTypes.UnauthorizedClient,
                null));
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var tokenSessionCreator = new Mock<ITokenSessionCreator>();
        var requestMetadata = new RequestMetadata();
        var authorizationCodeProcessor = new AuthorizationCodeProcessor(
            clientValidator.Object, 
            authorizationSessionValidator.Object, 
            authorizationSessionWriter.Object,
            tokenSessionCreator.Object);
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "authorization_code",
            ClientId = clientId.ToString(),
            ClientSecret = "client-secret",
            Code = "code",
            RedirectUri = new Uri("https://example.com/callback"),
        };
        
        // Act
        var processTokenResponse = await authorizationCodeProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);

        // Assert
        processTokenResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        processTokenResponse.OIdentError.Should().Be(OIdentErrors.InvalidClientSecret);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidRedirectUri_ReturnsError()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidRedirectUri,
                OAuthErrorTypes.InvalidRedirectUri,
                null));
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var tokenSessionCreator = new Mock<ITokenSessionCreator>();
        var requestMetadata = new RequestMetadata();
        var authorizationCodeProcessor = new AuthorizationCodeProcessor(
            clientValidator.Object, 
            authorizationSessionValidator.Object, 
            authorizationSessionWriter.Object,
            tokenSessionCreator.Object);
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "authorization_code",
            ClientId = clientId.ToString(),
            ClientSecret = "client-secret",
            Code = "code",
            RedirectUri = new Uri("https://example.com/invalid"),
        };
        
        // Act
        var processTokenResponse = await authorizationCodeProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);

        // Assert
        processTokenResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        processTokenResponse.OIdentError.Should().Be(OIdentErrors.InvalidRedirectUri);
    }
    
    [Fact]
    public async Task ProcessAsync_ValidRequest_ReturnsTokens()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clientRedirectUriId = Guid.NewGuid();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateSuccessResponseWithData(
                HttpStatusCode.OK,
                new ValidateClientResponse()
                {
                    ClientId = clientId,
                    ClientName = "test-client",
                    ClientRedirectUri = new ClientRedirectUri()
                    {
                        ClientRedirectUriId = clientRedirectUriId,
                        Uri = new Uri("https://example.com/callback")
                    }
                }));
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var tokenSessionCreator = new Mock<ITokenSessionCreator>();
        var requestMetadata = new RequestMetadata();
        var authorizationCodeProcessor = new AuthorizationCodeProcessor(
            clientValidator.Object, 
            authorizationSessionValidator.Object, 
            authorizationSessionWriter.Object,
            tokenSessionCreator.Object);
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "authorization_code",
            ClientId = clientId.ToString(),
            ClientSecret = "client-secret",
            Code = "code",
            RedirectUri = new Uri("https://example.com/invalid"),
        };
        
        // Act
        var processTokenResponse = await authorizationCodeProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);

        // Assert
        processTokenResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        processTokenResponse.Data.Should().NotBeNull();
        processTokenResponse.Data!.RefreshToken.Should().NotBeNullOrEmpty();
        processTokenResponse.Data!.AccessToken.Should().NotBeNullOrEmpty();
    }
}