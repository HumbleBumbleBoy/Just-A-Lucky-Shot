using Godot;
using System;

public partial class Fall : State
{
    private GenericEnemy _enemy;
    private VelocityComponent _velocity;
    
    public override void Ready()
    {
        _enemy = stateMachineComponent.GetParent<GenericEnemy>();
        _velocity = _enemy.VelocityComponent;
    }
    
    public override void PhysicsUpdate(float delta) // if sees player also keep moving in its direction. also attack it if can
    {
        Vector2 moveDirection = new Vector2 (_enemy.TargetDirection.X, 0);
        _enemy.Velocity = _velocity.CalculateVelocity(_enemy.Velocity, moveDirection, delta);
        _enemy.HandleMovement(delta);
        
        if (_enemy.IsOnFloor() /* && sees player */) // on floor sees player -> move (towards it)
        {
            stateMachineComponent.TransitionTo("Move");
        } 
    }
}