using System.Timers;
using Spectre.Console;
using Timer = System.Timers.Timer;

namespace ClickBoxin;

public class Game
{
        static public Player player = new Player();    
    
        static public bool esc = true;
        static public int WindowOpened = 0; //0 - Main, 1 - Upgrade Menu, 2 - Boss Window, 3 - Ultra Upgrade Menu, 4 - Ultra Restart, 5 - Settings, 6 - Load Menu, 7 - ClickMenu, 8 - Achievements
        static public string data = "";
        static public int time = 0;
        static public int bosstime = 30;

        static public Boss boss;
        
        static public List<Farm> farms = new List<Farm>
        {
            new Farm(60, 500, 2),
            new Farm(50, 1000, 4),
            new Farm(40, 5000, 8),
            new Farm(30, 10000, 16),
            new Farm(20, 20000, 32),
            new Farm(10, 100000,64)
        };

        static public List<Achievements> Achievs = new List<Achievements>
        {
            new Achievements("MORE POWAHHHH!", "Upgrade Damage for the first time."),           //1
            new Achievements("AUTOMATED", "Upgrade your first farm."),                                      //2
            new Achievements("FIRST BLOOD", "Kill your first boss"),                                                //3
            new Achievements("FIRST 1% OF MILLION", "Gain 10 000 score"),                                   //4
            new Achievements("WE'RE HALFWAY THERE", "Gain 500 000 score"),                            //5
            new Achievements("1 000 000 SCORE", "Gain 1 million score"),                                        //6
            new Achievements("LET'S DO IT AGAIN", "Restart the game for the first time"),              //7
            new Achievements("TASTE OF ULTRA", "Upgrade using ultra for the first time"),            //8
            new Achievements("STAGE 10: CLICKER FORCE", "Achieve stage 10"),                            //9
            new Achievements("RE: RE: RE:", "Restart the game 10 times"),                                      //10 
            new Achievements("DAILY GRIND", "Login for 2 days in a row"),                                       //11
            new Achievements("ALL WEEK BABY", "Login for 7 days in a row"),                                 //12
            new Achievements("THE GRIND NEVER STOPS!", "Login for 30 days in a row"),               //13
            new Achievements("LET'S GO GAMBLING!", "Use gamble machine for the first time"),    //14
            new Achievements("A CLEAN INDIVIDUAL", "Gather 10 tickets"),                                     //15
            new Achievements("CLICKER GOD", "Unlock all achievements")                                      //16
        };

