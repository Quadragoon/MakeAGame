using Godot;
using System;

public class MissileTargeted : Missile
{
    private float timer = 0.0f;
    public float fireDelay;
    private float freeFloatTime = 0.25f;
    public Vector2 TargetLocation;
    private Sprite missileSprite;
    private float freeFloatRotationSpeed;
    private float defaultSpriteRotation;
    private float turnSpeed = 10f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Visible = false;
        Speed = 500;
        missileSprite = GetNode<Sprite>("Sprite");
        defaultSpriteRotation = missileSprite.Rotation;
        freeFloatRotationSpeed = (GD.Randf() + 0.1f) * 10;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (fireDelay > 0)
        {
            fireDelay -= Mathf.Min(delta, fireDelay);
        }
        else if (freeFloatTime > 0)
        {
            Visible = true;
            freeFloatTime -= Mathf.Min(delta, freeFloatTime);
            missileSprite.Rotate(freeFloatRotationSpeed * delta);
            GlobalPosition += new Vector2((float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation)) * Speed/2 * delta;
        }
        else
        {
            Visible = true;
            missileSprite.Rotation = defaultSpriteRotation;
            Rotate(GetAngleTo(TargetLocation) * turnSpeed * delta);
            timer += delta;
            GlobalPosition += new Vector2((float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation)) * Speed * delta;
            if (timer >= 2.0f || GlobalPosition.DistanceTo(TargetLocation) < 5)
            {
                Explode();
            }
        }
    }
}
