using Godot;
using System;

public class Explosion : Node2D
{
    public float damage = 1.0f;
    private CollisionShape2D explosion;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        explosion = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
    }

    public void _OnArea2DBodyEntered(Node body)
    {
        explosion.SetDeferred("disabled", true);
        if(body.IsInGroup("Enemies"))
        {
            EnemyBase enemy = (EnemyBase)body;
            enemy.DamageAnimation();
            enemy.health-=damage;
        }
    }
}
