using System;
using Spectre.Console;
using SixLabors.ImageSharp;

namespace ClickBoxin;

class Program
{
    static void Main(string[] args)
    {
        int score = 0;
        
        var GameWin = new Table();  
        var UpgWin = new Table();
        
        GameWin.Alignment(Justify.Center);
        GameWin.Width(100);
        GameWin.Border(TableBorder.Rounded);
        GameWin.AddColumn(new TableColumn("[yellow]TRAIN[/]").Centered());
        GameWin.AddColumn(new TableColumn("[blue]UPGRADES[/]").Centered());
        
        UpgWin.AddColumn(new TableColumn("Use arrow keys to choose and press enter").Centered());
        
        void UpdateTable()
        {
            GameWin.Rows.Clear();
            GameWin.AddRow(new Markup($"score: {score}"), UpgWin);
        }
        
        while (true)
        {
            AnsiConsole.Write(GameWin);
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Spacebar :
                    score++;
                    UpdateTable();
                    break;
            }
            AnsiConsole.Clear();
        }
    }
}