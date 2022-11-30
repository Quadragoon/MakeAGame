using Godot;
using System;

public class Missile : Node2D
{
    public float Speed = 600;
    private float timer = 0.0f;

    [Export]
    public PackedScene ExplosionScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _OnArea2DBodyEntered(Node body)
    {
        
        if(body.IsInGroup("Enemies"))
        {
            EnemyBase enemy = (EnemyBase)body;
            enemy.health--;
        }
        Explode();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timer += delta;
        GlobalPosition += new Vector2((float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation)) * Speed * delta;
        if (timer >= 0.7f)
        {
            Explode();
        }
    }

    protected void Explode()
    {
        Node2D newExplosion = ExplosionScene.Instance<Node2D>();
        newExplosion.GlobalPosition = GlobalPosition;
        newExplosion.Rotation = (GD.Randf())*2*Mathf.Pi;
        GetParent().AddChild(newExplosion);
        QueueFree();
    }
}