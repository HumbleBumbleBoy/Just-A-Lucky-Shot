using Godot;
using System;

public partial class GunDamageComponent : Node   // Add signals later
{
    public GenericGun GenericGun;
    [Export] public float BaseDamage;
    public float _additionalDamage;
    public float _additionalMultiplier;

    public override void _Ready()
    {
        
    }

    // -----------------------------

    public void SetBaseDamage(float amount)
    {
        BaseDamage = Mathf.Max(amount, 1f);
    }

    public void IncreaseBaseDamage(float amount)
    {
        BaseDamage = Mathf.Max(BaseDamage + amount, 1f);
    }

    public void DecreaseBaseDamage(float amount)
    {
        BaseDamage = Mathf.Max(BaseDamage - amount, 1f);
    }

    // -----------------------------

    public void SetAdditionalDamage(float amount)   // Allow aditional damage to go into negatives so it decreases damage dealt, the damage is capped at 1DMG anyways so this being -999999 doesn't stop the ability of dealign damage
    {
        _additionalDamage = amount;
    }

    public void IncreaseAdditionalDamage(float amount)
    {
        _additionalDamage += amount;
    }

    public void DecreaseAdditionalDamage(float amount)
    {
        _additionalDamage -= amount;
    }

    // -----------------------------

    public void SetAdditionalMultiplier(float amount)
    {
        _additionalMultiplier = Mathf.Max(amount, 0.1f);
    }

    public void IncreaseAdditionalMultiplier(float amount)
    {
        _additionalMultiplier = Mathf.Max(_additionalMultiplier + amount, 0.1f);
    }

    public void DecreaseAdditionalMultiplier(float amount)
    {
        _additionalMultiplier = Mathf.Max(_additionalMultiplier - amount, 0.1f);
    }
}
