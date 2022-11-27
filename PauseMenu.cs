using Godot;
using System;

public class PauseMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel")) //Pause game function, ui_cancel bound to esc
        {
                this.Visible = true;
                GetTree().Paused = true;
        }
    }

    public void _OnResumeButtonPressed()
    {
        GetTree().Paused = false;
        Visible = false;
    }

    public void _OnQuitButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Menu.tscn");
    }
}
