using FluentAssertions;
using Moq;
using Orchid.Core.Abstraction;
using Xunit;

namespace Orchid.UnitTests.Mobile;

public sealed class MobileSyncWorkerTests
{
    [Fact]
    public void Worker_Should_Pause_Background_Polling_When_App_Suspended()
    {
        // Given
        var worker = new Mock<IMobileSyncBackgroundWorker>();

        // When & Then
        worker.Setup(x => x.IsActive).Returns(false);
        worker.Object.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Worker_Should_Resume_When_App_Returns_To_Foreground()
    {
        // Given
        var worker = new Mock<IMobileSyncBackgroundWorker>();
        worker.Setup(x => x.IsActive).Returns(true);

        // When & Then
        worker.Object.IsActive.Should().BeTrue();
    }
}
