using Godot;
using System;

public class EndlessMode : Game
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    private GameState gameState;
    private Game gameScene;
    private Label objective;
    private Timer mobTimer;
    

    private const int BASE_TIMER = 10;
    private const int BASE_KILLS = 5;
    private const int TIME_PER_LEVEL = 1;
    private const int KILLS_PER_LEVEL = 5;
    

    WeightedGroup<string> group = new WeightedGroup<string>(){
        {"Survival", 0}, //Value indicates weight, i.e. chance to happen
        {"Slay", 50}, 
    };

    WeightedGroup<string> bosses = new WeightedGroup<string>(){
        {"res://Enemy/Boss1.tscn", 50}, //Value indicates weight, i.e. chance to spawn
        
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("../GameState");
        gameScene = GetNode<Game>("../Game");
        objective = GetNode<Label>("HUD/Objective");
        mobTimer = GetNode<Timer>("MobTimer");

        level = 1;
        //score = gameState.score;
        //level = gameState.level++;
        

        objectiveType = group.GetItem();
        if(objectiveType == "Survival")
        {
            GetNode<Timer>("SurvivalTimer").WaitTime = BASE_TIMER + (level*TIME_PER_LEVEL);
            GetNode<Timer>("SurvivalTimer").Start();
        }

        base._Ready();
    }

    public void _OnSurvivalTimerTimeout()
    {
        //TODO: Add end of level animation -> maybe animate scene transition
        OfferUpgrade();
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(((kills >= (BASE_KILLS + (level*KILLS_PER_LEVEL))) && objectiveType == "Slay") && level % 5 != 0) 
        {
            //TODO: Add end of level animation -> maybe animate scene transition
            gameState.deadBoss = false;
            OfferUpgrade();
        }
        else if(gameState.deadBoss)
        {
            gameState.deadBoss = false;
            OfferUpgrade(); //TODO: Offer unique rewards
        }
    }

    public void OfferUpgrade()
    {
        //gameState.score = gameScene.score;
        // GetTree().ChangeScene("res://Game/UpgradeScreen.tscn");
        mobTimer.WaitTime /= 1.05f;
        Control upgradeScreen = GD.Load<PackedScene>("res://Game/UpgradeScreen.tscn").Instance() as Control;
        upgradeScreen.ShowOnTop = true;
        CanvasLayer hud = gameScene.GetNode<CanvasLayer>("HUD");
        objective.Visible = false;
        upgradeScreen.PauseMode = PauseModeEnum.Process;
        gameState.upgradeMode = true; //Makes sure you can't pause in the upgradescreen //TODO: make this better
        hud.AddChild(upgradeScreen);
        GetTree().Paused = true;
    }

    public void NextLevel()
    {
        level++;
        kills = 0;
        if(level % 5 == 0)
        {
            //TODO: Add boss
            objectiveType = "Boss";
            float mobSpawnAngle = (GD.Randf() * Mathf.Pi*2); //Random float between 0 and 2pi
            Vector2 mobSpawnOffset = Mathf.Polar2Cartesian(1000, mobSpawnAngle);

            string mobPath = bosses.GetItem();
            mobScene = ResourceLoader.Load(mobPath) as PackedScene;
            KinematicBody2D mob = mobScene.Instance() as KinematicBody2D;
            mob.Position = player.GlobalPosition + mobSpawnOffset;
            AddChild(mob);
        }
        else 
        {
            objectiveType = group.GetItem();
            if(objectiveType == "Survival")
                {
                    GetNode<Timer>("SurvivalTimer").WaitTime = BASE_TIMER + (level*TIME_PER_LEVEL);
                    GetNode<Timer>("SurvivalTimer").Start();
                }
        }
        
            
    }
}
