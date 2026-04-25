using Godot;
using System;

public partial class BulletVelocityComponent : Node
{
    [Export] private float MaxSpeed;
    [Export] private float AccelerationCoefficient;
    [Export] public float GravitationalPull;  
    [Export] private float Damping;
    private bool _initialized = false;

    public Vector2 CalculateVelocity(Vector2 currentVel, Vector2 targetDirection, float delta)
    {
        // set initial velocity
        if (!_initialized && targetDirection != Vector2.Zero)
        {
            currentVel = targetDirection.Normalized() * (MaxSpeed * 100);
            _initialized = true;
        }
        
        currentVel.Y += GravitationalPull * 100 * delta;
        currentVel.X *= Mathf.Clamp(1.0f - (Damping * delta), 0.0f, 1.0f);
        
        return currentVel;
    }

    public Vector2 CalculateBounce(Vector2 currentVel, Vector2 wallNormal)  // ???
    {
        // Get bounce component from parent
        BulletBounceComponent bounceComp = GetParent().GetNode<BulletBounceComponent>("BulletBounceComponent");
        
        if (bounceComp == null) return currentVel;
        
        // CRITICAL: Make sure normal is normalized!
        Vector2 normalizedNormal = wallNormal.Normalized();
        
        // Split velocity
        float dotProduct = currentVel.Dot(normalizedNormal);
        Vector2 perpendicular = dotProduct * normalizedNormal;
        Vector2 parallel = currentVel - perpendicular;
        
        // Apply friction and bounciness
        Vector2 newVelocity = (parallel * bounceComp.Friction) - (perpendicular * bounceComp.Bounciness);
        
        // Debug output
        GD.Print($"  Dot: {dotProduct}, Perp: {perpendicular}, Parallel: {parallel}");
        GD.Print($"  New Vel: {newVelocity}");
        
        return newVelocity;
    }
}