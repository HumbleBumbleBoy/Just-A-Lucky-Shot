using Godot;
using System;
using System.Text.RegularExpressions;

public partial class HurtBoxComponent : Area2D
{
    [Export] Node HealthComponent;
    [Export] GenericEntity _hurtBoxOwner;

    public override void _Ready()
    {
        
    }

    public void OnHurtBoxArea2DEntered(Area2D area)
    {
        if (area.GetParent() is GenericBullet bullet)
        {
            // check if bullet is of the opposite group, if so deal damage
            if ((_hurtBoxOwner.IsInGroup("Player") && bullet.firedBy == "Enemy") ||
                (_hurtBoxOwner.IsInGroup("Enemy") && bullet.firedBy == "Player"))
            {
                bullet.BulletDamageComponent.DealDamage(_hurtBoxOwner, bullet._trueDamage);

                if (_hurtBoxOwner.HealthComponent._currentHealth <= 0f)
                {
                    // play like an animation and fucking DIE
                    _hurtBoxOwner.QueueFree();
                }
            }
        }
    }
}
