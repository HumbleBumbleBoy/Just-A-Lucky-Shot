using Godot;
using System;

public partial class Player : CharacterBody2D, IMoveable
{
    [Export] public VelocityComponent VelocityComponent;
    private StateMachineComponent _stateMachine;
    public Vector2 TargetDirection;
    private float _leeway = 0.1f;
    private float _coyoteTimer = 0.1f;
    private bool _isJumping = false;
    private float _jumpHoldTime = 0;
    private const float _maxHoldTime = 0.4f;
    private const float _minForce = -350;
    private const float _maxForce = -500;
    
    public override void _Ready()
    {
        _stateMachine = GetNode<StateMachineComponent>("StateMachineComponent");
        _stateMachine.TransitionTo("PlayerMove");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        TargetDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");

        if (Mathf.Abs(TargetDirection.X) < _leeway) { TargetDirection.X = 0; }
        if (Mathf.Abs(TargetDirection.Y) < _leeway) { TargetDirection.Y = 0; }
        _stateMachine._PhysicsProcess((float)delta);
    }

    public void HandleMovement(float delta)
    {
        Vector2 moveDirection = new Vector2(TargetDirection.X, 0);
        Velocity = VelocityComponent.CalculateVelocity(Velocity, moveDirection, delta);
        
        // Start jump when pressing up AND (on ground OR within coyote window)
        if (TargetDirection.Y < 0 && (IsOnFloor() || _coyoteTimer > 0) && !_isJumping)
        {
            _isJumping = true;
            _jumpHoldTime = 0;
            _coyoteTimer = 0;  // Clear coyote timer on jump
        }
        
        // Rise while holding up, during jump, until max time
        if (_isJumping && Input.IsActionPressed("MoveUp") && _jumpHoldTime < _maxHoldTime)
        {
            _jumpHoldTime += delta;
            _jumpHoldTime = Mathf.Min(_jumpHoldTime, _maxHoldTime);
            float t = _jumpHoldTime / _maxHoldTime;
            float jumpForce = Mathf.Lerp(_minForce, _maxForce, t);
            Velocity = new Vector2(Velocity.X, jumpForce);
        }
        
        // Stop rising when release up or max time reached
        if (_isJumping && (Input.IsActionJustReleased("MoveUp") || _jumpHoldTime >= _maxHoldTime))
        {
            _isJumping = false;
        }
        
        // Update coyote timer: reset when on ground, decrease when in air
        if (IsOnFloor())
        {
            _coyoteTimer = 0.1f;  // Reset to full when on ground
        }
        else
        {
            _coyoteTimer -= delta;  // Decrease when in air
            _coyoteTimer = Mathf.Max(0, _coyoteTimer);  // Clamp to 0
        }
        
        // Gravity (only when not jumping OR when jump is over)
        if (!IsOnFloor() && !_isJumping)
        {
            Velocity += new Vector2(0, VelocityComponent.GravitationalPull * 100 * delta);
        }
        
        MoveAndSlide();
    }
}
