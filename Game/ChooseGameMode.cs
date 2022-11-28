using Godot;
using System;

public class ChooseGameMode : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _OnEndlessButtonPressed()
    {
        GetTree().ChangeScene("res://Game/Game.tscn");
    }

    public void _OnBackButtonPressed()
    {
        GetTree().ChangeScene("res://Game/Menu.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
