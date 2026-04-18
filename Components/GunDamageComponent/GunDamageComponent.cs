using Godot;
using System;

public partial class GunDamageComponent : Node   // Add signals later
{
    public GenericGun GenericGun;
    [Export] public float BaseDamage;

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
}
