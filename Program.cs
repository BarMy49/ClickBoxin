using System;
using System.IO;
using System.Media;
using Spectre.Console;
using System.Timers;
using SixLabors.ImageSharp;
using Spectre.Console.Rendering;
using Timer = System.Timers.Timer;

namespace ClickBoxin;

class Program
{
    static void Main(string[] args)
    {
        //Here are the variables and other data
        int score = 0;

        var image = new CanvasImage("assets/hum.png");
        image.MaxWidth(100);
        var music = new SoundPlayer("assets/musci.wav");
        music.PlayLooping();

        bool esc = true;
        bool selected = true;
        string data = "";
        
        int dmg = 1;
        bool farm1 = false;

        //Here is the main table
        var GameWin = new Table();

        GameWin.Alignment(Justify.Center);
        GameWin.Width(100);
        GameWin.Border(TableBorder.Rounded);
        GameWin.AddColumn(new TableColumn("[yellow]TRAIN[/]").Centered());
        GameWin.AddColumn(new TableColumn("[blue]MENU[/]").Centered());

        //After a click the window will refresh thanks to this function
        void UpdateTable()
        {
            GameWin.Rows.Clear();
            GameWin.AddRow(image, new Markup("[green]Press /Spacebar/ to attack! \nPress /U/ to upgrade! \nPress /S/ to save! \nPress /L/ to load! \nPress /Backspace/ to exit![/]"));
            GameWin.AddRow(new Markup($"score: {score}"));
        }

        //This function will open the upgrade menu
        void UpgradeMenu()
        {
            selected = true;
            while (selected)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]UPGRADE MENU[/]");
                AnsiConsole.MarkupLine($"[blue]SCORE: {score}[/]\t[red]DMG: {dmg}[/]");
                ;
                var upgrade = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(new[] { "Exit", "MORE DMG", "UPGRADE SCORE FARM 1", "Upgrade 3" }));

                switch (upgrade)
                {
                    case "Exit":
                        selected = false;
                        break;
                    case "MORE DMG":
                        AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                        AnsiConsole.MarkupLine($"This will cost {(dmg * 100)*dmg} score!");
                        AnsiConsole.MarkupLine("[green]Prees /spacebar/ to confirm[/] or [red]any other key to cancel[/]");
                        var key = Console.ReadKey(false).Key;
                        if (key == ConsoleKey.Spacebar)
                        {
                            if (score >= (dmg * 100)*dmg)
                            {
                                score -= (dmg * 100)*dmg;
                                dmg++;
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]Not enough score![/]");
                                Console.ReadKey();
                            }
                        }

                        break;
                    case "UPGRADE SCORE FARM 1":
                        AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                        AnsiConsole.MarkupLine($"This will cost 1000 score!");
                        AnsiConsole.MarkupLine("[green]Prees /spacebar/ to confirm[/] or [red]any other key to cancel[/]");
                        var key2 = Console.ReadKey(false).Key;
                        if (key2 == ConsoleKey.Spacebar)
                        {
                            if (score >= 1000)
                            {
                                score -= (1000);
                                farm1 = true;
                                StartBonusTimer();
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]Not enough score![/]");
                                Console.ReadKey();
                            }
                        }

                        break;
                    case "Upgrade 3":
                        AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                        AnsiConsole.MarkupLine($"This will cost 10 score!");
                        AnsiConsole.MarkupLine("[green]Prees /spacebar/ to confirm[/] or [red]any other key to cancel[/]");
                        var key3 = Console.ReadKey(false).Key;
                        if (key3 == ConsoleKey.Spacebar)
                        {
                            if (score >= 10)
                            {
                                score -= 10;
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]Not enough score![/]");
                                Console.ReadKey();
                            }
                        }

                        break;
                }

                UpdateTable();
            }
        }
        
        void StartBonusTimer()
        {
            var bonusTimer = new Timer(5000); // 5 seconds
            bonusTimer.Elapsed += OnBonusTimerElapsed;
            bonusTimer.AutoReset = true;
            bonusTimer.Enabled = true;
        }

        void OnBonusTimerElapsed(Object source, ElapsedEventArgs e)
        {
            score += 5; // Increment score by 5 every 5 seconds
            UpdateTable();
        }

        UpdateTable();

            //Here is the main loop
            while (esc)
            {
                AnsiConsole.Write(GameWin);
                var key = Console.ReadKey(false).Key;
                switch (key)
                {
                    case ConsoleKey.Backspace:
                        esc = false;
                        AnsiConsole.MarkupLine("[bold red]Game Over! Thanks for playing![/]");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.Spacebar:
                        score += dmg;
                        UpdateTable();
                        break;
                    case ConsoleKey.U:
                        UpgradeMenu();
                        break;
                    case ConsoleKey.S:
                        AnsiConsole.MarkupLine("[bold]Saving...[/]");
                        data = "Score: " + score.ToString() + "\nDmg: " + dmg.ToString() + "\nFarm1: " + farm1.ToString();
                        File.WriteAllText("save.txt", data);
                        break;
                    case ConsoleKey.L:
                        AnsiConsole.MarkupLine("[bold]Loading...[/]");
                        break;
                }

                AnsiConsole.Clear();
            }
    }
}