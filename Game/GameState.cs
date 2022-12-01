using Godot;
using System;

public class GameState : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public int level = 1;
    public int score = 0;
    public int acceleration = 2500;
    public int boostPower = 5000;
    public float maxHealth = 5;
    public float damage = 1;
    public float missileMultiplierChance = 0; //chance of new missile, 100 -> one guaranteed extra missile, 150 -> one extra and a chance for a third missile
    public float explosionRadiusScale = 1;
    public int speedUpgradeAmount = 100;
    public float fireDelay = 0.2f;
    public bool stormtrooper = false;
    public float boostCooldown = 3.0f;


    //PROBABLY UNNECESSARY BOOLS
    public bool upgradeMode = false;
    
    public override void _Ready()
    {
        
    }

    public void UpgradeHealth()
    {
        //TODO: Add sound effects and animation -> maybe animate scene transition
        maxHealth++;
    }

    public void UpgradeSpeed()
    {
        //TODO: Add sound effects and animation -> maybe animate scene transition
        acceleration += speedUpgradeAmount;
        boostPower += (speedUpgradeAmount * 2);
    }

    public void UpgradeDamage()
    {
        damage += 0.5f;
    }

    public void UpgradeAdditionalMissileChance()
    {
        missileMultiplierChance += 100.0f;
    }

    public void UpgradeExplosionRadius()
    {
        explosionRadiusScale+=0.5f;
    }

    public void UpgradeBoostCooldown()
    {
        boostCooldown /= 1.2f;
    }

    public void UpgradeAttackSpeed()
    {
        fireDelay /= 1.1f;
    }
    
    public void WildShots()
    {
        damage *= 2;
        stormtrooper = true;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
