using Godot;
using System;

public class Enemy3 : EnemyBase
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";;

    private Vector2 direction = Vector2.Zero;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        speed = 200; //Derived from EnemyBase
        health = 2;
        damage = 1;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Game game = GetNode<Game>("../.");
        if(health <=0)
        {
            //Todo: Add on death animation
            game.score+=100;
            game.kills++; //TODO: CHANGE THIS
            QueueFree();
        }
        Node2D player = GetNode<Node2D>("../Player");
        direction = GlobalPosition.DirectionTo(player.GlobalPosition);
        LookAt(player.GlobalPosition);
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocity = direction * speed;
        velocity = MoveAndSlide(velocity);
        int slideCount = GetSlideCount();
        for(int i = 0; i < slideCount; i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            if(collision.Collider is Node)
            {
                if(((Node)collision.Collider).IsInGroup("Players"))
                {
                    //TODO: Add collission sound and animation(maybe)
                    ((Player)collision.Collider).TakeDamage(damage);
                }
            }

        }
    }
}
