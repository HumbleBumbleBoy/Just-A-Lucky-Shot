using System.Linq;
using Godot;

public partial class Player : GenericEntity, IMoveable 
{
	[Export] public VelocityComponent VelocityComponent;
	[Export] public WeaponSwitchComponent WeaponSwitchComponent;
	[Export] public string CharacterName;
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
	public GenericGun _equipedGun;
	private StateMachineComponent _stateMachine;
	public Vector2 TargetDirection;
	public Hud hud;
	
	public override void _Ready()
	{
		_stateMachine = GetNode<StateMachineComponent>("StateMachineComponent");
		_dashCooldownTimer = _dashCooldown;
		WeaponSwitchComponent.SwitchToSlot(1);	// Switch to weapon slot 1 when first entering the game
		_equipedGun = WeaponSwitchComponent._equipedGun;
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		
		TargetDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		
		if (Mathf.Abs(TargetDirection.X) < _leeway) TargetDirection.X = 0;
		if (Mathf.Abs(TargetDirection.Y) < _leeway) TargetDirection.Y = 0;

		if (Input.IsActionPressed("Shoot")) HandleShoot();
		if (Input.IsActionPressed("Reload")) HandleReload();
		// Handle weapon switching
		if (Input.IsActionJustPressed("ScrollDown")) WeaponSwitchComponent.SwitchWeaponByOffset(-1);
		if (Input.IsActionJustPressed("ScrollUp")) WeaponSwitchComponent.SwitchWeaponByOffset(1);
		for (int i = 1; i <= 9; i++)
		{
			int slot = i;
			if (Input.IsActionJustPressed($"Slot{slot}"))
			{
				WeaponSwitchComponent.SwitchToSlot(slot);
			}
		}
		
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

	private void HandleShoot()  // Handles reload time and firerate in FiringComponent
	{
		hud.UpdateAmmoInClipText();
		_equipedGun = WeaponSwitchComponent._equipedGun;
		_equipedGun.FiringComponent.Shoot("Player");
	}
	
	private void HandleReload() 
	{
		_equipedGun.AmmoComponent.Reload(true);
	}
}