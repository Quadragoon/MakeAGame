using Godot;
using System;

public class UpgradeScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text"

    public int speedUpgradeAmount = 100;
    private GameState gameState;
    private Player player;
    
    //TODO: Add random upgrades
    //Upgrade health, speed, boostbar in some way, damage
    //Add different rarities to them

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<Player>("/root/Game/Player");
        gameState = GetNode<GameState>("/root/GameState");
    }

    public void _OnUpgradeHealthPressed()
    {
        //TODO: Add sound effects and animation -> maybe animate scene transition
        gameState.maxHealth++;
        gameState.healthBarMax++;
        gameState.healthBarLength += 28;
        ReturnToGame();
    }

    public void _OnUpgradeSpeedPressed()
    {
        //TODO: Add sound effects and animation -> maybe animate scene transition
        gameState.acceleration += speedUpgradeAmount;
        gameState.boostPower += (speedUpgradeAmount * 2);
        ReturnToGame();
    }

    private void ReturnToGame()
    {
        player.UpdateAttributes();
        GetTree().Paused = false;
        QueueFree();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