        static public void ResetAllProgress()
        {
            player = new Player();
            farms = new List<Farm>
            {
                new Farm(60, 500, 2),
                new Farm(50, 1000, 4),
                new Farm(40, 5000, 8),
                new Farm(30, 10000, 16),
                new Farm(20, 20000, 32),
                new Farm(10, 100000,64)
            };
            boss = new Boss(1, 30);
            for(int i = 0; i < Achievs.Count; i++)
            {
                Achievs[i].Lock();
            }
        }
        static public void SaveName(string name)
        {
            using (StreamWriter sw = File.AppendText("saves/users.txt"))
            {
                sw.Write($"{name},");
            }
        }
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
                        if(player.Dmg == 2)
                        {
                            Achievs[0].Unlock();
                        }
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
                        farm.Upgrade();
                        if(farm.Lvl == 1)
                        {
                            Achievs[1].Unlock();
                        }
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
                    player.Score += farm.ScorePP;
                }
            }
            if(player.Score >= 10000)
            {
                Achievs[3].Unlock();
            }
            if(player.Score >= 500000)
            {
                Achievs[4].Unlock();
            }
            if(player.Score >= 1000000)
            {
                Achievs[5].Unlock();
            }
            if(player.Stage >= 10)
            {
                Achievs[8].Unlock();
            }
            if(player.Restarts >= 10)
            {
                Achievs[9].Unlock();
            }
            if(player.Tickets >= 10)
            {
                Achievs[14].Unlock();
            }
            if (time == 120) time = 0;
        }

        static public void CreateBoss(int Stage, int BossTime)
        {
             boss = new Boss(Stage,BossTime);
        }

        static public void BossWon()
        {
            Achievs[2].Unlock();
            player.Score += boss.Score;
            player.Stage++;
            WindowOpened = 0;
            CreateBoss(player.Stage, bosstime);
        }

        static public void SaveGame()
        {
            data = "Score - " + player.Score.ToString()
                             + "\nStage-" + player.Stage.ToString()
                             + "\nDmg-" + player.Dmg.ToString()
                             +"\nDmgMulti-" + player.DmgMulti.ToString()
                             + "\nUltra-" + player.Ultra.ToString()
                             + "\nTickets-" + player.Tickets.ToString()
                             + "\nRestarts-" + player.Restarts.ToString()
                             + "\nLastRewardClaimDate-" + player.LastRewardClaimDate.ToString()
                             + "\nDailyLoginStreak-" + player.DailyLoginStreak.ToString();
            for (int i = 0; i < farms.Count; i++)
            {
                data += $"\nFarm{i + 1}ScorePP-{farms[i].Lvl}";
                data += $"\nFarm{i + 1}Cost-{farms[i].Cost}";
                data += $"\nFarm{i + 1}TimeCost-{farms[i].TimeCost}";
            }
            for(int i=0; i < Achievs.Count; i++)
            {
                data += $"\nAchievement{i}-{(Achievs[i].IsUnlocked ? 1 : 0)}";
            }
            data += $"\nProfile-{player.Name}";
            File.WriteAllText($"saves/save_{player.Name}.txt", data);
        }
        static public void LoadGame(string profile)
        {
            if (File.Exists($"saves/save_{profile}.txt"))
            {
                Game.data = File.ReadAllText($"saves/save_{profile}.txt");
                string[] data = Game.data.Split("\n"); 
                player.Score = int.Parse(data[0].Split("-")[1]);
                player.Stage = int.Parse(data[1].Split("-")[1]);
                player.Dmg = int.Parse(data[2].Split("-")[1]);
                player.DmgMulti = int.Parse(data[3].Split("-")[1]);
                player.Ultra = int.Parse(data[4].Split("-")[1]);
                player.Tickets = int.Parse(data[5].Split("-")[1]);
                player.Restarts = int.Parse(data[6].Split("-")[1]);
                player.LastRewardClaimDate = DateTime.Parse(data[7].Split("-")[1]);
                player.DailyLoginStreak = int.Parse(data[8].Split("-")[1]);
                for (int i = 0; i < farms.Count; i++)
                {
                    farms[i].Lvl = int.Parse(data[i * 3 + 9].Split("-")[1]);
                    farms[i].Cost = int.Parse(data[i * 3 + 10].Split("-")[1]);
                    farms[i].TimeCost = int.Parse(data[i * 3 + 11].Split("-")[1]);
                }
                for (int i = 0; i < Achievs.Count; i++)
                {
                    if(int.Parse(data[i + 27].Split("-")[1]) == 1)
                    {
                        Achievs[i].Unlock();
                    }
                }
                player.Name = data[43].Split("-")[1];
                boss = new Boss(player.Stage, bosstime);
            }
        }

        static public void UltraRestart()
        {
            player.Restart();
            for (int i = 0; i < farms.Count; i++)
            {
                farms[i].Restart();
            }
            Achievs[6].Unlock();
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
                        if(player.DmgMulti == 2)
                        {
                            Achievs[7].Unlock();
                        }
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
                        if(bosstime == 31)
                        {
                            Achievs[7].Unlock();
                        }
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
                        Achievs[7].Unlock();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Not enough ultra![/]");
                        Console.ReadKey();
                    }
                }
            }
        }
        static public void DailyLoginBonus()
        {
            if (player.LastRewardClaimDate.Date == DateTime.Today.AddDays(-1))
            {
                player.DailyLoginStreak++;
            }
            else
            {
                player.DailyLoginStreak = 1;
            }
            
            switch (player.DailyLoginStreak)
            {
                case 1:
                    player.Score += 500;
                    AnsiConsole.MarkupLine("[bold]You've got 500 Score![/]");
                    break;
                case 2:
                    player.Score += 5000;
                    AnsiConsole.MarkupLine("[bold]You've got 5000 Score![/]");
                    break;
                case 3:
                    player.Tickets += 3;
                    AnsiConsole.MarkupLine("[bold]You've got 3 Ticket![/]");
                    break;
                case 4:
                    player.Score += 10000;
                    AnsiConsole.MarkupLine("[bold]You've got 10000 Score![/]");
                    break;
                case 5:
                    player.Ultra += 10;
                    AnsiConsole.MarkupLine("[bold]You've got 10 Ultra![/]");
                    break;
                case 6:
                    player.Tickets += 6;
                    AnsiConsole.MarkupLine("[bold]You've got 6 Ticket![/]");
                    break;
                case 7:
                    player.Score += 100000;
                    AnsiConsole.MarkupLine("[bold]You've got 100000 Score![/]");
                    break;
            }
        }
        static public void ClaimReward()
        {
            if (!player.HasClaimedRewardToday())
            {
                DailyLoginBonus();
                player.LastRewardClaimDate = DateTime.Today;
            }
            switch (player.DailyLoginStreak)
            {
                case 2:
                    Achievs[10].Unlock();
                    break;
                case 7:
                    Achievs[11].Unlock();
                    break;
                case 30:
                    Achievs[12].Unlock();
                    break;
            }
        }

        static public void GambleLogic(int gamble)
        {
            if (player.Tickets >= 1)
            {
                player.Tickets -= 1;
                Random rnd = new Random();
                int win = rnd.Next(0, 100);
                if (win < 50)
                {
                    player.Score += 1000;
                    AnsiConsole.MarkupLine("[bold]You've won 1000 Score![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]You've lost![/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Not enough tickets![/]");
            }
        }
}