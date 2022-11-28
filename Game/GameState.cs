using Godot;
using System;

public class GameState : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public int level = 1;
    public int score = 0;
    public int acceleration = 2500;
    public int boostPower = 5000;
    public float maxHealth = 5;
    public float healthBarMax = 5;
    public int healthBarLength = 150;
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
