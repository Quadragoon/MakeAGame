using Godot;
using System;

public class Background : Sprite
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var image = GD.Load<Image>("res://Assets/background/pog.png");
        var texture = new ImageTexture();
        texture.CreateFromImage(image);
    }

}
