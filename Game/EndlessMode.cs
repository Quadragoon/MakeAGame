using Godot;
using System;

public class EndlessMode : Game
{
    private GameState gameState;
    private EndlessMode gameScene;
    private Label objective;
    private Timer mobTimer;
    
    private const int BASE_TIMER = 10;
    private const int BASE_KILLS = 5;
    private const int TIME_PER_LEVEL = 1;
    private const int KILLS_PER_LEVEL = 5;
    private bool offerUpgradeSwitch = true;
    public bool bossSwitch = false;
    
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
        gameState = GetNode<GameState>("/root/GameState");
        gameScene = GetNode<EndlessMode>("/root/EndlessMode");
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
        OfferUpgrade();
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if((kills >= (gameState.killTarget) && objectiveType == "Slay") && (level % 5 != 0) && offerUpgradeSwitch) 
        {
            offerUpgradeSwitch = false; //Toggle to prevent multiple offers
            level++;
            gameState.killTarget += (level*KILLS_PER_LEVEL);
            OfferUpgrade();
        }
        time = timer(delta,  time);
        if  (time > gameState.mobTimer && mobCounter < gameState.killTarget && offerUpgradeSwitch && !bossSwitch)
        {
            time = 0;
            mobCounter++;
            GD.Print(mobCounter);
            SpawnMobs();
        }
    }

    public void OfferUpgrade()
    {
        //Pause mob timer when upgrade is offered
        mobTimer.Paused = true;
        //Spawn upgrade capsule
        PackedScene capsuleScene = ResourceLoader.Load("res://Items/UpgradeCapsule.tscn") as PackedScene;
        UpgradeCapsule upgradeCapsule = (UpgradeCapsule)capsuleScene.Instance();
        upgradeCapsule.GlobalPosition = gameState.positionOfLastEnemyKilled;
        GetNode("/root/EndlessMode").AddChild(upgradeCapsule);
    }

    public void SpawnHealth()
    {
        PackedScene healthScene = ResourceLoader.Load("res://Items/HealthDrop.tscn") as PackedScene;
        HealthDrop healthDrop = (HealthDrop)healthScene.Instance();
        healthDrop.GlobalPosition = gameState.positionOfLastEnemyKilled;
        GetNode("/root/EndlessMode").GetNode("Items").AddChild(healthDrop);
    }
    public void DeleteChildren(Node node)
    {
        foreach(Node child in node.GetChildren())
        {
            child.QueueFree();
        }
    }

    public void NextLevel()
    {
        //mobTimer.Paused = false;
        //mobTimer.WaitTime /= 1.05f; //Faster enemy spawn rate
        if(GetNode("/root/EndlessMode").GetNode("Items").GetChildCount() > 0)
        {
            DeleteChildren(GetNode("/root/EndlessMode").GetNode("Items")); //Remove all items'
            //TODO: Add despawn animation for all items
        }
        gameState.mobTimer /= 1.05f;
        offerUpgradeSwitch = true; //Reset offerUpgradeSwitch
        if(level % 5 == 0) //Every 5th level spawn a boss. TODO: Add superbosses every 10th level?
        { 
            //mobTimer.Paused = true; //Pauses enemy spawning during bossfight
            bossSwitch = true;
            objectiveType = "Boss";
            //Spawn one additional boss every 5th level
            for(int i = 0; i < (level/5); i++)
            {
                SpawnBoss();
            }
        }
        else 
        {
            objectiveType = group.GetItem();
        }
    }

    public void SpawnBoss()
    {
        float mobSpawnAngle = (GD.Randf() * Mathf.Pi*2); //Random float between 0 and 2pi
        Vector2 mobSpawnOffset = Mathf.Polar2Cartesian(1000, mobSpawnAngle);
        string mobPath = bosses.GetItem();
        mobScene = ResourceLoader.Load(mobPath) as PackedScene;
        KinematicBody2D mob = mobScene.Instance() as KinematicBody2D;
        mob.Position = player.GlobalPosition + mobSpawnOffset;
        GetNode("/root/EndlessMode").GetNode("Bosses").AddChild(mob);
    }

    public void DeadBoss()
    { 
        //If there are no more bosses left, resume enemy spawning and offer upgrades
        if(GetNode("/root/EndlessMode").GetNode("Bosses").GetChildCount() == 1)
        {
            offerUpgradeSwitch = false;
            level++;
            bossSwitch = false;
            //TODO: Change to UpgradeScreen when items are implemented
            OfferUpgrade();
        }
    }
}
