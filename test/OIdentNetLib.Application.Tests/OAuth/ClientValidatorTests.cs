using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OIdentNetLib.Application.OAuth;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;

namespace OIdentNetLib.Application.Tests.OAuth;

[Trait("Category", "Unit")]
public class ClientValidatorTests
{
    [Fact]
    public async Task ValidateAsync_WhenClientDoesNotExist_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientValidator>>();
        var clientReader = new Mock<IClientReader>();
        clientReader.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);
        var passwordHasher = new Mock<IPasswordHasher>();
        var clientValidator = new ClientValidator(
            logger.Object,
            clientReader.Object,
            passwordHasher.Object);

        var request = new ValidateClientRequest
        {
            ClientId = Guid.NewGuid()
        };

        // Act
        var result = await clientValidator.ValidateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Error.Should().Be(OAuthErrorTypes.InvalidRequest);
        result.ErrorDescription.Should().Be("Invalid client_id parameter.");
    }
    
    [Fact]
    public async Task ValidateAsync_WhenClientSecretInvalid_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientValidator>>();
        var clientReader = new Mock<IClientReader>();
        clientReader.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new Client()
            {
                ClientId = Guid.NewGuid(),
                ClientSecretHash = "client_secret_hash",
                ClientType = ClientType.ServerSide,
                RedirectUris = new List<ClientRedirectUri>()
                {
                    new() { Uri = new Uri("https://example.com") }
                }
            });
        var passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);
        var clientValidator = new ClientValidator(
            logger.Object,
            clientReader.Object,
            passwordHasher.Object);

        var request = new ValidateClientRequest
        {
            ClientId = Guid.NewGuid(),
            ClientSecret = "client_secret",
            RedirectUri = new Uri("https://example.com/invalid")
        };

        // Act
        var result = await clientValidator.ValidateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Error.Should().Be(OAuthErrorTypes.InvalidClient);
        result.ErrorDescription.Should().Be("Invalid client credentials.");
    }

    [Fact]
    public async Task ValidateAsync_WhenUrlIsInvalid_ReturnsError()
    {
        // Arrange
        var logger = new Mock<ILogger<ClientValidator>>();
        var clientReader = new Mock<IClientReader>();
        clientReader.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new Client()
            {
                ClientId = Guid.NewGuid(),
                ClientSecretHash = "client_secret_hash",
                ClientType = ClientType.ServerSide,
                RedirectUris = new List<ClientRedirectUri>()
                {
                    new() { Uri = new Uri("https://example.com") }
                }
            });
        var passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        var clientValidator = new ClientValidator(
            logger.Object,
            clientReader.Object,
            passwordHasher.Object);

        var request = new ValidateClientRequest
        {
            ClientId = Guid.NewGuid(),
            ClientSecret = "client_secret",
            RedirectUri = new Uri("https://example.com/invalid")
        };

        // Act
        var result = await clientValidator.ValidateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Error.Should().Be(OAuthErrorTypes.InvalidRequest);
        result.ErrorDescription.Should().Be("Invalid redirect_uri parameter.");
    }
    
}