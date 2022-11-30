using Godot;
using System;

public class Player : Node2D
{
    [Export]
    public PackedScene MissileScene;

    protected Vector2 movementVector;
    public Vector2 ScreenSize;
    public int Acceleration;
    
    private int maxSpeed;
    private Vector2 velocity;
    private float maxHealth;
    private float currentHealth;
    private bool invinsible = false;
    private AudioStreamPlayer2D engineAudioPlayer;

    public int BoostPower;
    public float BoostTime = 0.25f;
    public float BoostCooldown = 3.0f;

    private float currentBoostTimer = 0.0f;
    private float currentBoostCooldown = 0.0f;
    private Vector2 boostDirection;
    private AudioStreamPlayer boostReadyAudioPlayer;
    private Timer invisibilityTimer;
    private TextureProgress healthBar;
    private TextureProgress boostBar;
    private GameState gameState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        healthBar = GetNode<TextureProgress>("../HUD/HealthContainer/HealthBar");
        ScreenSize = GetViewportRect().Size;
        movementVector = Vector2.Zero;
        engineAudioPlayer = GetNode<AudioStreamPlayer2D>("EngineAudioPlayer");
        boostReadyAudioPlayer = GetNode<AudioStreamPlayer>("BoostReadyAudioPlayer");
        invisibilityTimer = GetNode<Timer>("InvisibilityTimer");
        boostBar = GetNode<TextureProgress>("../HUD/BoostContainer/BoostBar");

        UpdateAttributes();
    }

    public void UpdateAttributes()
    {
        Acceleration = gameState.acceleration;
        BoostPower = gameState.boostPower;
        maxHealth = gameState.maxHealth;
        currentHealth = maxHealth;
        healthBar.MaxValue = gameState.healthBarMax;
        healthBar.Value = healthBar.MaxValue;
        maxSpeed = (int)(Acceleration * 0.125f);
        
        GD.Print(gameState.healthBarLength);
        Vector2 newHealthBarSize = new Vector2(gameState.healthBarLength, 30);
        MarginContainer healthBarContainer = GetNode<MarginContainer>("/root/Game/HUD/HealthContainer");
        GD.Print(healthBar.RectSize);
        healthBarContainer.MarginRight = gameState.healthBarLength;
        // healthBar.RectSize = newHealthBarSize;
        GD.Print(healthBar.RectSize);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(currentHealth <= 0)
        {      
            ClearGameState();
            GetTree().ChangeScene("res://Game/Menu.tscn");
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

        engineAudioPlayer.PitchScale = Math.Max((velocity.Length() / maxSpeed) * 1.5f, 0.25f);

        this.GlobalPosition += velocity * delta;
        
        Vector2 mousePos = GetGlobalMousePosition();

        LookAt(mousePos);

        if (Input.IsActionJustPressed("fire"))
        {
            Missile newMissile = MissileScene.Instance<Missile>();
            newMissile.GlobalPosition = GlobalPosition;
            newMissile.Rotation = Rotation;
            GetParent().AddChild(newMissile);
        }

        if (Input.IsActionJustPressed("fire_secondary"))
        {
            SuperAttack();
        }

        if (currentBoostCooldown > 0)
        {
            currentBoostCooldown = (delta >= currentBoostCooldown) ? 0 : currentBoostCooldown-delta; // this is a way to make sure we don't count below 0
            boostBar.Value = 1 - (currentBoostCooldown / BoostCooldown);
            if (currentBoostCooldown == 0)
                boostReadyAudioPlayer.Play();
        }
        else if (Input.IsActionJustPressed("boost") && movementInput != Vector2.Zero)
        {
            boostBar.Value = 0;
            currentBoostCooldown = BoostCooldown;
            currentBoostTimer = BoostTime;
            boostDirection = movementInput.Normalized();
        }
    }

    public override void _Input(InputEvent inputEvent)
    {

    }

    public void _OnInvisibilityTimerTimeout()
    {
        invinsible = false;
    }

    public void TakeDamage(float damage)
    {
        if(!invinsible)
        {
            currentHealth -= damage;
            healthBar.Value = currentHealth;
            invinsible = true;
            invisibilityTimer.Start();
        }
    }

    private void SuperAttack()
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
    }

    public void ClearGameState()
    {
        var gameState = GetNode<GameState>("../../GameState");
        gameState.level = 1;
        gameState.score = 0;
        gameState.acceleration = 2500;
        gameState.boostPower = 5000;
        gameState.maxHealth = 5;
        gameState.healthBarMax = 5;
        gameState.healthBarLength = 150;
    }
}
