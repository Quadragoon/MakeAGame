using Godot;
using System;

public class UpgradeScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text"

    public int speedUpgradeAmount = 100;
    
    //TODO: Add random upgrades

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _OnUpgradeHealthPressed()
    {
        //TODO: Add sound effects and animation -> maybe animate scene transition
        var gameState = GetNode<GameState>("../GameState");
        gameState.maxHealth++;
        gameState.healthBarMax++;
        gameState.healthBarLength+=28;
        GD.Print(gameState.maxHealth);
        GetTree().ChangeScene("res://Game/EndlessMode.tscn");
    }

    public void _OnUpgradeSpeedPressed()
    {
        //TODO: Add sound effects and animation -> maybe animate scene transition
        var gameState = GetNode<GameState>("../GameState");
        gameState.acceleration += speedUpgradeAmount;
        gameState.boostPower += (speedUpgradeAmount * 2);
        GetTree().ChangeScene("res://Game/EndlessMode.tscn");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
