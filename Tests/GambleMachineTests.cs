using Xunit;
namespace ClickBoxin.Tests;

public class GambleMachineTests
{
    [Fact]
    public void SpinWheel_ShouldModifyPlayerStats()
    {
        // Arrange
        var player = new Player();
        Game.player = player;

        // Act
        GamblingMachine.SpinWheelBasic();

        // Assert
        Assert.True(player.Score != 0 || player.Tickets != 0 || player.Ultra != 0 || player.Stage != 0);
    }
}