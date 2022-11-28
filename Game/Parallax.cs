using Godot;
using System;

public class Parallax : ParallaxBackground
{
    private Viewport viewport;
    private Sprite backgroundSprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        viewport = GetViewport();
        backgroundSprite = GetNode<Sprite>("ParallaxLayer/Background");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 viewportOrigin = viewport.GlobalCanvasTransform.origin * -1;
        float originX = viewportOrigin.x;
        float originY = viewportOrigin.y;

        originX = Mathf.Round(originX/1024);
        originY = Mathf.Round(originY/1024);
        float posX = (originX)*1024;
        float posY = (originY)*1024;

        backgroundSprite.Position = new Vector2(posX, posY);
        // GD.Print(backgroundSprite.Position);
    }
}
