using Godot;
using System;

public class Explosion : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float damage = 1.0f;
    private CollisionShape2D explosion;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        explosion = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
    }

    public void _OnArea2DBodyEntered(Node body)
    {
        //explosion.Disabled = true;
        if(body.IsInGroup("Enemies"))
        {
            EnemyBase enemy = (EnemyBase)body;
            enemy.health-=damage;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
