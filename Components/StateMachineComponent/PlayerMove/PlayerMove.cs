using Godot;

public partial class PlayerMove : State
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
        Vector2 moveDirection = new Vector2(_player.TargetDirection.X, 0);
        float dampingMultiplier = (_player.TargetDirection.X == 0) ? 1.0f : 0f;
        
        _player.Velocity = _velocity.CalculateVelocity(_player.Velocity, moveDirection, delta, dampingMultiplier);
        _player.HandleMovement(delta);
        
        if (!_player.IsOnFloor())
        {
            stateMachineComponent.TransitionTo("PlayerFall");
        }

        if (_player.TargetDirection.X == 0 && _player.IsOnFloor())
        {
            float floorAngle = Mathf.RadToDeg(Mathf.Acos(_player.GetFloorNormal().Dot(Vector2.Up)));
            if (floorAngle > 1.0f)
            {
                stateMachineComponent.TransitionTo("PlayerSlide");
            }
        }
    }
}