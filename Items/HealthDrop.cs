using Godot;
using System;

public class HealthDrop : Area2D
{
    private GameState gameState;
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
    }

    public void _OnHealthDropBodyEntered(Node body)
    {
        Player player = (Player)body;
        if(player.currentHealth < gameState.maxHealth)
        {
            player.currentHealth+=1;
            QueueFree();
        }
    }
}
