using Godot;
using System;

public partial class BulletDamageComponent : Node
{
    public GenericBullet GenericBullet;
    [Export] public float DamageMultiplier;
    [Export] public int Pierce;   // How many targets until bullet expires (connect like a signal from root node to here and add a counter then call an expire function from generic bullet, btw remmeber to extend that class to future bullet types you retard, thanks)
    private int _pierceLeft;

    public override void _Ready()
    {
        _pierceLeft = Pierce;
    }

    public void DealDamage(GenericEntity Target, float amount)
    {
        Target.HealthComponent.DecreaseHealth(amount);
    }

    public float CalculateDamage(float BaseDamage, float DamageMultiplayer)
    {
        float total = Math.Max(BaseDamage * DamageMultiplayer, 1.0f);   // Cap minimum damage at 1
        return total;
    }

    // -----------------------------

    public void SetDamageMultiplayer(float amount)
    {
        DamageMultiplier = Mathf.Max(amount, 0.1f);    // Cap multiplier at 10% damage
    }

    public void IncreaseDamageMultiplayer(float amount)
    {
        DamageMultiplier = Mathf.Max(DamageMultiplier + amount, 0.1f);
    }

    public void DecreaseDamageMultiplayer(float amount)
    {
        DamageMultiplier = Mathf.Max(DamageMultiplier - amount, 0.1f);
    }

    // -----------------------------

    public void SetPierce(int amount)
    {
        Pierce = Mathf.Max(amount, 0);    // Cap Pierce at 0
    }

    public void IncreasePierce(int amount)
    {
        Pierce = Mathf.Max(Pierce + amount, 0);
    }

    public void DecreasePierce(int amount)
    {
        Pierce = Mathf.Max(Pierce - amount, 0);
    }

    // -----------------------------

    public void SetPierceLeft(int amount)
    {
        _pierceLeft = Mathf.Max(amount, 0);    // Cap Pierce at 0
    }

    public void IncreasePierceLeft(int amount)
    {
        _pierceLeft = Mathf.Max(_pierceLeft + amount, 0);
    }

    public void DecreasePierceLeft(int amount)
    {
        _pierceLeft = Mathf.Max(_pierceLeft - amount, 0);
    }
}
