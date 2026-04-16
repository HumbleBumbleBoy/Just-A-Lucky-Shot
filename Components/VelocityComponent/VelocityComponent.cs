using Godot;

public partial class VelocityComponent : Node
{
    [Export] private float MaxSpeed;
    [Export] private float AccelerationCoefficient;
    [Export] public float GravitationalPull;  
    [Export] private float Damping;  
    private Vector2 velocity;

    public Vector2 CalculateVelocity(Vector2 currentVel, Vector2 targetDirection, float delta)
    {
        float dt = delta;

        if (targetDirection != Vector2.Zero)
        {
            Vector2 targetVel = targetDirection.Normalized() * (MaxSpeed * 100);
            currentVel.X = Mathf.MoveToward(currentVel.X, targetVel.X, AccelerationCoefficient * 100 * dt);
        }
        
        currentVel.X *= Mathf.Clamp(1.0f - (Damping * dt), 0.0f, 1.0f); // apply damping
        
        return currentVel;
    }
}
