using Godot;
using System;

public partial class PlayerDash : State
{
    private Player _player;
    private VelocityComponent _velocity;
    [Export] private float _dashForce;
    [Export] private float _dashDuration;
    private float _dashTimer = 0f;
    private Vector2 _dashDirection;

    public override void Ready()
    {
        _player = stateMachineComponent.GetParent<Player>();
        _velocity = _player.VelocityComponent;
    }

    public override void Enter()
    {
        _dashDirection = new Vector2(_player.TargetDirection.X != 0 ? Mathf.Sign(_player.TargetDirection.X) : Mathf.Sign(_player.Velocity.X), 0);
        
        if (_dashDirection.X == 0) _dashDirection.X = 1;
        _player.Velocity = new Vector2(_dashDirection.X * _dashForce, 0);
        _dashTimer = _dashDuration;
    }

    public override void PhysicsUpdate(float delta)
    {
        _dashTimer -= delta;
        
        _player.Velocity = _player.Velocity.Lerp(Vector2.Zero, 5f * delta);
        _player.MoveAndSlide();
        
        if (_dashTimer <= 0f)
        {
            GD.Print("Dash timer ended, transitioning out");
            if (_player.IsOnFloor())
                stateMachineComponent.TransitionTo("PlayerMove");
            else
                stateMachineComponent.TransitionTo("PlayerFall");
        }
    }
}
