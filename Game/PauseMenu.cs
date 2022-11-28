using Godot;
using System;

public class PauseMenu : Control
{

    private bool isPaused = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel")) //Pause game function, ui_cancel bound to esc
        {
                bool newPauseState = !GetTree().Paused; // the new Paused state will be the opposite of the current one
                GetTree().Paused = newPauseState;
                this.Visible = newPauseState; // visible if paused, not visible if not
        }
    }

    public void _OnResumeButtonPressed()
    {
        GetTree().Paused = false;
        this.Visible = false;
    }

    public void _OnQuitButtonPressed()
    {
        ClearGameState();
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Game/Menu.tscn");
    }

    public void ClearGameState()
    {
        var gameState = GetNode<GameState>("../../../GameState");
        gameState.level = 1;
        gameState.score = 0;
        gameState.acceleration = 2500;
        gameState.boostPower = 5000;
        gameState.maxHealth = 5;
        gameState.healthBarMax = 5;
        gameState.healthBarLength = 150;
    }
}
