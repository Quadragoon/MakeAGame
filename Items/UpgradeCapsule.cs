using Godot;
using System;

public class UpgradeCapsule : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Label upgradeLabel;
    WeightedGroup<UpgradeOption> group = new WeightedGroup<UpgradeOption>(){
        {new UpgradeOption("Health", "Common"), 100}, 
        {new UpgradeOption("Speed", "Common"), 100}, 
        {new UpgradeOption("Damage", "Epic"), 20}, 
        {new UpgradeOption("Attack Speed", "Epic"), 20},
        {new UpgradeOption("Additional Missile", "Extraordinary"), 10}, 
        {new UpgradeOption("Explosion Radius", "Rare"), 50}, 
        {new UpgradeOption("Boost Cooldown", "Uncommon"), 75},
        {new UpgradeOption("Large damage boost at a cost", "Cursed", true), 1},
    };
    
    private GameState gameState;
    private Player player;
    private EndlessMode endlessMode;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        player = GetNode<Player>("/root/EndlessMode/Player");
        endlessMode = GetNode<EndlessMode>("/root/EndlessMode");
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
