using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

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
}