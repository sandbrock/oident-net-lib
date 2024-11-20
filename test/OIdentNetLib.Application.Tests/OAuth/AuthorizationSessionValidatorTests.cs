using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OIdentNetLib.Application.OAuth;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.Tests.OAuth;

[Trait("Category", "Unit")]
public class AuthorizationSessionValidatorTests
{
    [Fact]
    public async Task ValidateAsync_WhenSessionDoesNotExist_ReturnsError()
    {
        // Arrange
        var locker = new Mock<ILogger<AuthorizationSessionValidator>>();
        var authorizationSessionReader = new Mock<IAuthorizationSessionReader>();
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationSessionValidator = new AuthorizationSessionValidator(
            locker.Object,
            authorizationSessionReader.Object,
            authorizationSessionWriter.Object);
        var validateSessionRequest = new ValidateSessionRequest()
        {
            AuthorizationCode = "authorization-code"
        };

        // Act
        var response = await authorizationSessionValidator.ValidateAsync(
            validateSessionRequest);
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.OIdentError.Should().Be(OIdentErrors.InvalidAuthorizationCode);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenSessionIsExpired_ReturnsError()
    {
        // Arrange
        var locker = new Mock<ILogger<AuthorizationSessionValidator>>();
        var authorizationSessionReader = new Mock<IAuthorizationSessionReader>();
        authorizationSessionReader.Setup(r => r.ReadByAuthCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new AuthorizationSession()
            {
                AuthorizationSessionId = Guid.NewGuid(),
                ResponseType = "code",
                AuthorizationCode = "authorization-code",
                SessionExpiresAt = DateTime.UtcNow.AddDays(-1)
            });
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationSessionValidator = new AuthorizationSessionValidator(
            locker.Object,
            authorizationSessionReader.Object,
            authorizationSessionWriter.Object);
        var validateSessionRequest = new ValidateSessionRequest()
        {
            AuthorizationCode = "authorization-code"
        };

        // Act
        var response = await authorizationSessionValidator.ValidateAsync(
            validateSessionRequest);
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.OIdentError.Should().Be(OIdentErrors.ExpiredAuthorizationCode);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenUserDoesNotExist_ReturnsError()
    {
        // Arrange
        var locker = new Mock<ILogger<AuthorizationSessionValidator>>();
        var authorizationSessionReader = new Mock<IAuthorizationSessionReader>();
        authorizationSessionReader.Setup(r => r.ReadByAuthCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new AuthorizationSession()
            {
                AuthorizationSessionId = Guid.NewGuid(),
                ResponseType = "code",
                AuthorizationCode = "authorization-code",
                SessionExpiresAt = DateTime.UtcNow.AddDays(1)
            });
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationSessionValidator = new AuthorizationSessionValidator(
            locker.Object,
            authorizationSessionReader.Object,
            authorizationSessionWriter.Object);
        var validateSessionRequest = new ValidateSessionRequest()
        {
            AuthorizationCode = "authorization-code"
        };

        // Act
        var response = await authorizationSessionValidator.ValidateAsync(
            validateSessionRequest);
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.OIdentError.Should().Be(OIdentErrors.InvalidAuthorizationCode);
    }    

    [Fact]
    public async Task ValidateAsync_WhatValid_ReturnsSuccess()
    {
        // Arrange
        var locker = new Mock<ILogger<AuthorizationSessionValidator>>();
        var authorizationSessionReader = new Mock<IAuthorizationSessionReader>();
        authorizationSessionReader.Setup(r => r.ReadByAuthCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new AuthorizationSession()
            {
                AuthorizationSessionId = Guid.NewGuid(),
                ResponseType = "code",
                AuthorizationCode = "authorization-code",
                SessionExpiresAt = DateTime.UtcNow.AddDays(1),
                User = new User()
                {
                    UserId = Guid.NewGuid(),
                    Email = "user@example.com"
                }
            });
        var authorizationSessionWriter = new Mock<IAuthorizationSessionWriter>();
        var authorizationSessionValidator = new AuthorizationSessionValidator(
            locker.Object,
            authorizationSessionReader.Object,
            authorizationSessionWriter.Object);
        var validateSessionRequest = new ValidateSessionRequest()
        {
            AuthorizationCode = "authorization-code"
        };

        // Act
        var response = await authorizationSessionValidator.ValidateAsync(
            validateSessionRequest);
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
    }    
}