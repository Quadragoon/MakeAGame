using Godot;
using System;

public class Missile : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public int Speed = 600;
    public Vector2 TargetLocation;
    private float timer = 0.0f;

    [Export]
    public PackedScene ExplosionScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timer += delta;
        GlobalPosition += new Vector2((float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation)) * Speed * delta;
        if (timer >= 5.0f || GlobalPosition.DistanceTo(TargetLocation) <= 10)
        {
            //QueueFree();
            Node2D newExplosion = ExplosionScene.Instance<Node2D>();
            newExplosion.GlobalPosition = GlobalPosition;
            newExplosion.Rotation = (GD.Randf())*2*Mathf.Pi;
            GetParent().AddChild(newExplosion);
            QueueFree();
        }
    }
}
