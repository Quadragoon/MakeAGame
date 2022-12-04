using Godot;
using System;

public class Game : Node2D
{
    WeightedGroup<string> group = new WeightedGroup<string>(){
        {"res://Enemy/Enemy1.tscn", 10}, //Value indicates weight, i.e. chance to spawn
        {"res://Enemy/Enemy2.tscn", 30}, 
        {"res://Enemy/Enemy3.tscn", 50}
    };

    public int score;
    public int level;
    public int kills;
    public string objectiveType;

    [Export]  
    public PackedScene mobScene;
    public GameState gameState;
    public Timer mobTimer;

    protected Node2D player;
    protected int mobCounter = 0;

    //private Random rnd = new Random();
    
    protected float time = 0;

    protected float timer(float delta, float a)
    {
        return a += 1 * delta;
    }

    public void _NewGame()
    {
        
    }

    public void _GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
    }

    // public void _OnMobTimerTimeout()
    // {
    //         float mobSpawnAngle = (GD.Randf() * Mathf.Pi*2); //Random float between 0 and 2pi
    //         Vector2 mobSpawnOffset = Mathf.Polar2Cartesian(1000, mobSpawnAngle);

    //         string mobPath = group.GetItem();
    //         mobScene = ResourceLoader.Load(mobPath) as PackedScene;
    //         KinematicBody2D mob = mobScene.Instance() as KinematicBody2D;
    //         mob.Position = player.GlobalPosition + mobSpawnOffset;
    //         GetNode<Node2D>("Mobs").AddChild(mob);
    //         GD.Print("Mob spawned");
    // }
    public void SpawnMobs()
    {
            float mobSpawnAngle = (GD.Randf() * Mathf.Pi*2); //Random float between 0 and 2pi
            Vector2 mobSpawnOffset = Mathf.Polar2Cartesian(1000, mobSpawnAngle);

            string mobPath = group.GetItem();
            mobScene = ResourceLoader.Load(mobPath) as PackedScene;
            KinematicBody2D mob = mobScene.Instance() as KinematicBody2D;
            mob.Position = player.GlobalPosition + mobSpawnOffset;
            GetNode<Node2D>("Mobs").AddChild(mob);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        mobTimer = GetNode<Timer>("MobTimer");
        
        GD.Randomize();
        
        for(int i = 0; i < GetChildCount(); i++)
        {
            string monsterPath = group.GetItem();
            PackedScene monsterScene = ResourceLoader.Load(monsterPath) as PackedScene;
            KinematicBody2D monster = monsterScene.Instance() as KinematicBody2D;
        }

        player = GetNode<Node2D>("Player");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }   
}
