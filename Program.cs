using System.Media;
using Spectre.Console;
using SixLabors.ImageSharp;
using SCColor = Spectre.Console.Color;

namespace ClickBoxin
{
    class Window
    {
        static public CanvasImage image;
        static public CanvasImage bossi;
        static public SoundPlayer music;
        static public SoundPlayer introm;
        static public SoundPlayer outrom;
        static public SoundPlayer bossm;
        
        static public Table GameWin;
        static public Table BossWin;

        static public string stats = "";
        static public bool musicOn;
        static public bool pressed = false;

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
            GameWin.AddRow(new Markup(" "), new Markup($"score: {Game.player.Score}"), new Markup($"Press /B/ to FIGHT THE BOSS!\nCost: {Game.boss.Cost}"));
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
        static public void UpgradeMenu()
        {
            Game.WindowOpened = 1;
            while (Game.WindowOpened == 1)
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
                    Game.WindowOpened = 0;
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
        static public void UltraUpgradeMenu()
        {
            Game.WindowOpened = 3;
            while (Game.WindowOpened == 3)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]ULTRA UPGRADE MENU[/]");
                AnsiConsole.MarkupLine($"[blue]ULTRA: {Game.player.Score}[/]\t[red]DMG: {Game.player.Dmg}[/]");
                var choices = new List<string> { "Exit", "DAMAGE MULTIPLIER" };
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
                    Game.WindowOpened = 0;
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
        static public void CreateBossTable()
        {
            BossWin = new Table();
            BossWin.Alignment(Justify.Center);
            BossWin.Width(100);
            BossWin.Border(TableBorder.Ascii2);
            BossWin.AddColumn(new TableColumn("[orange3]STATS[/]").Centered());
            BossWin.AddColumn(new TableColumn("[red]BOSS[/]").Centered());
            BossWin.AddColumn(new TableColumn("[blue]MENU[/]").Centered());
            BossWin.Columns[0].Width(30);
        }
        static public void BossTable()
        {
            BossWin.Rows.Clear();
            BossWin.AddRow(new Markup($"STAGE: {Game.player.Stage}\n[orange3]DMG: {Game.player.Dmg}[/]"), bossi
                , new Markup("[green]Press /Spacebar/ to attack! " +
                             "\nPress /M/ to mute! " +
                             "\nPress /Backspace/ to exit the battle![/]"));
            BossWin.AddRow(new Markup(" "), new Markup($"\n[red]BOSS HEALTH: {Game.boss.Health}[/]TIME LEFT: {Game.boss.Time}"));
        }

        static public void BossWindow()
        {
            Game.WindowOpened = 2;
            
            AnsiConsole.Clear();
            AnsiConsole.Write(BossWin);
            if(musicOn)
            {
                bossm.PlayLooping();
            }
            
            while(Game.boss.Time>0)
            {
                if (Game.boss.Health <= 0)
                {
                    Game.BossWon();
                    bossm.Stop();
                    AnsiConsole.Write(new Markup("[bold green]You won![/]"));
                    Thread.Sleep(2000);
                    break;
                }
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Spacebar:
                            Game.boss.Health -= Game.player.Dmg;
                            break;
                        case ConsoleKey.M:
                            if(musicOn)
                            {
                                bossm.Stop();
                                musicOn = false;
                            }
                            else
                            {
                                bossm.PlayLooping();
                                musicOn = true;
                            }
                            break;
                        case ConsoleKey.Backspace:
                            Game.boss.Time = 0;
                            break;
                    }
                    AnsiConsole.Clear();
                    BossTable();
                    AnsiConsole.Write(BossWin);
                }
            }
            Game.WindowOpened = 0;
            music.PlayLooping();
            
        }

        static public void Intro()
        {
            introm.Play();
            AnsiConsole.Clear();
            System.Threading.Thread.Sleep(1000);
            AnsiConsole.Write(new FigletText("CLICKBOXIN").Color(SCColor.Green));
            System.Threading.Thread.Sleep(2000);
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Ascii)
                .SpinnerStyle(Style.Parse("yellow"))
                .Start("[bold green]Starting the game...[/]", ctx =>
                {
                    System.Threading.Thread.Sleep(1000);
                    ctx.Status("[bold yellow]Loading Assets...[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Images loaded[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Music loaded[/]");
                    System.Threading.Thread.Sleep(1000);
                    
                    ctx.Status("[bold yellow]Loading player data...[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Score loaded[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Damage loaded[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Stage loaded[/]");
                    
                    ctx.Status("[bold yellow]Loading more data...[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Upgrades loaded[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Bosses loaded[/]");
                    System.Threading.Thread.Sleep(100);
                    AnsiConsole.MarkupLine($"[green]Logic loaded[/]");
                    
                    ctx.Status("[bold yellow]Finalizing...[/]");
                    System.Threading.Thread.Sleep(2000);
                    AnsiConsole.MarkupLine($"[green]Game loaded![/]");
                    System.Threading.Thread.Sleep(1000);
                });
            AnsiConsole.Clear();
        }

        static public void UltraRestartLoad()
        {
            Game.WindowOpened = 4;
            AnsiConsole.Clear();
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Arrow2)
                .SpinnerStyle(Style.Parse("yellow"))
                .Start("[bold green]Uploading...[/]", ctx =>
                {
                    System.Threading.Thread.Sleep(2000);
                    AnsiConsole.MarkupLine($"[green]Uploading the stats. Please hold on.[/]");
                    System.Threading.Thread.Sleep(3000);
                    AnsiConsole.MarkupLine($"[green]Player's score: {Game.player.Score}[/]");
                    System.Threading.Thread.Sleep(500);
                    AnsiConsole.MarkupLine($"[green]Player's Damage: {Game.player.Dmg}[/]");
                    System.Threading.Thread.Sleep(500);
                    AnsiConsole.MarkupLine($"[green]Player's Stage: {Game.player.Stage}[/]");
                    System.Threading.Thread.Sleep(500);
                    ctx.Spinner(Spinner.Known.Ascii);
                    ctx.Status("[bold yellow]Calculating Ultra...[/]");
                    System.Threading.Thread.Sleep(5000);
                    ctx.Spinner(Spinner.Known.Dots2);
                    ctx.Status("[bold yellow]Restarting...[/]");
                    AnsiConsole.MarkupLine($"[yellow]Restarting the game. Please hold on.[/]");
                    System.Threading.Thread.Sleep(5000);
                    AnsiConsole.MarkupLine("[bold]Restart Completed![/]");
                    System.Threading.Thread.Sleep(3000);
                });
            Intro();
            Game.WindowOpened = 0;
        }

        static void Main(string[] args)
        {
            GetAssets();
            CreateTable();
            CreateBossTable();
            Game.CreateBoss(Game.player.Stage);
            
            Intro();
            
            UpdateTable();
            AnsiConsole.Write(GameWin);
            music.PlayLooping();
            musicOn = true;
            Game.StartTimer();

            while (Game.esc)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
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
                        case ConsoleKey.I:
                        {
                            UltraUpgradeMenu();
                            break;
                        }
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
                        case ConsoleKey.B:
                        {
                            if (Game.player.Score >= Game.boss.Cost)
                            {
                                Game.player.Score -= Game.boss.Cost;
                                BossWindow();
                            }
                            break;
                        }
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