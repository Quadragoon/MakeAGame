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
    private ProgressBar healthBar;

    private Viewport viewport;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        currentHealth = maxHealth;
        ScreenSize = GetViewportRect().Size;
        movementVector = Vector2.Zero;
        maxSpeed = (int)(Acceleration * 0.125f);
        engineAudioPlayer = GetNode<AudioStreamPlayer2D>("EngineAudioPlayer");
        boostReadyAudioPlayer = GetNode<AudioStreamPlayer>("BoostReadyAudioPlayer");
        viewport = GetViewport();
        invisibilityTimer = GetNode<Timer>("InvisibilityTimer");
        healthBar = GetNode<ProgressBar>("../HUD/HealthBar");

        // viewport.GlobalCanvasTransform = viewport.GlobalCanvasTransform.Scaled(Vector2.One * 0.1f);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(currentHealth <= 0)
        {
            GD.Print("DEAD");
        }

        Vector2 movementInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        velocity += movementInput * Acceleration * delta;
        velocity -= velocity * 7.5f * delta;
        if (velocity.Length() < 0.5f)
            velocity = Vector2.Zero;

        if (currentBoostTimer > 0) // we're boosting!
        {
            velocity += BoostPower * boostDirection * delta;
            currentBoostTimer = Math.Max(currentBoostTimer - delta, 0); // this is a way to make sure we don't count below 0
        }

        engineAudioPlayer.PitchScale = Math.Max((velocity.Length() / maxSpeed) * 1.5f, 0.05f);

        this.GlobalPosition += velocity * delta;
        
        Vector2 mousePos = GetGlobalMousePosition();

        LookAt(mousePos);

        if (Input.IsActionJustPressed("fire"))
        {
            Missile newMissile = MissileScene.Instance<Missile>();
            newMissile.TargetLocation = viewport.GetMousePosition();
            newMissile.GlobalPosition = GlobalPosition;
            newMissile.Rotation = Rotation;
            GetParent().AddChild(newMissile);
        }

        if (currentBoostCooldown > 0)
        {
            currentBoostCooldown = (delta >= currentBoostCooldown) ? 0 : currentBoostCooldown-delta; // this is a way to make sure we don't count below 0
            if (currentBoostCooldown == 0)
                boostReadyAudioPlayer.Play();
        }
        else if (Input.IsActionJustPressed("boost") && movementInput != Vector2.Zero)
        {
            currentBoostCooldown = BoostCooldown;
            currentBoostTimer = BoostTime;
            boostDirection = movementInput.Normalized();
        }

        //Vector2 transformVector = GlobalPosition - (GetViewportRect().Size / 2);
        //Transform2D viewportTransform = viewport.GlobalCanvasTransform;
        //viewportTransform.origin = Vector2.Zero;
        //viewport.GlobalCanvasTransform = viewportTransform.Translated(transformVector * -1);
    }

    public override void _Input(InputEvent inputEvent)
    {

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
            healthBar.Value--;
            invinsible = true;
            invisibilityTimer.Start();
        }
    }
}
