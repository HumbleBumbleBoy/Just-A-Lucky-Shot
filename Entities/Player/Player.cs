using Godot;
using System;

public partial class Player : CharacterBody2D, IMoveable
{
    private StateMachineComponent _stateMachine;
    public Vector2 TargetDirection;
    [Export] public VelocityComponent VelocityComponent;
    [Export] private float _jumpHoldMaxTime;
    [Export] private float _minJumpForce;
    [Export] private float _maxJumpForce;
    [Export] private float _dashCooldown;
    private float _dashCooldownTimer;
    private bool _canDash = true;
    private float _leeway = 0.1f;
    private float _coyoteTimer = 0.1f;
    private bool _isJumping = false;
    private float _jumpHoldTimer = 0f;
    private bool _wasOnFloor = true;
    
    public override void _Ready()
    {
        _stateMachine = GetNode<StateMachineComponent>("StateMachineComponent");
        _dashCooldownTimer = _dashCooldown;
    }

    public override void _PhysicsProcess(double delta)
    {
         float deltaF = (float)delta;
        
        TargetDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
        
        if (Mathf.Abs(TargetDirection.X) < _leeway) TargetDirection.X = 0;
        if (Mathf.Abs(TargetDirection.Y) < _leeway) TargetDirection.Y = 0;
        
        _stateMachine._PhysicsProcess(deltaF);
    }
    
    public void HandleMovement(float delta)
    {
        UpdateCoyoteTimer(delta);
        HandleJumpStart();
        HandleJumpHold(delta);
        HandleJumpEnd();
        HandleFastFall(delta);
        ResetJumpOnGround();
        HandleDash(delta);

        MoveAndSlide();
    }
    
    private void UpdateCoyoteTimer(float delta)
    {
        if (IsOnFloor())
        {
            _coyoteTimer = 0.1f;
            _wasOnFloor = true;
        }
        else if (_wasOnFloor)
        {
            _wasOnFloor = false;
        }
        else
        {
            _coyoteTimer -= delta;
        }
    }
    
    private void HandleJumpStart()
    {
        if (TargetDirection.Y < 0 && _coyoteTimer > 0f && !_isJumping)
        {
            _isJumping = true;
            _jumpHoldTimer = 0f;
            _coyoteTimer = 0f;
            Velocity = new Vector2(Velocity.X, _minJumpForce);
        }
    }
    
    private void HandleJumpHold(float delta)
    {
        if (_isJumping && Input.IsActionPressed("MoveUp") && _jumpHoldTimer < _jumpHoldMaxTime)
        {
            _jumpHoldTimer += delta;
            _jumpHoldTimer = Mathf.Min(_jumpHoldTimer, _jumpHoldMaxTime);
            
            float t = _jumpHoldTimer / _jumpHoldMaxTime;
            float targetJumpForce = Mathf.Lerp(_minJumpForce, _maxJumpForce, t);
            
            if (Velocity.Y > targetJumpForce)
            {
                Velocity = new Vector2(Velocity.X, targetJumpForce);
            }
        }
    }
    
    private void HandleJumpEnd()
    {
        if (_isJumping && (Input.IsActionJustReleased("MoveUp") || _jumpHoldTimer >= _jumpHoldMaxTime))
        {
            _isJumping = false;
            
            if (Velocity.Y < _minJumpForce * 0.5f)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y * 0.75f);
            }
        }
    }
    
    private void HandleFastFall(float delta)
    {
        if (!IsOnFloor() && Input.IsActionPressed("MoveDown"))
        {
            Velocity += new Vector2(0, VelocityComponent.GravitationalPull * 200 * delta);
        }
    }
    
    private void ResetJumpOnGround()
    {
        if (IsOnFloor() && Velocity.Y >= 0)
        {
            _isJumping = false;
        }
    }

    private void HandleDash(float delta)
    {
        // Update dash cooldown EVERY frame
        if (!_canDash)
        {
            _dashCooldownTimer -= delta;
            if (_dashCooldownTimer <= 0.0f)
            {
                _canDash = true;
            }
        }

        if (Input.IsActionJustPressed("Dash") && _canDash)
        {
            _canDash = false;
            _dashCooldownTimer = _dashCooldown;
            _stateMachine.TransitionTo("PlayerDash");
            return;
        }
    }
}