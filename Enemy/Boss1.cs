using Godot;
using System;

public class Boss1 : EnemyBase
{
    private Vector2 direction = Vector2.Zero;
    private GameState gameState;
    private EndlessMode endlessMode;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("../../../GameState");
        endlessMode = GetNode<EndlessMode>("/root/EndlessMode");
        speed = 350 * (1.0f + (gameState.level-1.0f) * 0.04f); //Derived from EnemyBase
        health = 30 * (1.0f + (gameState.level-1.0f) * 2.0f);
        damage = 3 * (1.0f + (gameState.level-1.0f) * 0.5f);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Game game = GetNode<Game>("../../.");
        if(health <=0)
        {
            //Todo: Add on death animation, ADD BOSS UNIQUE ATTACKS
            game.score+=10000;
            //game.kills++;
            endlessMode.DeadBoss();
            QueueFree();
        }
        Node2D player = GetNode<Node2D>("../../Player");
        direction = GlobalPosition.DirectionTo(player.GlobalPosition);
        LookAt(player.GlobalPosition);
    }

    public override void _PhysicsProcess(float delta)
    {
        //Change velocity based on distance to Player
        if(GlobalPosition.DistanceTo(GetNode<Node2D>("../../Player").GlobalPosition) > 1800)
        {
            speed = 350 + GlobalPosition.DistanceTo(GetNode<Node2D>("../../Player").GlobalPosition * (1.0f + (gameState.level-1.0f) * 0.05f));
        }
        else
        {
            speed = 350 * (1.0f + (gameState.level-1.0f) * 0.05f);
        }
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

    public void BossAttack()
    {
        
    }
}
