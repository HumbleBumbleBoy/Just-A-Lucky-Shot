using Godot;
using System;

public partial class GenericEnemy : GenericEntity, IMoveable
{
    [Export] public VelocityComponent VelocityComponent;
    [Export] public PathfindComponent PathfindComponent;
	[Export] public string CharacterName;
	[Export] private float _jumpHoldMaxTime;
	[Export] private float _minJumpForce;
	[Export] private float _maxJumpForce;
	private float _leeway = 0.1f;
	private float _coyoteTimer = 0.1f;
	private bool _isJumping = false;
	private float _jumpHoldTimer = 0f;
	private bool _wasOnFloor = true;
	public GenericGun _equipedGun;
	private StateMachineComponent _stateMachine;
	public Vector2 TargetDirection;

	
	public override void _Ready()
	{
		_stateMachine = GetNode<StateMachineComponent>("StateMachineComponent");
		_equipedGun = GetTree().GetFirstNodeInGroup("Gun") as GenericGun;
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		
        // Wait for a timer to pass, then pathfind and move to save resources
        // PathfindComponent.Pathfind()
		// TargetDirection = ;  // Will get it from pathfind

		// if () HandleShoot();
		// if () HandleReload();
		
		_stateMachine._PhysicsProcess(deltaF);  // <- this should also be in the timer
	}
	
	public void HandleMovement(float delta)
	{
		UpdateCoyoteTimer(delta);
		HandleJumpStart();
		HandleJumpHold(delta);
		HandleJumpEnd();
		ResetJumpOnGround();

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
		if (_isJumping /* && input to jump up held */ && _jumpHoldTimer < _jumpHoldMaxTime)
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
	
	private void ResetJumpOnGround()
	{
		if (IsOnFloor() && Velocity.Y >= 0)
		{
			_isJumping = false;
		}
	}

	private void HandleShoot()  // Handles reload time and firerate in FiringComponent
	{
		_equipedGun.FiringComponent.Shoot("Enemy");
	}
	
	private void HandleReload() 
	{
		_equipedGun.AmmoComponent.Reload(false); // NOT player reloading
	}

}
