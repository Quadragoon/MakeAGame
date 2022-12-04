using Godot;
using System;

public class PauseMenu : Control
{

    private bool isPaused = false;
    private GameState gameState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel") == true && gameState.upgradeMode == false) //Pause game function, ui_cancel bound to esc
        {
                //GD.Print(GetTree().CurrentScene.Name);
                bool newPauseState = !GetTree().Paused; // the new Paused state will be the opposite of the current one
                GetTree().Paused = newPauseState;
                this.Visible = newPauseState; // visible if paused, not visible if not
                //Display stats for health, speed, damage, etc.
                GetNode<Label>("StatsContainer/Health").Text = "Health: " + gameState.maxHealth.ToString();
                GetNode<Label>("StatsContainer/Speed").Text = "Speed: " + gameState.acceleration.ToString("0.##");
                GetNode<Label>("StatsContainer/Damage").Text = "Damage: " + gameState.damage.ToString("0.##");
                GetNode<Label>("StatsContainer/AdditionalMissiles").Text = "Missiles: " + (gameState.missileMultiplierChance/100 + 1).ToString();
                GetNode<Label>("StatsContainer/AOE").Text = "Explosion Radius: " + gameState.explosionRadiusScale.ToString("0.##");
                GetNode<Label>("StatsContainer/Fire Speed").Text = "Fire Rate: " + (1/gameState.fireDelay).ToString("0.##");
                GetNode<Label>("StatsContainer/Boost Cooldown").Text = "Boost Cooldown: " + gameState.boostCooldown.ToString("0.##");
                //print float displaying only 2 decimals
                


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
        var gameState = GetNode<GameState>("/root/GameState");
        gameState.ClearGameState();
    }
}
