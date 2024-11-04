using System.Media;
using Spectre.Console;

namespace ClickBoxin
{
    class Window
    {
        static public CanvasImage image;
        static public CanvasImage bossi;
        static public SoundPlayer music;
        static public SoundPlayer introm;
        static public SoundPlayer outrom;
        static public SoundPlayer err;
        static public SoundPlayer bossm;
        static public Table GameWin;

        static public string stats = "";

        static public void UpdateStats()
        {
            stats = $"STAGE: {Game.player.Stage}" +
                    $"\n[orange3]DMG: {Game.player.Dmg}[/]";
            for (int i = 0; i < Game.farms.Count; i++)
            {
                var farm = Game.farms[i];
                if(farm.ScoreIncrement > 0) stats += $"\n+ {farm.ScoreIncrement} score every {farm.TimeInterval} seconds \n{GenerateProgressBar(Game.time % farm.TimeInterval, farm.TimeInterval)}";
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
            GameWin.AddRow(new Markup($"{stats}"), image
                , new Markup("[green]Press /Spacebar/ to attack! " +
                                                                        "\nPress /U/ to upgrade! " +
                                                                        "\nPress /S/ to save! " +
                                                                        "\nPress /L/ to load! " +
                                                                        "\nPress /M/ to mute! " +
                                                                        "\nPress /Backspace/ to exit![/]"));
            GameWin.AddRow(new Markup(" "), new Markup($"score: {Game.player.Score}"));
        }

        static public void UpgradeMenu()
        {
            Game.openwin = true;
            while (Game.openwin == true)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]UPGRADE MENU[/]");
                AnsiConsole.MarkupLine($"[blue]SCORE: {Game.player.Score}[/]\t[red]DMG: {Game.player.Dmg}[/]");
                var choices = new List<string> { "Exit", "DAMAGE" };
                for (int i = 0; i < Game.farms.Count; i++)
                {
                    var farm = Game.farms[i];
                    choices.Add($"FARM ({farm.ScoreIncrement+5} score every {farm.TimeInterval} seconds)");
                }
                var upgrade = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(choices));

                if (upgrade == "Exit")
                {
                    Game.openwin = false;
                }
                else if (upgrade == "DAMAGE")
                {
                    Game.UpgradeLogic(-1);
                }
                else
                {
                    int farmIndex = choices.IndexOf(upgrade) - 2;
                    Game.UpgradeLogic(farmIndex);
                }

