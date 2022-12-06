using Godot;
using System;

public class UpgradeCapsule : Area2D
{
    WeightedGroup<UpgradeOption> group = new WeightedGroup<UpgradeOption>(){
        {new UpgradeOption("Health", "Common"), 100}, 
        {new UpgradeOption("Speed", "Common"), 100}, 
        {new UpgradeOption("Damage", "Epic"), 25}, 
        {new UpgradeOption("Attack Speed", "Epic"), 25},
        {new UpgradeOption("Additional Missile", "Extraordinary"), 10}, 
        {new UpgradeOption("Explosion Radius", "Rare"), 50}, 
        {new UpgradeOption("Boost Cooldown", "Uncommon"), 75},
        {new UpgradeOption("Large damage boost at a cost", "Cursed", true), 5},
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
        ApplyUpgrade(group.GetItem()); //TODO: Add pickup animation
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
        if(option.Name == "Large damage boost at a cost" && gameState.stormtrooper) //Prevents player from getting the same curse twice
        {
            while(option.Name == "Large damage boost at a cost")
            {
                option = group.GetItem();
            }
        }
        upgradeLabel.Text = option.Name;
        switch(option.Rarity)
        {
            case "Uncommon":
                upgradeLabel.SelfModulate = new Color(.102f, 1f, 0f);
                // upgradeLabel.AddColorOverride("font_color", new Color(26f, 255f, 0f)); //Green
                break;
            case "Rare":
                upgradeLabel.SelfModulate = new Color(0f, .345f, 1f);
                // upgradeLabel.AddColorOverride("font_color", new Color(0f, 88f, 255f)); //Blue
                break;
            case "Epic":
                upgradeLabel.SelfModulate = new Color(.592f, 0f, 1f);
                // upgradeLabel.AddColorOverride("font_color", new Color(151f, 0f, 255f)); //Purple
                break;
            case "Extraordinary":
                upgradeLabel.SelfModulate = new Color(1f, 0.38f, 0f);
                // upgradeLabel.AddColorOverride("font_color", new Color(255f, 96f, 0f)); //Orange
                break;
            case "Cursed":
                upgradeLabel.SelfModulate = new Color(.592f, 0f, 0f);
                // upgradeLabel.AddColorOverride("font_color", new Color(150f, 0f, 0f)); //Red
                break;
            default:
                upgradeLabel.SelfModulate = Colors.White;
                // upgradeLabel.AddColorOverride("font_color", new Color(255f, 255f, 255f)); //White
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
