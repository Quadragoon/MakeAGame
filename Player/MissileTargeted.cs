using Godot;
using System;

public class MissileTargeted : Missile
{
    private float timer = 0.0f;
    public float fireDelay;
    public float freeFloatTime = .75f;
    public Vector2 TargetLocation;
    private Sprite missileSprite;
    private float freeFloatRotationSpeed;
    private float defaultSpriteRotation;
    private float turnSpeed = 4f;
    private float acceleration = 400f;
    public Node2D firedFrom;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Visible = false;
        Speed = 400;
        missileSprite = GetNode<Sprite>("Sprite");
        defaultSpriteRotation = missileSprite.Rotation;
        freeFloatRotationSpeed = (GD.Randf() - 0.5f) * 50;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (fireDelay > 0)
        {
            fireDelay -= Mathf.Min(delta, fireDelay);
            freeFloatTime -= Mathf.Min(delta, freeFloatTime);
            if (fireDelay <= 0)
            {
                Visible = true;
                GlobalPosition = firedFrom.GlobalPosition;
                TargetLocation = GetGlobalMousePosition();
            }
        }
        else if (freeFloatTime > 0)
        {
            Speed -= Mathf.Sqrt(Speed) * delta * 25;
            freeFloatTime -= Mathf.Min(delta, freeFloatTime);
            missileSprite.Rotate(freeFloatRotationSpeed * delta);
            GlobalPosition += new Vector2((float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation)) * Speed/2 * delta;
            if (freeFloatTime <= 0)
            {
                Rotation = missileSprite.Rotation;
                missileSprite.Rotation = defaultSpriteRotation;
            }
        }
        else
        {
            Speed += acceleration * delta;
            Rotate((GetAngleTo(TargetLocation) < 0 ? -1 : 1) * turnSpeed * delta);
            timer += delta;
            GlobalPosition += new Vector2((float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation)) * Speed * delta;
            if (timer >= 2.0f || GlobalPosition.DistanceTo(TargetLocation) < 5)
            {
                Explode();
            }
        }
    }
}
