using Godot;
using System;

public partial class FiringComponent : Node   // Add signals later
{
    public GenericGun GenericGun;
    [Export] public float FireRate;         // Shots per second
    [Export] public float MaxFireRate;      // Fire rate cap
    [Export] public int BulletsPerShot;
    [Export] public float SpreadDegrees;    // 1.0f = 1 degree
    [Export] public float MaxSpreadDegrees; // I recommend to not go more than 120 tho

    public override void _Ready()
    {
        
    }

    public void Shoot()
    {
        GenericGun.AmmoComponent.DecreaseCurrentAmmo(1);
        // Spawn a bullet that will keep an angle, im too tired to write this shit for now
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
