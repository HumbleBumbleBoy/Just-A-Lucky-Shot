using Godot;
using System;
using System.Text.RegularExpressions;

public partial class FiringComponent : Node   // Add signals later
{
    public GenericGun GenericGun;
    [Export] public float FireRate;         // Shots per second
    [Export] public float MaxFireRate;      // Fire rate cap
    [Export] public int BulletsPerShot;
    [Export] public float SpreadDegrees;    // 1.0f = 2 degree (1 up 1 down)
    [Export] public float MaxSpreadDegrees; // I recommend to not go more than 120 tho
    private Node2D _bulletsScene;
    private bool _canShoot = true;

    public override void _Ready()
    {
        _bulletsScene = (Node2D)GetTree().GetFirstNodeInGroup("BulletsNode");
        
    }

    public void Shoot(String GroupOfShooter)
    {
        if (GenericGun.AmmoComponent._isReloading) return; // Don't shoot if reloading
        if (GenericGun.AmmoComponent._currentAmmo <= 0) { GenericGun.AmmoComponent.Reload(GroupOfShooter == "Player"); /* Sets true/false based on shooter */ return; }; // Auto reload if no ammo in clip
        if (GenericGun.AmmoComponent._currentAmmo <= 0 && GenericGun.AmmoComponent.TotalAmmo <= 0) { return; }
        if (!_canShoot) return;
        
        ActuallyShoot(GroupOfShooter);

        GetTree().CreateTimer(1.0f / FireRate).Timeout += () => _canShoot = true;   // Makes a one shot timer that enables shooting fireRate times per second
    }

    private void ActuallyShoot(String GroupOfShooter)
    {
        _canShoot = false;
        GenericGun.AmmoComponent.DecreaseCurrentAmmo(1); // Shot fired, lose 1 ammo

        string _bullet = GenericGun.BulletTypeEquiped; // Which bullet to use for calculations
        PackedScene bulletScene = GD.Load<PackedScene>($"res://GameElements/Bullets/{_bullet}/{ToSnakeCase(_bullet)}.tscn"); // Get the scene for calculations
        
        int _bulletsShot = 0;
        while (_bulletsShot < BulletsPerShot)   // Check how many bullets to shoot
        {
            _bulletsShot++;
            GenericBullet _newBullet = (GenericBullet)bulletScene.Instantiate();

            _newBullet.BulletDamageComponent = _newBullet.GetNode<BulletDamageComponent>("BulletDamageComponent");
            _newBullet._trueDamage = _newBullet.BulletDamageComponent.CalculateDamage(GenericGun.DamageComponent.BaseDamage + GenericGun.DamageComponent._additionalDamage, _newBullet.BulletDamageComponent.DamageMultiplier + GenericGun.DamageComponent._additionalMultiplier); // find a way to check current bullet multiplier so i can add items that upgrade it
            _newBullet.firedBy = GroupOfShooter;    // Set this property so HitboxComponent knows who to detect
            _newBullet.GlobalPosition = GenericGun.ProjectileSpawnPoint.GlobalPosition;

            float baseRotation = GenericGun.GunPivot.Rotation;
            float randomDegrees = (float)GD.RandRange(-SpreadDegrees, SpreadDegrees);
            float spreadAngle = Mathf.DegToRad(randomDegrees);
            _newBullet.Rotation = baseRotation + spreadAngle;
            
            _bulletsScene.AddChild(_newBullet);
        }        
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
