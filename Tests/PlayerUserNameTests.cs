using Xunit;

namespace ClickBoxin.Tests
{
    public class PlayerUsernameTests
    {
        [Fact]
        public void Username_ShouldBeAccepted_WhenValid()
        {
            // Arrange
            var player = new Player();

            // Act
            player.Name = "ValidName";

            // Assert
            Assert.Equal("ValidName", player.Name);
        }

        [Fact]
        public void Username_ShouldBeRejected_WhenTooLong()
        {
            // Arrange
            var player = new Player();

            // Act
            player.Name = "ThisNameIsWayTooLong";

            // Assert
            Assert.NotEqual("ThisNameIsWayTooLong", player.Name);
        }

        [Fact]
        public void Username_ShouldBeRejected_WhenContainsInvalidCharacters()
        {
            // Arrange
            var player = new Player();

            // Act
            player.Name = "Invalid/Name";

            // Assert
            Assert.NotEqual("Invalid/Name", player.Name);
        }

        [Fact]
        public void Username_ShouldBeRejected_WhenEmpty()
        {
            // Arrange
            var player = new Player();

            // Act
            player.Name = "";

            // Assert
            Assert.NotEqual("", player.Name);
        }
    }
}
