using Godot;
using System;

public partial class Fall : State
{
    private CharacterBody2D _entity;
    
    public override void Enter()
    {
        _entity = GetOwner() as CharacterBody2D;
        // Play "move" animation
    }
    
    public override void PhysicsUpdate(float delta)
    {
        // Entity handles its own movement logic
        if (_entity is IMoveable movable)
        {
            movable.HandleMovement(delta);
        }
    }
}