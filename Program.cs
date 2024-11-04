using System;
using System.IO;
using System.Media;
using System.Timers;
using Spectre.Console;
using SixLabors.ImageSharp;
using Spectre.Console.Rendering;
using Timer = System.Timers.Timer;

namespace ClickBoxin
{
    class Game
    {
        static public bool esc = true;
        static public bool selected = false;
        static public string data = "";
        static public int time = 0;

        static public int score = 0;
        static public int stage = 1;
        static public int dmg = 1;
        static public int farm1 = 1;
        static public int farm2 = 1;

        static public void UpgradeLogic(int variable)
        {
            AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
            switch (variable)
            {
                case 1:
                    AnsiConsole.MarkupLine($"This will cost {(dmg * 100) * dmg} score!");
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
                        if (score >= (dmg * 100) * dmg)
                        {
                            score -= (dmg * 100) * dmg;
                            dmg++;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case 2:
                        if (score >= 1000 * farm1)
                        {
                            score -= 1000 * farm1;
                            farm1 += 2;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        if (score >= 1000 * ((1000 * (farm2))/2))
                        {
                            score -= 1000 * ((1000 * (farm2))/2);
                            farm2 += 10;
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
                score += farm1;
            }

            if (time % 60 == 0)
            {
                score += farm2;
            }

            if (time == 120) time = 0;
        }

        static public void SaveGame()
        {
            data = "Score: " + score.ToString()
                             + "\nStage: " + stage.ToString()
                             + "\nDmg: " + dmg.ToString()
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
                Game.score = int.Parse(data[0].Split(":")[1]);
                Game.stage = int.Parse(data[1].Split(":")[1]);
                Game.dmg = int.Parse(data[2].Split(":")[1]);
                Game.farm1 = int.Parse(data[3].Split(":")[1]);
                Game.farm2 = int.Parse(data[4].Split(":")[1]);
            }
        }
    }
    class Window
    {
        static public CanvasImage image;
        static public SoundPlayer music;
        static public Table GameWin;

        static public string stats = "";

        static public void UpdateStats()
        {
            stats = $"STAGE: {Game.stage}"
                    + $"\n[orange3]DMG: {Game.dmg} [/]";
            if (Game.farm1 >= 2)
            {
                stats += $"\n+ {Game.farm1} score every 10 seconds \n{GenerateProgressBar((Game.time%10), 10)}";
            }

            if (Game.farm2 >= 10)
            {
                stats += $"\n+ {Game.farm2} score every 60 seconds \n{GenerateProgressBar((Game.time%60), 60)}";
            }
        }

        static public string GenerateProgressBar(int value, int interval)
        {
            int progress = Math.Min((value * 10) / interval, 10);
            return progress switch
            {
                0 => "□□□□□□□□□□",
                1 => "■□□□□□□□□□",
                2 => "■■□□□□□□□□",
                3 => "■■■□□□□□□□",
                4 => "■■■■□□□□□□",
                5 => "■■■■■□□□□□",
                6 => "■■■■■■□□□□",
                7 => "■■■■■■■□□□",
                8 => "■■■■■■■■□□",
                9 => "■■■■■■■■■□",
                _ => "□□□□□□□□□□"
            };
        }

        static public void UpdateTable()
        {
            GameWin.Rows.Clear();
            GameWin.AddRow(new Markup($"{stats}"), image, new Markup("[green]Press /Spacebar/ to attack! \nPress /U/ to upgrade! \nPress /S/ to save! \nPress /L/ to load! \nPress /Backspace/ to exit![/]"));
            GameWin.AddRow(new Markup(" "), new Markup($"score: {Game.score}"));
        }

        static public void UpgradeMenu()
        {
            Game.selected = true;
            while (Game.selected == true)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]UPGRADE MENU[/]");
                AnsiConsole.MarkupLine($"[blue]SCORE: {Game.score}[/]\t[red]DMG: {Game.dmg}[/]");
                var upgrade = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(new[] { "Exit", "DMG", "SCORE FARM CO 10 SEKUND", "SCORE FARM CO MINUTE" }));

                switch (upgrade)
                {
                    case "Exit":
                        Game.selected = false;
                        break;
                    case "DMG":
                        Game.UpgradeLogic(1);
                        break;
                    case "SCORE FARM CO 10 SEKUND":
                        Game.UpgradeLogic(2);
                        break;
                    case "SCORE FARM CO MINUTE":
                        Game.UpgradeLogic(3);
                        break;
                }
                UpdateStats();
                UpdateTable();
            }
        }

        static void Main(string[] args)
        {
            image = new CanvasImage("assets/hum.png");
            image.MaxWidth(100);
            music = new SoundPlayer("assets/musci.wav");
            music.PlayLooping();

            GameWin = new Table();
            GameWin.Alignment(Justify.Center);
            GameWin.Width(100);
            GameWin.Border(TableBorder.Rounded);
            GameWin.AddColumn(new TableColumn("[red]STATS[/]").Centered());
            GameWin.AddColumn(new TableColumn("[yellow]TRAIN[/]").Centered());
            GameWin.AddColumn(new TableColumn("[blue]MENU[/]").Centered());

            UpdateTable();
            Game.StartTimer();
            AnsiConsole.Write(GameWin);

            while (Game.esc)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false).Key;
                    switch (key)
                    {
                        case ConsoleKey.Backspace:
                            Game.esc = false;
                            AnsiConsole.MarkupLine("[bold red]Game Over! Thanks for playing![/]");
                            Console.ReadKey();
                            break;
                        case ConsoleKey.Spacebar:
                            Game.score += Game.dmg;
                            break;
                        case ConsoleKey.U:
                            UpgradeMenu();
                            break;
                        case ConsoleKey.S:
                            AnsiConsole.MarkupLine("[bold]Saving...[/]");
                            Game.SaveGame();
                            break;
                        case ConsoleKey.L:
                            AnsiConsole.MarkupLine("[bold]Loading...[/]");
                            Game.LoadGame();
                            break;
                    }
                    UpdateTable();
                    UpdateStats();
                    AnsiConsole.Clear();
                    AnsiConsole.Write(GameWin);
                }
            }
        }
    }
}