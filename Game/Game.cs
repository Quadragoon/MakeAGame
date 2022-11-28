using Godot;
using System;

public class Game : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
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

    protected Node2D player;

    //private Random rnd = new Random();
    

    public void _NewGame()
    {
        
    }

    public void _GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
    }

    public void _OnMobTimerTimeout()
    {
        float mobSpawnAngle = (GD.Randf() * Mathf.Pi*2); //Random float between 0 and 2pi
        Vector2 mobSpawnOffset = Mathf.Polar2Cartesian(1000, mobSpawnAngle);

        string mobPath = group.GetItem();
        mobScene = ResourceLoader.Load(mobPath) as PackedScene;
        KinematicBody2D mob = mobScene.Instance() as KinematicBody2D;
        mob.Position = player.GlobalPosition + mobSpawnOffset;
        AddChild(mob);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        score = 0;
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
