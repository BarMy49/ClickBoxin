using System.Media;
using Spectre.Console;

namespace ClickBoxin
{
    class Window
    {
        static public CanvasImage image;
        static public SoundPlayer music;
        static public Table GameWin;

        static public string stats = "";

        static public void UpdateStats()
        {
            stats = $"STAGE: {Game.player.Stage}"
                    + $"\n[orange3]DMG: {Game.player.Dmg} [/]";
            if (Game.farm1 >= 2)
            {
                stats += $"\n+ {Game.farm1} score every 10 seconds \n{GenerateProgressBar((Game.time%10), 10)}";
            }

            if (Game.farm2 >= 10)
            {
                stats += $"\n+ {Game.farm2} score every 60 seconds \n{GenerateProgressBar((Game.time%30), 30)}";
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
            GameWin.AddRow(new Markup(" "), new Markup($"score: {Game.player.Score}"));
        }

        static public void UpgradeMenu()
        {
            Game.selected = true;
            while (Game.selected == true)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]UPGRADE MENU[/]");
                AnsiConsole.MarkupLine($"[blue]SCORE: {Game.player.Score}[/]\t[red]DMG: {Game.player.Dmg}[/]");
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
            image.MaxWidth(15);
            music = new SoundPlayer("assets/musci.wav");

            GameWin = new Table();
            GameWin.Alignment(Justify.Center);
            GameWin.Width(100);
            GameWin.Border(TableBorder.Rounded);
            GameWin.AddColumn(new TableColumn("[orange3]STATS[/]").Centered());
            GameWin.AddColumn(new TableColumn("[yellow]TRAIN[/]").Centered());
            GameWin.AddColumn(new TableColumn("[blue]MENU[/]").Centered());
            GameWin.Columns[0].Width(30);

            UpdateTable();
            Game.StartTimer();
            AnsiConsole.Write(GameWin);
            music.PlayLooping();

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
                            Game.player.Score += Game.player.Dmg;
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