                UpdateStats();
                UpdateTable();
            }
        }
        
        static public void GetAssets()
        {
            image = new CanvasImage("../../../assets/hum.png");
            bossi = new CanvasImage("../../../assets/evil.png");
            image.MaxWidth(15);
            bossi.MaxWidth(15);
            music = new SoundPlayer("../../../assets/musci.wav");
            introm = new SoundPlayer("../../../assets/introm.wav");
            bossm = new SoundPlayer("../../../assets/bossm.wav");
            outrom = new SoundPlayer("../../../assets/outrom.wav");
            err = new SoundPlayer("../../../assets/err.wav");
        }
        
        static public void CreateTable()
        {
            GameWin = new Table();
            GameWin.Alignment(Justify.Center);
            GameWin.Width(100);
            GameWin.Border(TableBorder.Rounded);
            GameWin.AddColumn(new TableColumn("[orange3]STATS[/]").Centered());
            GameWin.AddColumn(new TableColumn("[yellow]TRAIN[/]").Centered());
            GameWin.AddColumn(new TableColumn("[blue]MENU[/]").Centered());
            GameWin.Columns[0].Width(30);
        }

        static public void BossWindow()
        {
            
        }

        static public void Intro()
        {
            introm.Play();
            for(int i=0;i<20;i++)
            {
                if(Console.KeyAvailable)
                {
                    Console.ReadKey();
                    AnsiConsole.Clear();
                    break;
                }
                AnsiConsole.Clear();
                switch (i)
                {
                    case 0:
                        AnsiConsole.Write(new FigletText(" ").Color(Color.Yellow));
                        break;
                    case 1:
                        AnsiConsole.Write(new FigletText("_").Color(Color.Yellow));
                        break;
                    case 2:
                        AnsiConsole.Write(new FigletText(" ").Color(Color.Yellow));
                        break;
                    case 3:
                        AnsiConsole.Write(new FigletText("_").Color(Color.Yellow));
                        break;
                    case 4:
                        AnsiConsole.Write(new FigletText(" ").Color(Color.Yellow));
                        break;
                    case 5:
                        AnsiConsole.Write(new FigletText("W_").Color(Color.Yellow));
                        break;
                    case 6:
                        AnsiConsole.Write(new FigletText("WE_").Color(Color.Yellow));
                        break;
                    case 7:
                        AnsiConsole.Write(new FigletText("WEL_").Color(Color.Yellow));
                        break;
                    case 8:
                        AnsiConsole.Write(new FigletText("WELC_").Color(Color.Yellow));
                        break;
                    case 9:
                        AnsiConsole.Write(new FigletText("WELCO_").Color(Color.Yellow));
                        break;
                    case 10:
                        AnsiConsole.Write(new FigletText("WELCOM_").Color(Color.Yellow));
                        break;
                    case 11:
                        AnsiConsole.Write(new FigletText("WELCOME_").Color(Color.Yellow));
                        break;
                    case 12:
                        AnsiConsole.Write(new FigletText("WELCOME ").Color(Color.Yellow));
                        break;
                    case 13:
                        AnsiConsole.Write(new FigletText("WELCOME_").Color(Color.Yellow));
                        break;
                    case 14:
                        AnsiConsole.Write(new FigletText("WELCOME ").Color(Color.Yellow));
                        break;
                    case 15:
                        AnsiConsole.Write(new FigletText("WELCOM_").Color(Color.Yellow));
                        break;
                    case 16:
                        AnsiConsole.Write(new FigletText("WELCOM_").Color(Color.Yellow));
                        err.Play();
                        break;
                    case 17:
                        AnsiConsole.Write(new FigletText(" ").Color(Color.Yellow));
                        break;
                    case 18:
                        AnsiConsole.Write(new FigletText("_").Color(Color.Yellow));
                        break;
                    case 19:
                        AnsiConsole.Write(new FigletText(" ").Color(Color.Yellow));
                        break;
                }
                System.Threading.Thread.Sleep(500);
            }
        }

        static void Main(string[] args)
        {
            GetAssets();
            CreateTable();
            UpdateTable();
            
            Intro();
            
            AnsiConsole.Write(GameWin);
            music.PlayLooping();
            bool musicOn = true;
            Game.StartTimer();

            while (Game.esc)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false).Key;
                    switch (key)
                    {
                        case ConsoleKey.Backspace:
                            Game.esc = false;
                            break;
                        case ConsoleKey.Spacebar:
                            Game.player.Score += Game.player.Dmg;
                            break;
                        case ConsoleKey.U:
                            UpgradeMenu();
                            break;
                        case ConsoleKey.M:
                            if(musicOn)
                            {
                                music.Stop();
                                musicOn = false;
                            }
                            else
                            {
                                music.PlayLooping();
                                musicOn = true;
                            }
                            break;
                        case ConsoleKey.S:
                            AnsiConsole.Status()
                                .AutoRefresh(true)
                                .Start("[green]Saving...[/]", ctx =>
                                {   
                                    System.Threading.Thread.Sleep(1000);
                                    ctx.Spinner(Spinner.Known.Balloon2);
                                    ctx.SpinnerStyle(Style.Parse("green"));
                                    System.Threading.Thread.Sleep(1000);
                            });
                            Game.SaveGame();
                            break;
                        case ConsoleKey.L:
                            AnsiConsole.Status()
                                .AutoRefresh(true)
                                .Start("[green]Loading...[/]", ctx =>
                                {   
                                    System.Threading.Thread.Sleep(1000);
                                    ctx.Spinner(Spinner.Known.Balloon);
                                    ctx.SpinnerStyle(Style.Parse("green"));
                                    System.Threading.Thread.Sleep(1000);
                                });
                            Game.LoadGame();
                            break;
                    }
                    UpdateTable();
                    UpdateStats();
                    AnsiConsole.Clear();
                    AnsiConsole.Write(GameWin);
                }
            }
            outrom.Play();
            AnsiConsole.MarkupLine("[bold red]Game Over! Thanks for playing![/]\n\nPress anything to close the game...");
            Console.ReadKey();
        }
    }
}