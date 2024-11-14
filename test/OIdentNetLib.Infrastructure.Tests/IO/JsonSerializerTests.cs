using FluentAssertions;
using OIdentNetLib.Infrastructure.IO;

namespace OIdentNetLib.Infrastructure.Tests.Encryption;

[Trait("Category", "Unit")]
public class JsonSerializerTests
{
    [Fact]
    public async Task Serialize_WhenObjectIsNull_ThenReturnEmptyString()
    {
        // Arrange
        var serializer = new JsonSerializer();

        // Act
        var result = await serializer.SerializeAsync(null);

        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Serialize_WhenObjectIsNotNull_ThenReturnJsonString()
    {
        // Arrange
        var serializer = new JsonSerializer();
        var obj = new { Name = "Test" };

        // Act
        var result = await serializer.SerializeAsync(obj);

        // Assert
        result.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Serialize_Deserialize_WhenObjectIsNotNull_ThenReturnObject()
    {
        // Arrange
        var serializer = new JsonSerializer();
        var deserializer = new JsonDeserializer();
        var obj = new { Name = "Test" };

        // Act
        var json = await serializer.SerializeAsync(obj);
        var result = await deserializer.DeserializeAsync<object>(json);

        // Assert
        result.Should().NotBeNull();
    }
}