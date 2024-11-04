using System.Timers;
using Spectre.Console;
using Timer = System.Timers.Timer;

namespace ClickBoxin;

public class Game
{
        static public bool esc = true;
        static public bool selected = false;
        static public string data = "";
        static public int time = 0;
        
        static public Player player = new Player(0, 1, 1);
        
        static public int farm1 = 1;
        static public int farm2 = 1;

        static public void UpgradeLogic(int variable)
        {
            AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
            switch (variable)
            {
                case 1:
                    AnsiConsole.MarkupLine($"This will cost {(player.Dmg * 100) * player.Dmg} score!");
                    break;
                case 2:
                    AnsiConsole.MarkupLine($"This will cost {1000 * farm1} score!");
                    break;
                case 3:
                    AnsiConsole.MarkupLine($"This will cost {(1000 * (farm2))/2} score!");
                    break;
            }
            AnsiConsole.MarkupLine("[green]Press /spacebar/ to confirm[/] or [red]any other key to cancel[/]");

            var key = Console.ReadKey(false).Key;
            if (key == ConsoleKey.Spacebar)
            {
                switch (variable)
                {
                    case 1:
                        if (player.Score >= (player.Dmg * 100) * player.Dmg)
                        {
                            player.Score -= (player.Dmg * 100) * player.Dmg;
                            player.Dmg++;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case 2:
                        if (player.Score >= 1000 * farm1)
                        {
                            player.Score -= 1000 * farm1;
                            farm1 += 3;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        if (player.Score >= 1000 * ((1000 * (farm2))/2))
                        {
                            player.Score -= 1000 * ((1000 * (farm2))/2);
                            farm2 += 20;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                }
            }
        }

        static public void StartTimer()
        {
            var bonusTimer = new Timer(1000); // 1 second
            bonusTimer.Elapsed += OnTimerElapsed;
            bonusTimer.AutoReset = true;
            bonusTimer.Enabled = true;
        }

        static public void OnTimerElapsed(Object source, ElapsedEventArgs e)
        {
            time += 1; // Keep track of passed seconds one by one
            if (selected == false)
            {
                Window.UpdateTable();
                Window.UpdateStats();
                AnsiConsole.Clear();
                AnsiConsole.Write(Window.GameWin);
            }
            if (time % 10 == 0)
            {
                player.Score += farm1;
            }

            if (time % 30 == 0)
            {
                player.Score += farm2;
            }

            if (time == 120) time = 0;
        }

        static public void SaveGame()
        {
            data = "Score: " + player.Score.ToString()
                             + "\nStage: " + player.Stage.ToString()
                             + "\nDmg: " + player.Dmg.ToString()
                             + "\nFarm1: " + farm1.ToString()
                             + "\nFarm2: " + farm2.ToString();
            File.WriteAllText("save.txt", data);
        }
        static public void LoadGame()
        {
            if (File.Exists("save.txt"))
            {
                Game.data = File.ReadAllText("save.txt");
                string[] data = Game.data.Split("\n");
                player.Score = int.Parse(data[0].Split(":")[1]);
                player.Stage = int.Parse(data[1].Split(":")[1]);
                player.Dmg = int.Parse(data[2].Split(":")[1]);
                Game.farm1 = int.Parse(data[3].Split(":")[1]);
                Game.farm2 = int.Parse(data[4].Split(":")[1]);
            }
        }
}