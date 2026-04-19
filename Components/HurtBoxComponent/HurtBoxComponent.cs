using Godot;
using System;

public partial class HurtBoxComponent : Area2D
{
    [Export] Node HealthComponent;
    [Export] GenericEntity _hurtBoxOwner;

    public override void _Ready()
    {
        
    }

    public void OnHurtBoxArea2DEntered(Area2D area)
    {
        GD.Print($"Area name: {area.Name} \nArea class: {area.GetClass()} \nArea parent name: {area.GetParent().Name} \nArea parent class: {area.GetParent().GetClass()}");
        if (area.GetParent() is GenericBullet bullet)
        {
            bullet.BulletDamageComponent.DealDamage(_hurtBoxOwner, bullet._trueDamage);
            if (_hurtBoxOwner.HealthComponent._currentHealth <= 0f)
            {
                // play like an animation and fucking DIE
            }
            GD.Print($"Damage calculated: {bullet._trueDamage} \nHealth left: {_hurtBoxOwner.HealthComponent._currentHealth}");
        }
    }
}
