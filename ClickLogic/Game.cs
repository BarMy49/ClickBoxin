using System.Timers;
using Spectre.Console;
using Timer = System.Timers.Timer;

namespace ClickBoxin;

public class Game
{
        static public Player player = new Player();    
    
        static public bool esc = true;
        static public int WindowOpened = 0; //0 - Main, 1 - Upgrade Menu, 2 - Boss Window, 3 - Ultra Upgrade Menu
        static public string data = "";
        static public int time = 0;
        static public int bosstime = 30;

        static public Boss boss;
        
        static public List<Farm> farms = new List<Farm>
        {
            new Farm(0, 60, 500, 2),
            new Farm(0, 50, 1000, 4),
            new Farm(0, 40, 5000, 8),
            new Farm(0, 30, 10000, 16),
            new Farm(0, 20, 20000, 32),
            new Farm(0, 10, 100000,64)
        };
        static public void UpgradeLogic(int variable)
        {
            if (variable < 0 || variable >= farms.Count)
            {
                AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                AnsiConsole.MarkupLine($"This will cost {(player.Dmg * 100) * player.Dmg} score!");
                AnsiConsole.MarkupLine("[green]Press /spacebar/ to confirm[/] or [red]any other key to cancel[/]");

                var key = Console.ReadKey(false).Key;
                if (key == ConsoleKey.Spacebar)
                {
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
                }
            }else
            {
                var farm = farms[variable];
                AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                AnsiConsole.MarkupLine($"This will cost {farm.Cost} score!");
                AnsiConsole.MarkupLine("[green]Press /spacebar/ to confirm[/] or [red]any other key to cancel[/]");

                var key = Console.ReadKey(false).Key;
                if (key == ConsoleKey.Spacebar)
                {
                    if (player.Score >= farm.Cost)
                    {
                        player.Score -= farm.Cost;
                        farm.ScoreIncrement += 10; // Increase the score increment
                        farm.Cost += (farm.Cost/2); // Increase the cost by the half of the current cost
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Not enough score![/]");
                        Console.ReadKey();
                    }
                }
            }
        }

        static public void StartTimer()
        {
            var timer = new Timer(1000); // 1 second
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        static public void OnTimerElapsed(Object source, ElapsedEventArgs e)
        {
            time += 1; // Keep track of passed seconds one by one
            switch (WindowOpened)
            {
                case 0:
                    if (esc == true)
                    {
                        Window.UpdateTable();
                        Window.UpdateStats();
                        AnsiConsole.Clear();
                        AnsiConsole.Write(Window.GameWin);
                    }
                    break;
                case 2:
                {
                    Window.BossTable();
                    AnsiConsole.Clear();
                    AnsiConsole.Write(Window.BossWin);
                    boss.Time--;
                    break;
                }
            }
            foreach (var farm in farms)
            {
                if (time % farm.TimeInterval == 0)
                {
                    player.Score += farm.ScoreIncrement;
                }
            }

            if (time == 120) time = 0;
        }

        static public void CreateBoss(int Stage, int BossTime)
        {
             boss = new Boss(Stage,BossTime);
        }

        static public void BossWon()
        {
            player.Score += boss.Score;
            player.Stage++;
            WindowOpened = 0;
            CreateBoss(player.Stage, bosstime);
        }

        static public void SaveGame()
        {
            data = "Score: " + player.Score.ToString()
                             + "\nStage: " + player.Stage.ToString()
                             + "\nDmg: " + player.Dmg.ToString();
            for (int i = 0; i < farms.Count; i++)
            {
                data += $"\nFarm{i + 1}ScoreIncrement: {farms[i].ScoreIncrement}";
                data += $"\nFarm{i + 1}TimeInterval: {farms[i].TimeInterval}";
                data += $"\nFarm{i + 1}Cost: {farms[i].Cost}";
            }
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
                for (int i = 0; i < farms.Count; i++)
                {
                    farms[i].ScoreIncrement = int.Parse(data[3 + i * 3].Split(":")[1]);
                    farms[i].TimeInterval = int.Parse(data[4 + i * 3].Split(":")[1]);
                    farms[i].Cost = int.Parse(data[5 + i * 3].Split(":")[1]);
                }
            }
        }

        static public void UltraRestart()
        {
            player.Ultra += (player.Score/10000) * player.Stage * player.Dmg;
            Window.UltraRestartLoad();
            player.Dmg = 1;
            player.Score = 0;
            player.Stage = 1;
            for (int i = 0; i < farms.Count; i++)
            {
                farms[i].ScoreIncrement = 0;
                farms[i].Cost = farms[i].InitialCost;
            }
        }
        static public void UltraUpgradeLogic(int variable)
        {
            if (variable < 0 || variable >= farms.Count)
            {
                AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                AnsiConsole.MarkupLine($"This will cost {player.DmgMulti * player.DmgMulti} ultra!");
                AnsiConsole.MarkupLine("[green]Press /spacebar/ to confirm[/] or [red]any other key to cancel[/]");

                var key = Console.ReadKey(false).Key;
                if (key == ConsoleKey.Spacebar)
                {
                    if (player.Ultra >= player.DmgMulti * player.DmgMulti)
                    {
                        player.Ultra -= player.DmgMulti * player.DmgMulti;
                        player.DmgMulti++;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Not enough ultra![/]");
                        Console.ReadKey();
                    }
                }
            }else if (variable == 0)
            {
                AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                AnsiConsole.MarkupLine($"This will cost {bosstime/2} ultra!");
                AnsiConsole.MarkupLine("[green]Press /spacebar/ to confirm[/] or [red]any other key to cancel[/]");

                var key = Console.ReadKey(false).Key;
                if (key == ConsoleKey.Spacebar)
                {
                    if(player.Ultra >= bosstime/2)
                    {
                        player.Ultra -= bosstime/2;
                        bosstime += 1;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Not enough ultra![/]");
                        Console.ReadKey();
                    }
                }
            }else
            {
                var farm = farms[variable];
                AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                AnsiConsole.MarkupLine($"This will cost {farm.TimeCost} ultra!");
                AnsiConsole.MarkupLine("[green]Press /spacebar/ to confirm[/] or [red]any other key to cancel[/]");

                var key = Console.ReadKey(false).Key;
                if (key == ConsoleKey.Spacebar)
                {
                    if (player.Ultra >= farm.TimeCost)
                    {
                        player.Ultra -= farm.Cost;
                        farm.TimeInterval -= 1; // Decrease the time interval
                        farm.TimeCost += (farm.TimeCost/2); // Increase the cost by the half of the current cost
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Not enough ultra![/]");
                        Console.ReadKey();
                    }
                }
            }
        }
}