using Godot;
using System;

public class ChooseGameMode : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _OnEndlessButtonPressed()
    {
        GetTree().ChangeScene("res://Game/EndlessMode.tscn");
    }

    public void _OnWIPButtonPressed()
    {
        GetTree().ChangeScene("res://Game/BingusMode.tscn");
    }

    public void _OnBackButtonPressed()
    {
        GetTree().ChangeScene("res://Game/Menu.tscn");
    }
}
