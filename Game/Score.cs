using Godot;
using System;

public class Score : Godot.Label
{
    private string score;
    private Game game;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game = GetTree().CurrentScene as Game;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        score = game.score.ToString().PadZeros(9);
        this.Text = score;
    }
}
