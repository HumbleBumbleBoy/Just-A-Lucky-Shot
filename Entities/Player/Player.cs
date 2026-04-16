using Godot;
using System;

public partial class Player : CharacterBody2D, IMoveable
{
    [Export] public VelocityComponent VelocityComponent;
    private StateMachineComponent _stateMachine;
    public Vector2 TargetDirection;
    private bool _isJumping = false;
    private float _jumpHoldTime = 0;
    private const float MAX_HOLD_TIME = 0.4f;
    private const float MIN_FORCE = -350;
    private const float MAX_FORCE = -500;
    
    public override void _Ready()
    {
        _stateMachine = GetNode<StateMachineComponent>("StateMachineComponent");
        _stateMachine.TransitionTo("PlayerMove");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        TargetDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");

        float leeway = 0.1f;
        if (Mathf.Abs(TargetDirection.X) < leeway) { TargetDirection.X = 0; }
        if (Mathf.Abs(TargetDirection.Y) < leeway) { TargetDirection.Y = 0; }
        _stateMachine._PhysicsProcess((float)delta);
    }

    public void HandleMovement(float delta)
    {
        Vector2 moveDirection = new Vector2(TargetDirection.X, 0);
        Velocity = VelocityComponent.CalculateVelocity(Velocity, moveDirection, delta);
        
        // Start jump on ground when pressing up
        if (TargetDirection.Y < 0 && IsOnFloor() && !_isJumping)
        {
            _isJumping = true;
            _jumpHoldTime = 0;
        }
        
        // Rise while holding up, during jump, until max time
        if (_isJumping && Input.IsActionPressed("MoveUp") && _jumpHoldTime < MAX_HOLD_TIME)
        {
            _jumpHoldTime += delta;
            _jumpHoldTime = Mathf.Min(_jumpHoldTime, MAX_HOLD_TIME);
            float t = _jumpHoldTime / MAX_HOLD_TIME;
            float jumpForce = Mathf.Lerp(MIN_FORCE, MAX_FORCE, t);
            Velocity = new Vector2(Velocity.X, jumpForce);
        }
        
        // Stop rising when release up or max time reached
        if (_isJumping && (Input.IsActionJustReleased("MoveUp") || _jumpHoldTime >= MAX_HOLD_TIME))
        {
            _isJumping = false;
        }
        
        // Reset jump when back on ground
        if (IsOnFloor() && !_isJumping)
        {
            _jumpHoldTime = 0;
        }
        
        // Gravity (only when not jumping OR when jump is over)
        if (!IsOnFloor() && !_isJumping)
        {
            Velocity += new Vector2(0, VelocityComponent.GravitationalPull * 100 * delta);
        }
        
        GD.Print($"Velocity is: {Velocity} \n Move direction: {moveDirection}");
        MoveAndSlide();
    }
}
