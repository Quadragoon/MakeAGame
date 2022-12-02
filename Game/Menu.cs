using Godot;
using System;

public class Menu : Control
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _OnStartButtonPressed()
    {
        GetTree().ChangeScene("res://Game/ChooseGameMode.tscn");
    }

    public void _OnOptionsButtonPressed()
    {

    }

    public void _OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
