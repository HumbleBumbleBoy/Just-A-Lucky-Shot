using Godot;
using System;

public partial class BulletDamageComponent : Node
{
    public GenericBullet GenericBullet;
    [Export] public float DamageMultiplayer;
    [Export] public int Pierce;   // How many targets until bullet expires (connect like a signal from root node to here and add a counter then call an expire function from generic bullet, btw remmeber to extend that class to future bullet types you retard, thanks)
    private int _pierceLeft;

    public override void _Ready()
    {
        _pierceLeft = Pierce;
    }

    // lowk write functions that use allat

    // -----------------------------

    public void SetDamageMultiplayer(float amount)
    {
        DamageMultiplayer = Mathf.Max(amount, 0.1f);    // Cap multiplier at 10% damage
    }

    public void IncreaseDamageMultiplayer(float amount)
    {
        DamageMultiplayer = Mathf.Max(DamageMultiplayer + amount, 0.1f);
    }

    public void DecreaseDamageMultiplayer(float amount)
    {
        DamageMultiplayer = Mathf.Max(DamageMultiplayer - amount, 0.1f);
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
