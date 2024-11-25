﻿using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Windows.Media.Capture;
using Spectre.Console;
using SixLabors.ImageSharp;
using SCColor = Spectre.Console.Color;

namespace ClickBoxin
{
    class Window
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 vKey);
        
        static public CanvasImage image;
        static public CanvasImage bossi;
        static public SoundPlayer music;
        static public SoundPlayer introm;
        static public SoundPlayer outrom;
        static public SoundPlayer bossm;
        
        static public Table GameWin;
        static public Table BossWin;
        static public Table DailyBonus;

        static public string stats = "";
        static public string menu = "";
        static public string[] daily = new string[7];
        static public bool musicOn;
        static public bool SpacebarPressed = false;
        static public GamblingMachine GamblingMachine = new GamblingMachine();

        static public void GetAssets()
        {
            image = new CanvasImage("../../../assets/hum.png");
            image.MaxWidth(15);
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
            
            DailyBonus = new Table();
            DailyBonus.Alignment(Justify.Center);
            DailyBonus.Width(100);
            DailyBonus.Border(TableBorder.Double);
            for (int i = 0; i < 7; i++)
            {
                DailyBonus.AddColumn(new TableColumn($"[bold]DAY {i+1}[/]").Centered());
            }
        }
        static public void UpdateStats()
        {
            stats = $"[bold]{Game.player.Name}[/]" + $"\nSTAGE: {Game.player.Stage}" +
                    $"\n[orange3]DMG: {Game.player.Dmg} X{Game.player.DmgMulti}[/]";
            for (int i = 0; i < Game.farms.Count; i++)
            {
                var farm = Game.farms[i];
                if(farm.ScorePP > 0) stats += $"\n+ {farm.ScorePP} score every {farm.TimeInterval} seconds \n{GenerateProgressBar(Game.time % farm.TimeInterval, farm.TimeInterval)}";
            }
        }
        static public void UpdateTable()
        {
            menu = "[green]Press /Spacebar/ to attack!" + "\nPress /U/ to upgrade!" + "\nPress /Y/ to open ClickMenu";
            if (Game.player.Stage > 2)
            {
                menu += "\nPress /R/ restart the game!";
            }
            menu += "\nPress /S/ to open settings!" + "\nPress /Backspace/ to exit the game![/]";
            GameWin.Rows.Clear();
            GameWin.AddRow(new Markup($"{stats}"), image, new Markup(menu));
            GameWin.AddRow(new Markup(" "), new Markup($"score: {Game.player.Score}"), new Markup($"Press /B/ to FIGHT THE BOSS!\nCost: {Game.boss.Cost}"));
        }
        static public void UpdateDailyBonus()
        {
            DailyBonus.Rows.Clear();
            for (int i = 0; i < 7; i++)
            {
                if((Game.player.DailyLoginStreak-1)%7 < i)
                {
                    switch (i)
                    {
                        case 0:
                            daily[i] = "500 Score\n[green]CLAIM[/]";
                            break;
                        case 1:
                            daily[i] = "5000 Score\n[green]CLAIM[/]";
                            break;
                        case 2:
                            daily[i]= "3 Tickets\n[green]CLAIM[/]";
                            break;
                        case 3:
                            daily[i] = "10000 Score\n[green]CLAIM[/]";
                            break;
                        case 4:
                            daily[i] = "10 Ultra\n[green]CLAIM[/]";
                            break;
                        case 5:
                            daily[i] = "6 Tickets\n[green]CLAIM[/]";
                            break;
                        case 6:
                            daily[i] = "100000 Score\n[green]CLAIM[/]";
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            daily[i] = "500 Score\n[red]CLAIMED[/]";
                            break;
                        case 1:
                            daily[i] = "5000 Score\n[red]CLAIMED[/]";
                            break;
                        case 2:
                            daily[i] = "3 Tickets\n[red]CLAIMED[/]";
                            break;
                        case 3:
                            daily[i] = "10000 Score\n[red]CLAIMED[/]";
                            break;
                        case 4:
                            daily[i] = "10 Ultra\n[red]CLAIMED[/]";
                            break;
                        case 5:
                            daily[i] = "6 Tickets\n[red]CLAIMED[/]";
                            break;
                        case 6:
                            daily[i] = "100000 Score\n[red]CLAIMED[/]";
                            break;
                    }
                }
            }
            DailyBonus.AddRow(daily[0], daily[1], daily[2], daily[3], daily[4], daily[5], daily[6]);
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
        static public void SettingsMenu()
        {
            Game.WindowOpened = 5;
            while (Game.WindowOpened == 5)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]SETTINGS MENU[/]");
                AnsiConsole.MarkupLine($"[green]Settings for profile {Game.player.Name}[/]");
                var choices = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices("Exit", "Save Game", "Load Game", "Audio Off/On", "Reset All Progress"));

                switch (choices)
                {
                    case "Exit":
                        Game.WindowOpened = 0;
                        break;
                    case "Save Game":
                        Game.SaveGame();
                        AnsiConsole.Status()
                            .AutoRefresh(true)
                            .Start("[green]Saving...[/]", ctx =>
                            {   
                                System.Threading.Thread.Sleep(1000);
                                ctx.Spinner(Spinner.Known.Balloon2);
                                ctx.SpinnerStyle(Style.Parse("green"));
                                System.Threading.Thread.Sleep(1000);
                            });
                        break;
                    case "Load Game":
                        LoadMenu();
                        break;
                    case "Audio Off/On":
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
                    case "Reset All Progress":
                        Game.ResetAllProgress();
                        AnsiConsole.Clear();
                        musicOn = false;
                        InitialLogin();
                        musicOn = true;
                        break;
                }

                UpdateStats();
                UpdateTable();
            }
        }
        static public void LoadMenu()
        {
            Game.WindowOpened = 6;
            while (Game.WindowOpened == 6)
            {
                AnsiConsole.Clear();
                
                AnsiConsole.MarkupLine($"[bold]LOAD MENU[/]");
                AnsiConsole.MarkupLine($"[green]Choose the profile to load[/]");
                var choices = new List<string> {"Exit"};
                if(File.Exists("saves/users.txt"))
                {
                    var Users = File.ReadAllText("saves/users.txt");
                    string[] users = Users.Split(",");
                    for(int i = 0;i<users.Count();i++)
                    {
                        choices.Add(users[i]);
                    }
                }
                var profile = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(choices));

                if (profile == "Exit")
                {
                    Game.WindowOpened = 0;
                }
                else
                {
                    AnsiConsole.Status()
                        .AutoRefresh(true)
                        .Start("[green]Loading...[/]", ctx =>
                        {   
                            System.Threading.Thread.Sleep(1000);
                            ctx.Spinner(Spinner.Known.Balloon);
                            ctx.SpinnerStyle(Style.Parse("green"));
                            System.Threading.Thread.Sleep(1000);
                        });
                    Game.LoadGame(profile);
                    Game.WindowOpened = 0;
                }
            }
        }
        static public void ClickMenu()
        {
            Game.WindowOpened = 7;
            while (Game.WindowOpened == 7)
            {
                AnsiConsole.Clear();
                UpdateDailyBonus();
                AnsiConsole.MarkupLine($"[bold blue]DAILY LOGIN BONUS \t DAILY STREAK: {Game.player.DailyLoginStreak}[/]");
                AnsiConsole.Write(DailyBonus);
                AnsiConsole.WriteLine($"{Game.player.Name} STATS(Stage: {Game.player.Stage} Score: {Game.player.Score}[blue]Ultra: {Game.player.Ultra}[/] [orange3]Dmg: {Game.player.Dmg} [bold]Dmg Multi: {Game.player.DmgMulti}[/][/] Tickets: {Game.player.Tickets} Restarts: {Game.player.Restarts})");
                AnsiConsole.MarkupLine($"[bold]CLICK MENU[/]");
                var choices = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices("Exit", "Claim Daily Reward", "Ultra Upgrade", "Achievements", "Gamble Machine"));
                switch (choices)
                {
                    case "Exit":
                        Game.WindowOpened = 0;
                        break;
                    case "Claim Daily Reward":
                        if(Game.player.HasClaimedRewardToday())
                        {
                            AnsiConsole.MarkupLine("[red]You have already claimed your reward today![/]");
                        }
                        else
                        {
                            Game.ClaimReward();
                        }
                        break;
                    case "Ultra Upgrade":
                        UltraUpgradeMenu();
                        break;
                    case "Achievements":
                        AchievementsMenu();
                        break;
                    case "Gamble Machine":
                        GambleMenu();
                        break;
                }

                UpdateStats();
                UpdateTable();
            }
        }
        static public void AchievementsMenu()
        {
            Game.WindowOpened = 8;
            while (Game.WindowOpened == 8)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[bold]ACHIEVEMENTS MENU[/]");
                for(int i = 0; i<Game.Achievs.Count;i++)
                {
                    if (Game.Achievs[i].IsUnlocked)
                    {
                        AnsiConsole.MarkupLine($"[green]UNLOCKED {Game.Achievs[i].Name}[/]\n{Game.Achievs[i].Description}");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]LOCKED {Game.Achievs[i].Name}[/]\n{Game.Achievs[i].Description}");
                    }
                }

                Console.ReadKey();
                Game.WindowOpened = 0;

                UpdateStats();
                UpdateTable();
            }
        }
        static public void GambleMenu()
        {
            Game.WindowOpened = 9;
            while (Game.WindowOpened == 9)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("GAMBLE").Color(SCColor.Yellow));
                AnsiConsole.MarkupLine($"[bold]LET'S GO GAMBLING![/][green]Tickets: {Game.player.Tickets}[/] Score:{Game.player.Score} [blue]Ultra: {Game.player.Ultra} [/][bold]Stage: {Game.player.Stage}[/]");
                AnsiConsole.MarkupLine($"[green bold]WARNING![/] [green]Gamble Machine can set the values at negative(except Stage) and also doesn't update current boss!\n\tThe game will also automatically save after gambling![/]");
                var choices = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices("Exit", "GAMBLE \t\t for 1 Ticket", "EPIC GAMBLE \t for 10 Tickets"));
                switch (choices)
                {
                    case "Exit":
                        Game.WindowOpened = 0;
                        break;
                    case  "GAMBLE \t\t for 1 Ticket":
                        Game.Achievs[13].Unlock();
                        Game.player.Tickets--;
                        AnsiConsole.Status()
                            .AutoRefresh(true)
                            .Spinner(Spinner.Known.Balloon)
                            .SpinnerStyle(Style.Parse("bold red"))
                            .Start("[bold red]GAMBLE...[/]", ctx =>
                            {   
                                System.Threading.Thread.Sleep(1000);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +10000 Score[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +10 Ultra[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +2Tickets Score[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: -10 Ultra[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +1 Stage[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: -1 Ticket[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: -10000 Score[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: 10000 Score[/]");
                                System.Threading.Thread.Sleep(50);
                            });
                        GamblingMachine.SpinWheelBasic();
                        System.Threading.Thread.Sleep(5000);
                        Game.SaveGame();
                        break;
                    case "EPIC GAMBLE \t for 10 Tickets":
                        Game.Achievs[13].Unlock();
                        Game.player.Tickets--;
                        AnsiConsole.Status()
                            .AutoRefresh(true)
                            .Spinner(Spinner.Known.Balloon)
                            .SpinnerStyle(Style.Parse("bold red"))
                            .Start("[bold red]EPIC GAMBLE...[/]", ctx =>
                            {   
                                System.Threading.Thread.Sleep(1000);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +10000 Score[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +10 Ultra[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +2Tickets Score[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: -10 Ultra[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: +1 Stage[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: -1 Ticket[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: -10000 Score[/]");
                                System.Threading.Thread.Sleep(50);
                                AnsiConsole.MarkupLine("[green]GAMBLE: 10000 Score[/]");
                                System.Threading.Thread.Sleep(50);
                            });
                        GamblingMachine.SpinWheelAdvanced();
                        System.Threading.Thread.Sleep(5000);
                        Game.SaveGame();
                        break;
                }

                UpdateStats();
                UpdateTable();
            }
        }
        static public void UpgradeMenu()
        {
            Game.WindowOpened = 1;
            while (Game.WindowOpened == 1)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold]UPGRADE MENU[/]");
                AnsiConsole.MarkupLine($"[blue]SCORE: {Game.player.Score}[/]\t[red]DMG: {Game.player.Dmg}[/]");
                var choices = new List<string> { "Exit", $"DAMAGE \t\t cost:{(Game.player.Dmg * 100) * Game.player.Dmg}" };
                for (int i = 0; i < Game.farms.Count; i++)
                {
                    var farm = Game.farms[i];
                    choices.Add($"{farm.TimeInterval} SECOND FARM \t cost:{farm.Cost}");
                }
                var upgrade = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(choices));

                if (upgrade == "Exit")
                {
                    Game.WindowOpened = 0;
                }
                else if (upgrade == $"DAMAGE \t\t cost:{(Game.player.Dmg * 100) * Game.player.Dmg}")
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

                AnsiConsole.MarkupLine($"[bold]ULTRA UPGRADE MENU[/]\n[green]Ultra upgrades stay after restart![/]");
                AnsiConsole.MarkupLine($"[blue]ULTRA: {Game.player.Ultra}[/]\t[red]DMG: {Game.player.Dmg} X{Game.player.DmgMulti}[/]");
                var choices = new List<string> { "Exit", 
                    $"DAMAGE MULTIPLIER [green]for {Game.player.DmgMulti * Game.player.DmgMulti}[/]",
                    $"GREATER BOSS TIME! [green]for {Game.bosstime/2}[/]"
                };
                for (int i = 0; i < Game.farms.Count; i++)
                {
                    var farm = Game.farms[i];
                    choices.Add($"LOWER FARM'S TIME! Current: {farm.TimeInterval} seconds [green]for {Game.farms[i].TimeCost}[/]");
                }
                var upgrade = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(choices));

                if (upgrade == "Exit")
                {
                    Game.WindowOpened = 0;
                }
                else if (upgrade == $"DAMAGE MULTIPLIER [green]for {Game.player.DmgMulti * Game.player.DmgMulti} ultra[/]")
                {
                    Game.UltraUpgradeLogic(-1);
                }
                else if (upgrade == $"GREATER BOSS TIME! [green]for {Game.bosstime/2} ultra[/]")
                {
                    Game.UltraUpgradeLogic(0);
                }
                else
                {
                    int farmIndex = choices.IndexOf(upgrade) - 2;
                    Game.UltraUpgradeLogic(farmIndex);
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
            
            var random = new Random();
            int bossimage = random.Next(1, 5);
            switch (bossimage)
            {
                case 1:
                    bossi = new CanvasImage("../../../assets/boss1.png");
                    break;
                case 2:
                    bossi = new CanvasImage("../../../assets/boss2.png");
                    break;
                case 3:
                    bossi = new CanvasImage("../../../assets/boss3.png");
                    break;
                case 4:
                    bossi = new CanvasImage("../../../assets/boss4.png");
                    break;
                case 5:
                    bossi = new CanvasImage("../../../assets/boss5.png");
                    break;
            }
            bossi.MaxWidth(15);
            
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
                            Game.boss.Health -= (Game.player.Dmg*Game.player.DmgMulti);
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
            Game.boss.Time = Game.bosstime;
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
        static public void InitialLogin()
        {
            AnsiConsole.MarkupLine("[green]Start your adventure.[/]");
            AnsiConsole.MarkupLine("[bold]Enter your username:[/]");
            while(true)
            {
                Game.player.Name = Console.ReadLine();
                if ((Game.player.Name.Length < 10&&Game.player.Name.Length>0)&&!(Game.player.Name.Contains("/")||Game.player.Name.Contains(",")||Game.player.Name.Contains(".")))
                {
                    Directory.CreateDirectory("saves");
                    Game.SaveName(Game.player.Name);
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Username incorrect or is too long![/]");
                }
            }
        }

        static public void UltraRestartLoad()
        {
            Game.WindowOpened = 4;
            music.Stop();
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
            if (!File.Exists("saves/users.txt"))
            {
                InitialLogin();
            }
            else
            {
                LoadMenu();
                if(Game.player.Name == "")
                {
                    InitialLogin();
                }
            }
            Game.CreateBoss(Game.player.Stage,Game.bosstime);
            
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
                            SpacebarPressed = true;
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
                        case ConsoleKey.R:
                        {
                            Game.WindowOpened = 4;
                            AnsiConsole.Clear();
                            AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
                            AnsiConsole.MarkupLine($"You will lose all your progress but gain [bold]{(Game.player.Score/10000) * Game.player.Stage * Game.player.Dmg} ULTRA![/]\n[italic grey]Ultra will be counted from 10000 score! || Ultra upgrades stay after restart![/]");
                            AnsiConsole.MarkupLine("[green]Press /spacebar/ to confirm[/] or [red]any other key to cancel[/]");
                            key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.Spacebar)
                            {
                                UltraRestartLoad();
                                Game.UltraRestart();
                            }
                            break;
                        }
                        case ConsoleKey.S:
                            SettingsMenu();
                            break;
                        case ConsoleKey.Y:
                            ClickMenu();
                            break;
                    }
                    UpdateTable();
                    UpdateStats();
                    AnsiConsole.Clear();
                    AnsiConsole.Write(GameWin);
                }
                else
                {
                    if (SpacebarPressed == true)
                    {
                        Game.player.Score += (Game.player.Dmg*Game.player.DmgMulti);
                        SpacebarPressed = false;
                    }
                }
            }
            outrom.Play();
            AnsiConsole.MarkupLine("[bold red]Game Over! Thanks for playing![/]\n\nPress anything to close the game...");
            Console.ReadKey();
        }
    }
}