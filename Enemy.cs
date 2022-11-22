using Godot;
using System;

public class Enemy : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private float speed = 200;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Node2D player = GetNode<Node2D>("../Player");

        float moveAmount = speed * delta;
        var direction = GlobalPosition.DirectionTo(player.GlobalPosition);
        Position += direction * moveAmount;
        
        LookAt(player.GlobalPosition);
    }
}
