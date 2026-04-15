using Godot;
using System;

public partial class HurtBoxComponent : Node
{
    [Export] Node HealthComponent;

    public void OnHurtBoxArea2DEntered(Area2D area2D)
    {
        // check if area2d belongs to a "bullet" or enemy projectile, emit -health based on the area2D damage value then check if current health <=0 to change state to dead
    }
}
