using Godot;
using System;

public partial class HealthComponent : Node  // Note to self for this and all future components, keep them STRICT. This will just increase and decrese health PERIOD, no death managing or other effects.
{
    [Export] public float MaxHealth;
    [Export] public float DamageResistance;
    [Export] public bool IsPercentageValue;
    public float _currentHealth;

    public override void _Ready()
    {
        
    }

    public void SetHealth(float amount)
    {
        _currentHealth = amount;
    }

    public void IncreaseHealth(float amount)
    {
        _currentHealth += amount;
    }

    public void DecreaseHealth(float amount)
    {
        _currentHealth -= amount;
    }

    public void SetMaxHealth(float amount)
    {
        MaxHealth = amount;
    }

    public void IncreaseMaxHealth(float amount)
    {
        MaxHealth += amount;
    }

    public void DecreaseMaxHealth(float amount)
    {
        MaxHealth -= amount;
    }
}
