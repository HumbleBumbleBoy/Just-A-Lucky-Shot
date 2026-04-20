using Godot;
using System;
using System.Text.RegularExpressions;

public partial class FiringComponent : Node   // Add signals later
{
    public GenericGun GenericGun;
    [Export] public float FireRate;         // Shots per second
    [Export] public float MaxFireRate;      // Fire rate cap
    [Export] public int BulletsPerShot;
    [Export] public float SpreadDegrees;    // 1.0f = 1 degree
    [Export] public float MaxSpreadDegrees; // I recommend to not go more than 120 tho
    private Node2D _bulletsScene;

    public override void _Ready()
    {
        _bulletsScene = (Node2D)GetTree().GetFirstNodeInGroup("BulletsNode");
    }

    public void Shoot()
    {
        GenericGun.AmmoComponent.DecreaseCurrentAmmo(1);
        // Spawn a bullet in the game scene node for bullets, calculate its true damage and assign it to the bullet HERE, fix the previous implementation and think 4 times next time dumbass
        string _bullet = GenericGun.BulletTypeEquiped;
        PackedScene bulletScene = GD.Load<PackedScene>($"res://GameElements/Bullets/{_bullet}/{ToSnakeCase(_bullet)}.tscn");
        GenericBullet _newBullet = (GenericBullet)bulletScene.Instantiate();

        _newBullet.BulletDamageComponent = _newBullet.GetNode<BulletDamageComponent>("BulletDamageComponent");
        _newBullet._trueDamage = _newBullet.BulletDamageComponent.CalculateDamage(GenericGun.DamageComponent.BaseDamage, _newBullet.BulletDamageComponent.DamageMultiplier); // find a way to check current bullet multiplier so i can add items that upgrade it
        _newBullet.GlobalPosition = GenericGun.ProjectileSpawnPoint.GlobalPosition;
        _newBullet.Rotation = GenericGun.GunPivot.Rotation;
        
        _bulletsScene.AddChild(_newBullet);
    }

    public static string ToSnakeCase(string input)  // yes this is a fuckign converter in a random component
    {
        if (string.IsNullOrEmpty(input))
            return input;
        
        // Insert underscore before each uppercase letter (except first character)
        string pattern = @"(?<!^)(?=[A-Z])";
        string result = Regex.Replace(input, pattern, "_");
        
        return result.ToLower();
    }

    // -----------------------------

    public void SetFireRate(float amount)
    {
        FireRate = Mathf.Clamp(amount, 0.1f, MaxFireRate);
    }

    public void IncreaseFireRate(float amount)
    {
        FireRate = Mathf.Clamp(FireRate + amount, 0.1f, MaxFireRate);
    }

    public void DecreaseFireRate(float amount)
    {
        FireRate = Mathf.Clamp(FireRate - amount, 0.1f, MaxFireRate);
    }

    // -----------------------------

    public void SetBulletsPerShot(int amount)
    {
        BulletsPerShot = Math.Max(1, amount); // You always have at least 1 bullet in the mag;
    }

    public void IncreaseBulletsPerShot(int amount)
    {
        BulletsPerShot = Mathf.Max(1, BulletsPerShot + amount);
    }

    public void DecreaseBulletsPerShot(int amount)
    {
        BulletsPerShot = Mathf.Max(1, BulletsPerShot - amount);
    }

    // -----------------------------

    public void SetSpreadDegrees(float amount)
    {
        SpreadDegrees = Mathf.Clamp(amount, 1.0f, MaxSpreadDegrees);
    }

    public void IncreaseSpreadDegrees(float amount)
    {
        SpreadDegrees = Mathf.Clamp(SpreadDegrees + amount, 1.0f, MaxSpreadDegrees);
    }

    public void DecreaseSpreadDegrees(float amount)
    {
        SpreadDegrees = Mathf.Clamp(SpreadDegrees - amount, 1.0f, MaxSpreadDegrees);
    }
}
