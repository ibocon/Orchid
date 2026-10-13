using System.Security.Cryptography;
using FluentAssertions;
using Orchid.Core.Cryptography;
using Xunit;

namespace Orchid.UnitTests.Core;

public sealed class AesGcmCryptoServiceTests
{
    [Fact]
    public void Encrypt_And_Decrypt_Should_Restore_Original_PlainText()
    {
        // Given
        var service = new AesGcmCryptoService();
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var plainText = "Sensitive Clipboard Data"u8.ToArray();

        // When
        var encryptResult = service.Encrypt(plainText, key);

        // Then
        encryptResult.IsSuccess.Should().BeTrue();
        encryptResult.Value.CipherText.Should().NotBeEquivalentTo(plainText);

        var decryptResult = service.Decrypt(
            encryptResult.Value.CipherText,
            encryptResult.Value.Nonce,
            encryptResult.Value.Tag,
            key);

        decryptResult.IsSuccess.Should().BeTrue();
        decryptResult.Value.Should().BeEquivalentTo(plainText);
    }

    [Fact]
    public void Decrypt_With_Invalid_Key_Should_Fail()
    {
        // Given
        var service = new AesGcmCryptoService();
        var validKey = new byte[32];
        var invalidKey = new byte[32];
        RandomNumberGenerator.Fill(validKey);
        RandomNumberGenerator.Fill(invalidKey);
        var plainText = "Confidential Information"u8.ToArray();

        var encryptResult = service.Encrypt(plainText, validKey);
        encryptResult.IsSuccess.Should().BeTrue();

        // When
        var decryptResult = service.Decrypt(
            encryptResult.Value.CipherText,
            encryptResult.Value.Nonce,
            encryptResult.Value.Tag,
            invalidKey);

        // Then
        decryptResult.IsFailure.Should().BeTrue();
    }
}
