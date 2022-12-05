using Godot;
using System;

public class UpgradeCapsule : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    // WeightedGroup<UpgradeOption> group = new WeightedGroup<UpgradeOption>(){
    //     {new UpgradeOption("Health", "Common"), 100}, 
    //     {new UpgradeOption("Speed", "Common"), 100}, 
    //     {new UpgradeOption("Damage", "Epic"), 20}, 
    //     {new UpgradeOption("Attack Speed", "Epic"), 20},
    //     {new UpgradeOption("Additional Missile", "Extraordinary"), 10}, 
    //     {new UpgradeOption("Explosion Radius", "Rare"), 50}, 
    //     {new UpgradeOption("Boost Cooldown", "Uncommon"), 75},
    //     {new UpgradeOption("Large damage boost at a cost", "Cursed", true), 1},
    // };
    WeightedGroup<UpgradeOption> group = new WeightedGroup<UpgradeOption>(){
        {new UpgradeOption("Health", "Common"), 100}, 
        {new UpgradeOption("Speed", "Common"), 100}, 
        {new UpgradeOption("Damage", "Epic"), 100}, 
        {new UpgradeOption("Attack Speed", "Epic"), 100},
        {new UpgradeOption("Additional Missile", "Extraordinary"), 100}, 
        {new UpgradeOption("Explosion Radius", "Rare"), 100}, 
        {new UpgradeOption("Boost Cooldown", "Uncommon"), 100},
        {new UpgradeOption("Large damage boost at a cost", "Cursed", true), 100},
    };
    
    private GameState gameState;
    private Player player;
    private EndlessMode endlessMode;
    private AnimationPlayer animationPlayer;
    private Label upgradeLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        player = GetNode<Player>("/root/EndlessMode/Player");
        endlessMode = GetNode<EndlessMode>("/root/EndlessMode");
        animationPlayer = GetNode<AnimationPlayer>("/root/EndlessMode/HUD/UpgradeAnimation");
        upgradeLabel = GetNode<Label>("/root/EndlessMode/HUD/UpgradeContainer/UpgradeText");
    }

    public void _OnUpgradeCapsuleBodyEntered(Node body)
    {
        ApplyUpgrade(group.GetItem());
        ReturnToGame();
    }

    private void ReturnToGame()
    {
        player.UpdateAttributes();
        endlessMode.NextLevel();
        QueueFree();
    }

    private void ApplyUpgrade(UpgradeOption option)
    {   
        //TODO: Add upgrade animation
        //TODO: A label that shows the upgrade, theme depending on rarity
        upgradeLabel.Text = option.Name;
        switch(option.Rarity)
        {
            case "Uncommon":
                upgradeLabel.AddColorOverride("font_color", new Color(26f, 255f, 0f)); //Green
                break;
            case "Rare":
                upgradeLabel.AddColorOverride("font_color", new Color(0f, 88f, 255f)); //Blue
                break;
            case "Epic":
                upgradeLabel.AddColorOverride("font_color", new Color(151f, 0f, 255f)); //Purple
                break;
            case "Extraordinary":
                upgradeLabel.AddColorOverride("font_color", new Color(255f, 96f, 0f)); //Orange
                break;
            case "Cursed":
                upgradeLabel.AddColorOverride("font_color", new Color(150f, 0f, 0f)); //Red
                break;
            default:
                upgradeLabel.AddColorOverride("font_color", new Color(255f, 255f, 255f)); //White
                break;
        }
        //upgradeLabel.Theme = ResourceLoader.Load("res://Assets/FlatUI/" + option.Rarity + ".tres") as Theme;
        animationPlayer.Play("UpgradeAnim");

        switch(option.Rarity)
        {
            case "Common":
                switch(option.Name)
                {
                    case "Health":
                        gameState.UpgradeHealth();
                        break;
                    case "Speed":
                        gameState.UpgradeSpeed();
                        break;
                }
                break;
            case "Uncommon":
                switch(option.Name)
                {
                    case "Boost Cooldown":
                        gameState.UpgradeBoostCooldown();
                        break;
                }
                break;
            case "Rare":
                switch(option.Name)
                {
                    case "Explosion Radius":
                        gameState.UpgradeExplosionRadius();
                        break;
                }
                break;
            case "Epic":
                switch(option.Name)
                {
                    case "Damage":
                        gameState.UpgradeDamage();
                        break;
                    case "Attack Speed":
                        gameState.UpgradeAttackSpeed();
                        break;
                }
                break;
            case "Extraordinary":
                switch(option.Name)
                {
                    case "Additional Missile":
                        gameState.UpgradeAdditionalMissile();
                        break;
                }
                break;
            case "Cursed":
                switch(option.Name)
                {
                    case "Large damage boost at a cost":
                        gameState.WildShots();
                        break;
                }
                break;

        }
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {  
        
    }
}
