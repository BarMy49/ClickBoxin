using Xunit;

namespace ClickBoxin.Tests
{
    public class WindowTests
    {
        [Fact]
        public void GetAssets_ShouldLoadAssets()
        {
            // Act
            Window.GetAssets();

            // Assert
            Assert.NotNull(Window.image);
            Assert.NotNull(Window.bossi);
            Assert.NotNull(Window.music);
            Assert.NotNull(Window.introm);
            Assert.NotNull(Window.bossm);
            Assert.NotNull(Window.outrom);
        }

        [Fact]
        public void CreateTable_ShouldInitializeTables()
        {
            // Act
            Window.CreateTable();

            // Assert
            Assert.NotNull(Window.GameWin);
            Assert.NotNull(Window.DailyBonus);
        }

        [Fact]
        public void UpdateStats_ShouldUpdateStatsString()
        {
            // Arrange
            var player = new Player { Name = "TestPlayer", Stage = 1, Dmg = 10, DmgMulti = 2 };
            Game.player = player;

            // Act
            Window.UpdateStats();

            // Assert
            Assert.Contains("TestPlayer", Window.stats);
            Assert.Contains("STAGE: 1", Window.stats);
            Assert.Contains("DMG: 10 X2", Window.stats);
        }
    }
}