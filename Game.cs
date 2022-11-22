using Godot;
using System;

public class Game : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]  
    public PackedScene MobScene;

    private Node2D player;
    private Path2D mobPath;
    private PathFollow2D mobSpawnLocation;

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
        
        Enemy newEnemy = MobScene.Instance<Enemy>();
        mobPath.Position = player.Position;
        mobSpawnLocation.Offset = GD.Randi(); //Random location on Path2D
        newEnemy.Position = player.GlobalPosition + mobSpawnOffset; //Set pos to random location
        AddChild(newEnemy);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();

        player = GetNode<Node2D>("Player");
        mobPath = GetNode<Path2D>("MobPath");
        mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }
}
