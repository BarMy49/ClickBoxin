using System;
using System.Media;
using Spectre.Console;
using SixLabors.ImageSharp;
using Spectre.Console.Rendering;

namespace ClickBoxin;

class Program
{
    static void Main(string[] args)
    {
        //Here are the variables and other data
        int score = 0;
        var image = new CanvasImage("assets/hum.png");
        image.MaxWidth(15);
        var music = new SoundPlayer("assets/musci.wav");
        music.PlayLooping();
        
        bool esc = true;
        bool selected = true;
        
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
            GameWin.AddRow(image, new Markup("[green]Press /Spacebar/ to attack! \nPress /U/ to upgrade! \nPress /Backspace/ to exit![/]"));
            GameWin.AddRow(new Markup($"score: {score}"));
        }
        
        //This function will open the upgrade menu
        void UpgradeMenu()
        {
            selected = true;
            while (selected)
            {
                
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]UPGRADE MENU SCORE:{score}[/]");
                AnsiConsole.MarkupLine($"[blue]SCORE:{score}[/]");
                var upgrade = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(new[] { "Exit", "Upgrade 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Upgrade 5" }));

                switch (upgrade)
                {
                    case "Exit":
                        selected = false;
                        break;
                    case "Upgrade 1":
                        if (score >= 10)
                        {
                            score -= 10;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case "Upgrade 2":
                        if (score >= 10)
                        {
                            score -= 10;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case "Upgrade 3":
                        if (score >= 10)
                        {
                            score -= 10;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case "Upgrade 4":
                        if (score >= 10)
                        {
                            score -= 10;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                    case "Upgrade 5":
                        if (score >= 10)
                        {
                            score -= 10;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }
                        break;
                }
            }
            UpdateTable();
        }

        UpdateTable();
        
        //Here is the main loop
        while (esc)
        {
            AnsiConsole.Write(GameWin);
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Backspace:
                    esc = false;
                    AnsiConsole.MarkupLine("[bold red]Game Over! Thanks for playing![/]");
                    break;
                case ConsoleKey.Spacebar:
                    score++;
                    UpdateTable();
                    break;
                case ConsoleKey.U:
                    UpgradeMenu();
                    break;
            }
            AnsiConsole.Clear();
        }
    }
}