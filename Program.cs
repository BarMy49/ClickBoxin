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
        //Game Logic Variables
        static public bool esc = true;
        static public bool selected = true;
        static public string data = "";
        
        //Gmae Variables
        static public int score = 0;
        static public int time = 0;
        static public int dmg = 1;
        static public int farm1 = 0;

        static public void UpgradeLogic(int variable)
        {
            AnsiConsole.MarkupLine("[bold]Are you sure?[/]");
            switch (variable)
            {
                case 1:
                    AnsiConsole.MarkupLine($"This will cost {(dmg * 100) * dmg} score!");
                    break;
                case 2:
                    AnsiConsole.MarkupLine($"This will cost 1000 score!");
                    break;
                case 3:
                    AnsiConsole.MarkupLine($"This will cost 10 score!");
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
                        if (score >= 1000)
                        {
                            score -= 1000;
                            farm1 += 2;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Not enough score![/]");
                            Console.ReadKey();
                        }

                        break;
                    case 3:
                        if (score >= 10)
                        {
                            score -= 10;
                        }

                        break;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Not enough score![/]");
                Console.ReadKey();
            }
            
        }

        static public void StartTimer()
        {
            var bonusTimer = new Timer(1000); // 1 seconds
            bonusTimer.Elapsed += OnBonusTimerElapsed;
            bonusTimer.AutoReset = true;
            bonusTimer.Enabled = true;
        }

        static public void OnBonusTimerElapsed(Object source, ElapsedEventArgs e)
        {
            if (time == 60) time = 0;
            time += 1; // Keep track of passed seconds one by one
            if (time % 10 == 0)
            {
                score += farm1;
                
            }
        }
        
        static public void SaveGame()
        {
            data = "Score: " + score.ToString() + "\nDmg: " + dmg.ToString() + "\nFarm1: " + farm1.ToString();
            File.WriteAllText("save.txt", data);
        }
        
        static public void LoadGame()
        {
            if (File.Exists("save.txt"))
            {
                Game.data = File.ReadAllText("save.txt");
                string[] data = Game.data.Split("\n");
                Game.score = int.Parse(data[0].Split(":")[1]);
                Game.dmg = int.Parse(data[1].Split(":")[1]);
                Game.farm1 = int.Parse(data[2].Split(":")[1]);
            }
        }
    }
    
    class Window
    {
        //Game Assets Objects
        static public CanvasImage image;
        static public SoundPlayer music;
        static public Table GameWin;

        static public string stats = "";
        
        //Loading Progress Bar
        static public string p0 = "□□□□□□□□□□ 0%";
        static public string p1 = "■□□□□□□□□□ 10%";
        static public string p2 = "■■□□□□□□□□ 20%";
        static public string p3 = "■■■□□□□□□□ 30%";
        static public string p4 = "■■■■□□□□□□ 40%";
        static public string p5 = "■■■■■□□□□□ 50%";
        static public string p6 = "■■■■■■□□□□ 60%";
        static public string p7 = "■■■■■■■□□□ 70%";
        static public string p8 = "■■■■■■■■□□ 80%";
        static public string p9 = "■■■■■■■■■□ 90%";
        
        static public void UpdateStats()
        {
            //stats = $"[orange3]DMG: {Game.dmg} \nGain {Game.farm1} every 10 seconds \n{p0}";
        }
        static public void UpdateTable()
        {
            GameWin.Rows.Clear();
            GameWin.AddRow(new Markup(stats),image, new Markup("[green]Press /Spacebar/ to attack! \nPress /U/ to upgrade! \nPress /S/ to save! \nPress /L/ to load! \nPress /Backspace/ to exit![/]"));
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
                        .AddChoices(new[] { "Exit", "DMG", "SCORE FARM 1", "Upgrade 3" }));

                switch (upgrade)
                {
                    case "Exit":
                        Game.selected = false;
                        break;
                    case "DMG":
                        Game.UpgradeLogic(1);
                        break;
                    case "SCORE FARM 1":
                        Game.UpgradeLogic(2);
                        break;
                    case "Upgrade 3":
                        Game.UpgradeLogic(3);
                        break;
                }

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

            while (Game.esc)
            {
                AnsiConsole.Write(GameWin);
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
                UpdateStats();
                UpdateTable();
                AnsiConsole.Clear();
            }
        }
    }
}