using Godot;

public partial class PlayerFall : State
{
    private Player _player;
    private VelocityComponent _velocity;
    
    public override void Ready()
    {
        _player = stateMachineComponent.GetParent<Player>();
        _velocity = _player.VelocityComponent;
    }
    
    public override void PhysicsUpdate(float delta)
    {
        // Air control - minimal damping to preserve momentum
        Vector2 moveDirection = new Vector2(_player.TargetDirection.X, 0);
        _player.Velocity = _velocity.CalculateVelocity(_player.Velocity, moveDirection, delta);
        _player.HandleMovement(delta);
        
        // Transition to move or slide when landing
        if (_player.IsOnFloor())
        {
            float floorAngle = Mathf.RadToDeg(Mathf.Acos(_player.GetFloorNormal().Dot(Vector2.Up)));
            
            if (floorAngle > 1.0f)
            {
                stateMachineComponent.TransitionTo("PlayerSlide");
            }
            else
            {
                stateMachineComponent.TransitionTo("PlayerMove");
            }
        }
    }
}