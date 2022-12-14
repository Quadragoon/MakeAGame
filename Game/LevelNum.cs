using Godot;
using System;

public class LevelNum : Label
{
    private string levelNumber;
    private Game game;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game = GetTree().CurrentScene as Game;
    }

    public override void _Process(float delta)
    {
        levelNumber = game.level.ToString();
        this.Text = levelNumber;
    }
}
