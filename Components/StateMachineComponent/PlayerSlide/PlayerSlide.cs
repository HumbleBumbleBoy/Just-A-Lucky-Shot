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
        _player.Velocity = _velocity.CalculateVelocity(_player.Velocity * 2, moveDirection, delta);
        _player.HandleMovement(delta);
        
        // Transition to fall if leaving ground
        if (!_player.IsOnFloor() && _player.Velocity.Y < 0)
        {
            stateMachineComponent.TransitionTo("PlayerFall");
            return;
        } else
        {
            stateMachineComponent.TransitionTo("PlayerMove");
            return;
        }
    }
}