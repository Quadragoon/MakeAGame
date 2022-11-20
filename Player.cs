using Godot;
using System;

public class Player : Node2D
{
    [Export]
    public PackedScene MissileScene;

    protected Vector2 movementVector;
    public Vector2 ScreenSize;
    public int Acceleration = 5000;
    
    private int maxSpeed;
    private Vector2 velocity;
    private AudioStreamPlayer2D engineAudioPlayer;

    private Viewport viewport;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        movementVector = Vector2.Zero;
        maxSpeed = (int)(Acceleration * 0.125f);
        engineAudioPlayer = GetNode<AudioStreamPlayer2D>("EngineAudioPlayer");
        viewport = GetViewport();
        // viewport.GlobalCanvasTransform = viewport.GlobalCanvasTransform.Scaled(Vector2.One * 0.1f);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 movementInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        velocity += movementInput * Acceleration * delta;
        velocity -= velocity * 7.5f * delta;

        engineAudioPlayer.PitchScale = Math.Max((velocity.Length() / maxSpeed) * 1.5f, 0.05f);

        this.GlobalPosition += velocity * delta;

        Vector2 mousePos = viewport.GetMousePosition();

        LookAt(mousePos);

        if (Input.IsActionJustPressed("fire"))
        {
            Missile newMissile = MissileScene.Instance<Missile>();
            newMissile.TargetLocation = viewport.GetMousePosition();
            newMissile.GlobalPosition = GlobalPosition;
            newMissile.Rotation = Rotation;
            GetParent().AddChild(newMissile);
        }

        Vector2 transformVector = GlobalPosition - (GetViewportRect().Size / 2);
        Transform2D viewportTransform = viewport.GlobalCanvasTransform;
        viewportTransform.origin = Vector2.Zero;
        viewport.GlobalCanvasTransform = viewportTransform.Translated(transformVector * -1);
    }

    public override void _Input(InputEvent inputEvent)
    {

    }
}
