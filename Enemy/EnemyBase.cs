using Godot;
using System;

public class EnemyBase : KinematicBody2D
{

    //Can add more parameters to make every enemy more unique
    protected float speed;
    public float health;
    public float damage;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
}
