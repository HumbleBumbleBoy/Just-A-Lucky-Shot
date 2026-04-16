using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HealthComponent : Node  // Note to self for this and all future components, keep them STRICT. This will just increase and decrese health PERIOD, no death managing or other effects.
{
    [Export] public float MaxHealth;
    [Export] public float ArmorAmount;  // Flat number reduction
    public float _currentHealth;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
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
        float finalDamage = Math.Max(amount - ArmorAmount, 1);  // Caps minimum damage dealt to 1
        _currentHealth -= finalDamage;
        
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
