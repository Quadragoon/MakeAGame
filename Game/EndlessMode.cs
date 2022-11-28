using Godot;
using System;

public class EndlessMode : Game
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    

    WeightedGroup<string> group = new WeightedGroup<string>(){
        {"Survival", 50}, //Value indicates weight, i.e. chance to happen
        {"Slay", 0}, 
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        score = 0;
        var gameState = GetNode<GameState>("../GameState");
        level = gameState.level++;
        

        objectiveType = group.GetItem();
        switch(objectiveType)
        {
            case "Survival":
            GetNode<Timer>("SurvivalTimer").WaitTime = (60 + ((level-1)*5));
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
        GetTree().ReloadCurrentScene();
    }





//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(kills >= (50 + ((level-1)*5)))
        {
            GetTree().ReloadCurrentScene();
        }
    }
}
