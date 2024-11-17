using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Application.Options;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.Tests.OAuth;

public class AuthorizationProcessorTests
{
    [Fact]
    public async Task ProcessAsync_InvalidClientId_ReturnsBadRequest()
    {
        // Arrange
        var options = new Mock<IOptions<OIdentOptions>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidClientId,
                OAuthErrorTypes.InvalidRequest,
                "Invalid client_id parameter."));
        var authorizationCodeCreator = new Mock<IAuthorizationCodeCreator>();
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationProcessor = new AuthorizationProcessor(
            options.Object,
            clientValidator.Object,
            authorizationCodeCreator.Object,
            authorizationSessionValidator.Object,
            authorizationSessionWriter.Object);
        var processAuthorizationRequest = new ProcessAuthorizationRequest
        {
            ResponseType = "code",
            ClientId = Guid.NewGuid(),
            ClientSecret = "secret",
            RedirectUri = new Uri("https://example.com/callback"),
            Scope = "read write",
            State = "state",
            CodeChallenge = "code_challenge",
            CodeChallengeMethod = "code_challenge_method"
        };
        
        // Act
        var response = await authorizationProcessor.ProcessAsync(
            new RequestMetadata(),
            processAuthorizationRequest, 
            new ValidateSessionRequest());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Error.Should().Be(OAuthErrorTypes.InvalidRequest);
        response.ErrorDescription.Should().Be("Invalid client_id parameter.");
    }

    [Fact]
    public async Task ProcessAsync_InvalidRedirectUri_ReturnsBadRequest()
    {
        // Arrange
        var options = new Mock<IOptions<OIdentOptions>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidRedirectUri,
                OAuthErrorTypes.InvalidRequest,
                "Invalid redirect_uri parameter."));
        var authorizationCodeCreator = new Mock<IAuthorizationCodeCreator>();
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationProcessor = new AuthorizationProcessor(
            options.Object,
            clientValidator.Object,
            authorizationCodeCreator.Object,
            authorizationSessionValidator.Object,
            authorizationSessionWriter.Object);
        var processAuthorizationRequest = new ProcessAuthorizationRequest
        {
            ResponseType = "code",
            ClientId = Guid.NewGuid(),
            ClientSecret = "secret",
            RedirectUri = new Uri("https://example.com/callback"),
            Scope = "read write",
            State = "state",
            CodeChallenge = "code_challenge",
            CodeChallengeMethod = "code_challenge_method"
        };
        
        // Act
        var response = await authorizationProcessor.ProcessAsync(
            new RequestMetadata(),
            processAuthorizationRequest, 
            new ValidateSessionRequest());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Error.Should().Be(OAuthErrorTypes.InvalidRequest);
        response.ErrorDescription.Should().Be("Invalid redirect_uri parameter.");
    }

    [Fact]
    public async Task ProcessAsync_InvalidResponseType_RedirectsWithError()
    {
        // Arrange
        var options = new Mock<IOptions<OIdentOptions>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateSuccessResponse(HttpStatusCode.OK));
        var authorizationCodeCreator = new Mock<IAuthorizationCodeCreator>();
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationProcessor = new AuthorizationProcessor(
            options.Object,
            clientValidator.Object,
            authorizationCodeCreator.Object,
            authorizationSessionValidator.Object,
            authorizationSessionWriter.Object);
        var processAuthorizationRequest = new ProcessAuthorizationRequest
        {
            ResponseType = "token",
            ClientId = Guid.NewGuid(),
            ClientSecret = "secret",
            RedirectUri = new Uri("https://example.com/callback"),
            Scope = "read write",
            State = "state",
            CodeChallenge = "code_challenge",
            CodeChallengeMethod = "code_challenge_method"
        };
        
        // Act
        var response = await authorizationProcessor.ProcessAsync(
            new RequestMetadata(),
            processAuthorizationRequest, 
            new ValidateSessionRequest());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Uri.Should().NotBeNull();
        response.Uri!.ToString().Should().Be("https://example.com/callback?error=unsupported_response_type&error_description=Unsupported response_type parameter.");
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidClientSecret_RedirectsWithError()
    {
        // Arrange
        var options = new Mock<IOptions<OIdentOptions>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                OIdentErrors.InvalidClientSecret,
                OAuthErrorTypes.UnauthorizedClient,
                "Invalid client credentials."));
        var authorizationCodeCreator = new Mock<IAuthorizationCodeCreator>();
        var authorizationSessionValidator = new Mock<IAuthorizationSessionValidator>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationProcessor = new AuthorizationProcessor(
            options.Object,
            clientValidator.Object,
            authorizationCodeCreator.Object,
            authorizationSessionValidator.Object,
            authorizationSessionWriter.Object);
        var processAuthorizationRequest = new ProcessAuthorizationRequest
        {
            ResponseType = "code",
            ClientId = Guid.NewGuid(),
            ClientSecret = "secret",
            RedirectUri = new Uri("https://example.com/callback"),
            Scope = "read write",
            State = "state",
            CodeChallenge = "code_challenge",
            CodeChallengeMethod = "code_challenge_method"
        };
        
        // Act
        var response = await authorizationProcessor.ProcessAsync(
            new RequestMetadata(),
            processAuthorizationRequest, 
            new ValidateSessionRequest());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Error.Should().Be(OAuthErrorTypes.UnauthorizedClient);
        response.ErrorDescription.Should().Be("Invalid client credentials.");
    }
    
}