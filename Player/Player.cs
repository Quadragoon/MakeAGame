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
    private AudioStreamPlayer superAttackReadyAudioPlayer;
    private Timer invisibilityTimer;
    private TextureProgress healthBar;
    private TextureProgress boostBar;
    private GameState gameState;
    private Random rand = new Random();
    private float fireDelay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        healthBar = GetNode<TextureProgress>("/root/EndlessMode/HUD/BarContainer/HealthBar");
        ScreenSize = GetViewportRect().Size;
        movementVector = Vector2.Zero;
        engineAudioPlayer = GetNode<AudioStreamPlayer2D>("EngineAudioPlayer");
        boostReadyAudioPlayer = GetNode<AudioStreamPlayer>("BoostReadyAudioPlayer");
        superAttackReadyAudioPlayer = GetNode<AudioStreamPlayer>("SuperAttackReadyAudioPlayer");
        invisibilityTimer = GetNode<Timer>("InvisibilityTimer");
        boostBar = GetNode<TextureProgress>("/root/EndlessMode/HUD/BarContainer/BoostBar");
        
        UpdateAttributes();
    }

    public void UpdateAttributes()
    {
        Acceleration = gameState.acceleration;
        BoostPower = gameState.boostPower;
        maxHealth = gameState.maxHealth;
        currentHealth = maxHealth;
        healthBar.MaxValue = gameState.maxHealth;
        healthBar.Value = healthBar.MaxValue;
        maxSpeed = (int)(Acceleration * 0.125f);
        BoostCooldown = gameState.boostCooldown;
        
        Control barSpacing = GetNode<Control>("/root/EndlessMode/HUD/BarContainer/BarSpacing");
        healthBar.SizeFlagsStretchRatio = Mathf.Clamp(0.2f * gameState.maxHealth, 1, 6);
        barSpacing.SizeFlagsStretchRatio = Mathf.Clamp(6 - (0.2f * gameState.maxHealth), 0, 5);
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

        if (Input.IsActionPressed("fire") && fireDelay <= 0f)
        {
            fireDelay = gameState.fireDelay;
            bool stormtrooper = gameState.stormtrooper;
            float fireangle = 0.2f;
            int extraMissilesFired=0;

            

            //Check how many extra missiles will be fired 
            for (int i = 0; i < gameState.missileMultiplierChance/100; i++){
                if((rand.Next(0, 100)) < gameState.missileMultiplierChance - 100*i)
                { 
                    extraMissilesFired++; 
                }
            }

            //Fire extra missiles with different angles depending on if even or odd
            for (int i = 0; i < extraMissilesFired; i++){
                int isEven = 0;
                if (extraMissilesFired%2 == 1){
                    isEven = 1;
                }
                Missile additionalMissile = MissileScene.Instance<Missile>();
                additionalMissile.GlobalPosition = GlobalPosition;

                if(stormtrooper){
                fireangle = rand.Next(-15,15);
                fireangle /= 10f;
                }
                
                //TODO: Make nicer pairs in the middle
                if(i%2 == 0){
                    additionalMissile.Rotation = Rotation + fireangle * (1 + (isEven + i)/2);
                } else{
                    additionalMissile.Rotation = Rotation - fireangle * (1 + (isEven + i)/2);
                }
                    GetParent().AddChild(additionalMissile);
            }

            //fire the last/first missile in the middle or to the side depending on if even or odd
            if(extraMissilesFired%2 == 0 && !stormtrooper){

                fireangle=0.0f;
            }
            
            if(stormtrooper){
                fireangle = rand.Next(-15,15);
                fireangle /= 10f;
            }
            Missile newMissile = MissileScene.Instance<Missile>();
            newMissile.GlobalPosition = GlobalPosition;
            newMissile.Rotation = Rotation - fireangle;
            GetParent().AddChild(newMissile);

            

        }
        else
        {
            fireDelay-=delta;
        }

        if (Input.IsActionJustPressed("fire_secondary"))
        {   
            if(gameState.superAttackCooldown <= 0f)
            {
                gameState.superAttackCooldown = gameState.superAttackCooldownMax;
                SuperAttack();
            }
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

        //TODO: add unique bar and cooldown sound to superattack
        if (gameState.superAttackCooldown > 0)
        {
            gameState.superAttackCooldown = (delta >= gameState.superAttackCooldown) ? 0 : gameState.superAttackCooldown-delta; // this is a way to make sure we don't count below 0
            //boostBar.Value = 1 - (currentBoostCooldown / BoostCooldown);
            if (gameState.superAttackCooldown == 0)
                superAttackReadyAudioPlayer.Play();
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
        Vector2 targetLocation = GetGlobalMousePosition();
        PackedScene targetedMissileScene = GD.Load<PackedScene>("res://Player/MissileTargeted.tscn");
        PackedScene launcherScene = GD.Load<PackedScene>("res://Player/Launcher.tscn");
        Launcher launcher = launcherScene.Instance<Launcher>();
        launcher.Init(targetedMissileScene, 0.01f, targetLocation, 30, launchAngleOffset: Mathf.Pi);
        AddChild(launcher);
    }

    public void ClearGameState()
    {
        var gameState = GetNode<GameState>("../../GameState");
        gameState.ClearGameState();
    }
}
