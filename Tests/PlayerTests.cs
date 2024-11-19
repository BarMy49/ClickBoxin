using Xunit;

namespace ClickBoxin.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_ShouldUpdateScore()
        {
            // Arrange
            var player = new Player { Score = 0 };

            // Act
            player.Score += 100;

            // Assert
            Assert.Equal(100, player.Score);
        }

        [Fact]
        public void Player_ShouldUpdateTickets()
        {
            // Arrange
            var player = new Player { Tickets = 0 };

            // Act
            player.Tickets += 10;

            // Assert
            Assert.Equal(10, player.Tickets);
        }
    }
}