using Godot;
using System;

public class EndlessMode : Game
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    

    WeightedGroup<string> group = new WeightedGroup<string>(){
        {"Survival", 50}, //Value indicates weight, i.e. chance to happen
        {"Slay", 50}, 
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        var gameState = GetNode<GameState>("../GameState");
        score = gameState.score;
        level = gameState.level++;
        

        objectiveType = group.GetItem();
        switch(objectiveType)
        {
            case "Survival":
            GetNode<Timer>("SurvivalTimer").WaitTime = (20 + ((level-1)*5));
            GetNode<Timer>("SurvivalTimer").Start();
            break;
            /*
            case "Slay":

            break;*/
        }

        base._Ready();
    }

    public void _OnSurvivalTimerTimeout()
    {
        //TODO: Add end of level animation -> maybe animate scene transition
        var gameState = GetNode<GameState>("../GameState");
        var gameScene = GetNode<Game>("../Game");
        gameState.score = gameScene.score;
        GetTree().ChangeScene("res://Game/UpgradeScreen.tscn");
    }





//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if((kills >= (10 + ((level-1)*5))) && objectiveType == "Slay")
        {
            //TODO: Add end of level animation -> maybe animate scene transition
            var gameState = GetNode<GameState>("../GameState");
            var gameScene = GetNode<Game>("../Game");
            gameState.score = gameScene.score;
            GetTree().ChangeScene("res://Game/UpgradeScreen.tscn");
        }
    }
}
