using Godot;
using System;

public class Background : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var image = GD.Load<Image>("res://Assets/background/pog.png");
        var texture = new ImageTexture();
        texture.CreateFromImage(image);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
