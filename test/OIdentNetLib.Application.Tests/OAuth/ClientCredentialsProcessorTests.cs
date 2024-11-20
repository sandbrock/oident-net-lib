using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.Tests.OAuth;

[Trait("Category", "Unit")]
public class ClientCredentialsProcessorTests
{
    [Fact]
    public async Task ProcessAsync_InvalidInput_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientCredentialsProcessor>>();
        var clientValidator = new Mock<IClientValidator>();
        var requestMetadata = new RequestMetadata();
        var processTokenRequest = new ProcessTokenRequest();
        var clientCredentialsProcessor = new ClientCredentialsProcessor(
            logger.Object,
            clientValidator.Object);
        
        // Act
        var response = await clientCredentialsProcessor.ProcessAsync(
            requestMetadata, 
            processTokenRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidGrantType_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientCredentialsProcessor>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidClientId,
                OAuthErrorTypes.InvalidRequest,
                null));
        var requestMetadata = new RequestMetadata();
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "client_creds",
            ClientId = "client-id",
            ClientSecret = "client-secret",
        };
        var clientCredentialsProcessor = new ClientCredentialsProcessor(
            logger.Object,
            clientValidator.Object);
        
        // Act
        var response = await clientCredentialsProcessor.ProcessAsync(
            requestMetadata, 
            processTokenRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.OIdentError.Should().Be(OIdentErrors.InvalidGrantType);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidClientId_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientCredentialsProcessor>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidClientId,
                OAuthErrorTypes.InvalidRequest,
                null));
        var requestMetadata = new RequestMetadata();
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "client_credentials",
            ClientId = "client-id",
            ClientSecret = "client-secret",
        };
        var clientCredentialsProcessor = new ClientCredentialsProcessor(
            logger.Object,
            clientValidator.Object);
        
        // Act
        var response = await clientCredentialsProcessor.ProcessAsync(
            requestMetadata, 
            processTokenRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.OIdentError.Should().Be(OIdentErrors.InvalidClientId);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidClientSecret_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientCredentialsProcessor>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                OIdentErrors.InvalidClientSecret,
                OAuthErrorTypes.UnauthorizedClient,
                null));
        var requestMetadata = new RequestMetadata();
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "client_credentials",
            ClientId = "client-id",
            ClientSecret = "client-secret",
        };
        var clientCredentialsProcessor = new ClientCredentialsProcessor(
            logger.Object,
            clientValidator.Object);
        
        // Act
        var response = await clientCredentialsProcessor.ProcessAsync(
            requestMetadata, 
            processTokenRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.OIdentError.Should().Be(OIdentErrors.InvalidClientSecret);
    }
    
    [Fact]
    public async Task ProcessAsync_InvalidScope_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientCredentialsProcessor>>();
        var clientValidator = new Mock<IClientValidator>();
        clientValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidateClientRequest>()))
            .ReturnsAsync(GenericHttpResponse<ValidateClientResponse>.CreateSuccessResponseWithData(
                HttpStatusCode.OK,
                new ValidateClientResponse()
                {
                    ClientId = Guid.NewGuid(),
                    ClientName = "test-client",
                }));
        var requestMetadata = new RequestMetadata();
        var processTokenRequest = new ProcessTokenRequest()
        {
            GrantType = "client_credentials",
            ClientId = "client-id",
            ClientSecret = "client-secret",
            Scope = "invalid-scope",
            Resource = "https://api.example.com",
        };
        var clientCredentialsProcessor = new ClientCredentialsProcessor(
            logger.Object,
            clientValidator.Object);
        
        // Act
        var response = await clientCredentialsProcessor.ProcessAsync(
            requestMetadata, 
            processTokenRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.OIdentError.Should().Be(OIdentErrors.InvalidScope);
    }
}