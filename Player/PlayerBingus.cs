using Godot;
using System;

public class PlayerBingus : Node2D
{
    [Export]
    public PackedScene MissileScene;

    protected Vector2 movementVector;
    public Vector2 ScreenSize;
    public int Acceleration = 5000;
    
    private int maxSpeed;
    private Vector2 velocity;
    private int maxHealth = 5;
    private int currentHealth;
    private bool invinsible = false;
    private AudioStreamPlayer2D engineAudioPlayer;

    public int BoostPower = 10000;
    public float BoostTime = 0.25f;
    public float BoostCooldown = 3.0f;

    private float currentBoostTimer = 0.0f;
    private float currentBoostCooldown = 0.0f;
    private Vector2 boostDirection;
    private AudioStreamPlayer boostReadyAudioPlayer;
    private Timer invisibilityTimer;
    private TextureProgress healthBar;
    private TextureProgress boostBar;
    private Vector2 _movementInput;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        currentHealth = maxHealth;
        ScreenSize = GetViewportRect().Size;
        movementVector = Vector2.Zero;
        maxSpeed = (int)(Acceleration * 0.125f);
        engineAudioPlayer = GetNode<AudioStreamPlayer2D>("EngineAudioPlayer");
        boostReadyAudioPlayer = GetNode<AudioStreamPlayer>("BoostReadyAudioPlayer");
        invisibilityTimer = GetNode<Timer>("InvisibilityTimer");
        healthBar = GetNode<TextureProgress>("../HUD/HealthContainer/HealthBar");
        boostBar = GetNode<TextureProgress>("../HUD/BoostContainer/BoostBar");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(currentHealth <= 0)
        {
            // TODO: ded
        }

        engineAudioPlayer.PitchScale = Math.Max((velocity.Length() / maxSpeed) * 1.5f, 0.25f);

        if (IsNetworkMaster())
        {
            _movementInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            Vector2 mousePos = GetGlobalMousePosition();
            LookAt(mousePos);

            if (Input.IsActionJustPressed("fire"))
            {
                FireMissile();
                Rpc("FireMissile");
            }

            if (currentBoostCooldown > 0)
            {
                currentBoostCooldown = (delta >= currentBoostCooldown) ? 0 : currentBoostCooldown-delta; // this is a way to make sure we don't count below 0
                boostBar.Value = 1 - (currentBoostCooldown / BoostCooldown);
                if (currentBoostCooldown == 0)
                    boostReadyAudioPlayer.Play();
            }
            else if (Input.IsActionJustPressed("boost") && _movementInput != Vector2.Zero)
            {
                boostBar.Value = 0;
                currentBoostCooldown = BoostCooldown;
                currentBoostTimer = BoostTime;
                boostDirection = _movementInput.Normalized();
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity += _movementInput * Acceleration * delta;
        velocity -= velocity * 7.5f * delta;
        if (velocity.Length() < 0.5f)
            velocity = Vector2.Zero;

        if (currentBoostTimer > 0) // we're boosting!
        {
            velocity += BoostPower * boostDirection * delta;
            currentBoostTimer = Math.Max(currentBoostTimer - delta, 0); // this is a way to make sure we don't count below 0
        }

        GlobalPosition += velocity * delta;
        if (IsNetworkMaster())
            Rpc("UpdateNetworkData", GlobalPosition, velocity, Rotation);
    }

    [Puppet]
    private void FireMissile()
    {
        Missile newMissile = MissileScene.Instance<Missile>();
        newMissile.GlobalPosition = GlobalPosition;
        newMissile.Rotation = Rotation;
        GetParent().AddChild(newMissile);
    }

    [Puppet]
    private void UpdateNetworkData(Vector2 newPosition, Vector2 newVelocity, float newRotation)
    {
        GlobalPosition = newPosition;
        velocity = newVelocity;
        Rotation = newRotation;
    }

    public void _OnInvisibilityTimerTimeout()
    {
        invinsible = false;
    }

    public void TakeDamage(int damage)
    {
        if(!invinsible)
        {
            currentHealth -= damage;
            healthBar.Value = currentHealth;
            invinsible = true;
            invisibilityTimer.Start();
        }
    }

    /*private void SuperAttack()
    {
        Vector2 targetedLocation = GetGlobalMousePosition();
        PackedScene targetedMissileScene = GD.Load<PackedScene>("res://Player/MissileTargeted.tscn");
        for (int i = 0; i < 30; i++)
        {
            float targetAreaRadius = 50.0f;
            MissileTargeted newMissile = targetedMissileScene.Instance<MissileTargeted>();
            newMissile.GlobalPosition = GlobalPosition;
            newMissile.Rotation = Rotation;
            newMissile.Rotate(Mathf.Pi);
            newMissile.Rotate(GD.Randf() - 0.5f);
            newMissile.TargetLocation = targetedLocation + Mathf.Polar2Cartesian((float)GD.RandRange(5, targetAreaRadius), (float)GD.RandRange(0, Mathf.Tau));
            newMissile.fireDelay = 0.01f * (i + 1);
            newMissile.firedFrom = this;
            GetParent().AddChild(newMissile);
        }
    }*/
}
