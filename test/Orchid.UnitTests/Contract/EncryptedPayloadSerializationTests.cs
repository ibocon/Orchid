using System.Text.Json;
using FluentAssertions;
using Orchid.Contract.Payload;
using Xunit;

namespace Orchid.UnitTests.Contract;

public sealed class EncryptedPayloadSerializationTests
{
    [Fact]
    public void EncryptedPayload_Should_Serialize_And_Deserialize_Deterministically()
    {
        // Given
        var original = new EncryptedPayload(
            Guid.NewGuid(),
            [1, 2, 3, 4],
            [5, 6, 7],
            [8, 9],
            "AES-256-GCM",
            DateTimeOffset.UtcNow);

        // When
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<EncryptedPayload>(json);

        // Then
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(original);
    }
}
