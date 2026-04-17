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
        
        _player.Velocity = _velocity.CalculateVelocity(_player.Velocity, moveDirection, delta);
        _player.HandleMovement(delta);
        
        if (!_player.IsOnFloor())
        {
            stateMachineComponent.TransitionTo("PlayerFall");
        }
    }
}