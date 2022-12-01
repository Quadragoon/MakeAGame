using Godot;
using System;

public class Objective : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Game game;
    private string objective;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game = GetTree().CurrentScene as Game;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch(game.objectiveType)
        {
            case "Survival":
            this.Text = (GetNode<Timer>("../../SurvivalTimer").TimeLeft).ToString("0.##");
            break;

            case "Slay":
            this.Text = ("Kills: " + game.kills.ToString()); 
            break;

            case "Boss":
            this.Text = "Bosstime Baby";
            break;

        }
    }
}
