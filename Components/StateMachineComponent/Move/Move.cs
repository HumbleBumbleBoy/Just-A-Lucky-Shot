using Godot;
using System;

public partial class Move : State
{
    private GenericEnemy _enemy;
    private VelocityComponent _velocity;
    
    public override void Ready()
    {
        _enemy = stateMachineComponent.GetParent<GenericEnemy>();
        _velocity = _enemy.VelocityComponent;
    }
    
    public override void PhysicsUpdate(float delta) // add ability to attack player while moving, move towards a set postiion to save resources
    { 
        Vector2 moveDirection = new Vector2(_enemy.TargetDirection.X, 0);
        
        _enemy.Velocity = _velocity.CalculateVelocity(_enemy.Velocity, moveDirection, delta);
        _enemy.HandleMovement(delta);
        
        if (!_enemy.IsOnFloor())
        {
            stateMachineComponent.TransitionTo("Fall");
        }
    }
}
