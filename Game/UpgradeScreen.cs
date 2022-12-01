using Godot;
using System;

class UpgradeOption
{
    public string Name {get;}
    public string Rarity {get;}
    public bool Cursed {get;}
    public UpgradeOption(string name, string rarity, bool cursed = false)
    {
        Name = name;
        Rarity = rarity;
        Cursed = cursed;
    }
}

public class UpgradeScreen : Control
{
    private GameState gameState;
    private Player player;
    private EndlessMode endlessMode;
    private UpgradeOption option1;
    private UpgradeOption option2;
    private UpgradeOption option15;
    private Button button1;
    private Button button2;
    private Button button15;
    private Label objective;
    
    WeightedGroup<UpgradeOption> group = new WeightedGroup<UpgradeOption>(){
        {new UpgradeOption("Health", "Common"), 50}, 
        {new UpgradeOption("Speed", "Common"), 50}, 
        {new UpgradeOption("Damage", "Uncommon"), 25}, 
        {new UpgradeOption("Attack Speed", "Uncommon"), 25},
        {new UpgradeOption("Additional Missile", "Extraordinary"), 500}, 
        {new UpgradeOption("Explosion Radius", "Rare"), 10}, 
        {new UpgradeOption("Boost Cooldown", "Rare"), 10}, 

    };
    
    //TODO: Add random upgrades
    //Upgrade health, speed, boostbar in some way, damage
    //Add different rarities to them

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<Player>("/root/Game/Player");
        gameState = GetNode<GameState>("/root/GameState");
        endlessMode = GetNode<EndlessMode>("/root/Game");
        button1 = GetNode<Button>("OptionContainer/Option1");
        button2 = GetNode<Button>("OptionContainer/Option2");
        button15 = GetNode<Button>("OptionContainer/Option15");
        objective = GetNode<Label>("../Objective");
        
        GenerateUpgrades();
    }

    public void _OnOption1Pressed()
    {
        ApplyUpgrade(option1);
        ReturnToGame();
    }

    public void _OnOption2Pressed()
    {
        ApplyUpgrade(option2);
        ReturnToGame();
    }

    public void _OnOption15Pressed()
    {
        ApplyUpgrade(option15);
        ReturnToGame();
    }

    private void ReturnToGame()
    {
        player.UpdateAttributes();
        endlessMode.NextLevel();
        objective.Visible = true;
        gameState.upgradeMode = false; //TODO: fix this!" !!!#
        GetTree().Paused = false;
        QueueFree();
    }

    private void GenerateUpgrades()
    {
        option1 = group.GetItem();
        group.Remove(option1);
        option2 = group.GetItem();
        group.Remove(option2);
        option15 = group.GetItem();

        button1.Text = option1.Name;
        button1.Theme = ResourceLoader.Load("res://Assets/FlatUI/" + option1.Rarity + ".tres") as Theme;

        button2.Text = option2.Name;
        button2.Theme = ResourceLoader.Load("res://Assets/FlatUI/" + option2.Rarity + ".tres") as Theme;

        button15.Text = option15.Name;
        button15.Theme = ResourceLoader.Load("res://Assets/FlatUI/" + option15.Rarity + ".tres") as Theme;

    }

    private void ApplyUpgrade(UpgradeOption option)
    {
        switch(option.Name)
        {
            case "Health":
                gameState.UpgradeHealth();
            break;

            case "Speed":
                gameState.UpgradeSpeed();
            break;

            case "Damage":
                gameState.UpgradeDamage();
            break;

            case "Attack Speed":
                gameState.UpgradeAttackSpeed();
            break;

            case "Additional Missile":
                gameState.UpgradeAdditionalMissileChance();
            break;

            case "Explosion Radius":
                gameState.UpgradeExplosionRadius();
            break;

            case "Boost Cooldown":
                gameState.UpgradeBoostCooldown();
            break;
        }
        //TODO: Apply upgrades to character
    }

    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
