using Godot;

public partial class PlayerSlide : State
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
        
        // Get current floor angle
        float floorAngle = 0f;
        if (_player.IsOnFloor())
        {
            floorAngle = Mathf.RadToDeg(Mathf.Acos(_player.GetFloorNormal().Dot(Vector2.Up)));
        }
        
        // On steep slopes, reduce damping (keep momentum)
        if (floorAngle > 30f)
        {
            dampingMultiplier *= 0.1f; // a tenth of damping on slopes ??? idk
        
        _player.Velocity = _velocity.CalculateVelocity(_player.Velocity, moveDirection, delta, dampingMultiplier);
        _player.HandleMovement(delta);
        
        if (!_player.IsOnFloor())
        {
            stateMachineComponent.TransitionTo("PlayerFall");
        }
        else
        {
            floorAngle = Mathf.RadToDeg(Mathf.Acos(_player.GetFloorNormal().Dot(Vector2.Up)));
            if (floorAngle <= 1.0f)
            {
                stateMachineComponent.TransitionTo("PlayerMove");
            }
        }
    }
}
}