using Godot;
using System;

public class EnemyBase : KinematicBody2D
{

    //Can add more parameters to make every enemy more unique
    protected float speed;
    public float health;
    public float damage;
    protected GameState gameState;
    protected Vector2 direction = Vector2.Zero;
    protected AnimationPlayer animationPlayer;
    protected EndlessMode endlessMode;
    WeightedGroup<string> group = new WeightedGroup<string>(){
        {"Health", 10}, 
        //{"Ammo", 100}, -> FOR SUPERATTACK?
        {"Nothing", 100}

    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("../../../GameState");
        animationPlayer = GetNode<AnimationPlayer>("TakeDamage");
        endlessMode = GetNode<EndlessMode>("../../../EndlessMode");
    }
    public override void _Process(float delta)
    {

    }
    public void DamageAnimation()
    {
        // var Sprite = GetNode<Sprite>("Sprite");
        // Sprite.Modulate = new Color(10, 10, 10);
        animationPlayer.Play("Damage");
    }

    public void SpawnItems()
    {
        switch(group.GetItem())
        {
            case "Health":
                {
                    endlessMode.SpawnHealth();
                }
            break;
            default:
            break;
        }
    }
}
