using Godot;
using System;

public class Game : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    WeightedGroup<string> group = new WeightedGroup<string>(){
        {"res://Enemy1.tscn", 10}, //Value indicates weight, i.e. chance to spawn
        {"res://Enemy2.tscn", 30}, 
        {"res://Enemy3.tscn", 50}
    };

    public int score = 0;

    [Export]  
    public PackedScene MobScene;

    private Node2D player;

    private Random rnd = new Random();
    

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

        string monsterPath = group.GetItem();
        PackedScene monsterScene = ResourceLoader.Load(monsterPath) as PackedScene;
        KinematicBody2D monster = monsterScene.Instance() as KinematicBody2D;
        monster.Position = player.GlobalPosition + mobSpawnOffset;
        AddChild(monster);

        /*switch(rnd.Next(3))
        {
            case 0:
            GD.Print("POG");
            Enemy1 newEnemy1 = (Enemy1)MobScene.Instance<Enemy1>();
            newEnemy1.Position = player.GlobalPosition + mobSpawnOffset; //Set pos to random location
            AddChild(newEnemy1);
            break;

            case 1:
            Enemy2 newEnemy2 = (Enemy2)MobScene.Instance<Enemy2>();
            newEnemy2.Position = player.GlobalPosition + mobSpawnOffset; //Set pos to random location
            AddChild(newEnemy2);
            break;

            case 2:
            Enemy3 newEnemy3 = (Enemy3)MobScene.Instance<Enemy3>();
            newEnemy3.Position = player.GlobalPosition + mobSpawnOffset; //Set pos to random location
            AddChild(newEnemy3);
            break;
        }*/
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
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
