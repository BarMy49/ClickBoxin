using Spectre.Console;
namespace ClickBoxin;

public class GamblingMachine
{
    private static Random random = new Random();

    public static void SpinWheelBasic()
    {
        int prizeIndex = random.Next(0, 10); 
        switch (prizeIndex)
        {
            case 0:
                Game.player.Score += 10000;
                AnsiConsole.MarkupLine("[green]You won 10000 Score![/]");
                break;
            case 1:
                Game.player.Score -= 10000;
                AnsiConsole.MarkupLine("[red]You lost 10000 Score![/]");
                break;
            case 2:
                Game.player.Tickets += 3;
                AnsiConsole.MarkupLine("[green]You won 3 Tickets![/]");
                break;
            case 3:
                Game.player.Tickets -= 1;
                AnsiConsole.MarkupLine("[red]You lost Tickets![/]");
                break;
            case 4:
                Game.player.Ultra += 10;
                AnsiConsole.MarkupLine("[green]You won 10 Ultra![/]");
                break;
            case 5:
                Game.player.Ultra -= 10;
                AnsiConsole.MarkupLine("[red]You lost 10 Ultra![/]");
                break;
            case 6:
                Game.player.Stage += 1;
                AnsiConsole.MarkupLine("[green]You advanced 1 Stage![/]");
                break;
            case 7:
                if(Game.player.Stage > 1)
                {
                    Game.player.Stage -= 1;
                    AnsiConsole.MarkupLine("[red]You went back 1 Stage![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]You can't go back any further![/]");
                }
                break;
            case 8:
                Game.player.Score += 5000;
                AnsiConsole.MarkupLine("[green]You won 5000 Score![/]");
                break;
            case 9:
                Game.player.Ultra += 5;
                AnsiConsole.MarkupLine("[green]You won 5 Ultra![/]");
                break;
            case 10:
                Game.player.Score = 100000;
                AnsiConsole.MarkupLine("[green]Your score was set to 100000![/]");
                break;
        }
    }
    public static void SpinWheelAdvanced()
    {
        int prizeIndex = random.Next(0, 10);
        switch (prizeIndex)
        {
            case 0:
                Game.player.Score += 100000;
                AnsiConsole.MarkupLine("[green]You won 100000 Score![/]");
                break;
            case 1:
                Game.player.Score -= 100000;
                AnsiConsole.MarkupLine("[red]You lost 100000 Score![/]");
                break;
            case 2:
                Game.player.Tickets += 15;
                AnsiConsole.MarkupLine("[green]You won 3 Tickets![/]");
                break;
            case 3:
                Game.player.Tickets += 5;
                AnsiConsole.MarkupLine("[red]You won 5 Tickets![/]");
                break;
            case 4:
                Game.player.Ultra += 100;
                AnsiConsole.MarkupLine("[green]You won 100 Ultra![/]");
                break;
            case 5:
                Game.player.Ultra -= 50;
                AnsiConsole.MarkupLine("[red]You lost 50 Ultra![/]");
                break;
            case 6:
                Game.player.Stage += 2;
                AnsiConsole.MarkupLine("[green]You advanced 2 Stages![/]");
                break;
            case 7:
                if(Game.player.Stage > 2)
                {
                    Game.player.Stage -= 2;
                    AnsiConsole.MarkupLine("[red]You went back 2 Stages![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]You can't go back any further![/]");
                }
                break;
            case 8:
                Game.player.Score += 50000;
                AnsiConsole.MarkupLine("[green]You won 50000 Score![/]");
                break;
            case 9:
                Game.player.Ultra += 50;
                AnsiConsole.MarkupLine("[green]You won 50 Ultra![/]");
                break;
            case 10:
                Game.player.Score = 1000000;
                AnsiConsole.MarkupLine("[green]Your score was set to 1000000![/]");
                break;
        }
    }
}