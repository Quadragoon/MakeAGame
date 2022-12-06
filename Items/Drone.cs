using Godot;
using System;

public class Drone : Node2D
{
    private PackedScene MissileScene = GD.Load<PackedScene>("res://Player/Missile.tscn");
    private float fireAngle = 0.2f;
    private float fireDelay = 0.5f;
    private GameState gameState;
    private AnimationPlayer animationPlayer;
    private Node2D sprite;
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play("pos");
        sprite = GetNode<Sprite>("Sprite");
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(fireDelay > 0)
        {
            fireDelay -= delta;
        }
        else
        {
            Fire();
            fireDelay = 0.5f;
        }
    }

    public void Fire()
    {
        Missile newMissile = MissileScene.Instance<Missile>();
        newMissile.GlobalPosition = sprite.GlobalPosition;
        newMissile.Rotation = sprite.Rotation*3;
        GetParent().AddChild(newMissile);
    }
}
