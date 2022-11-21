using Godot;
using System;

public class Game : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]  
    public PackedScene MobScene;

    public void _NewGame()
    {
        
    }

    public void _GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
    }

    public void _OnMobTimerTimeout()
    {
        Enemy newEnemy = MobScene.Instance<Enemy>();
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Offset = GD.Randi(); //Random location on Path2D
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2; //Direction perpendicular to the path direction
        newEnemy.Position = mobSpawnLocation.Position; //Set pos to random location
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4); //Add some randomness to the enemy direction
        newEnemy.Rotation = direction;
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        LookAt(GlobalPosition);
        AddChild(newEnemy);

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }
}
