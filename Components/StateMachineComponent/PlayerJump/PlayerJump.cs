using Godot;

public partial class PlayerJump : State
{
    private Player _player;
    private VelocityComponent _velocity;
    
    private float _jumpHoldTimer = 0f;
    private bool _hasAppliedInitialJump = false;
    
    [Export] private float _jumpHoldMaxTime = 0.5f;
    [Export] private float _minJumpForce = -350;
    [Export] private float _maxJumpForce = -600;
    
    public override void Ready()
    {
        _player = stateMachineComponent.GetParent<Player>();
        _velocity = _player.VelocityComponent;
    }
    
    public override void Enter()
    {
        _jumpHoldTimer = 0f;
        _hasAppliedInitialJump = false;
        
        // Apply initial jump force
        _player.Velocity = new Vector2(_player.Velocity.X, _minJumpForce);
    }
    
    public override void PhysicsUpdate(float delta)
    {
        // Variable jump height (hold to jump higher)
        if (Input.IsActionPressed("MoveUp") && _jumpHoldTimer < _jumpHoldMaxTime)
        {
            _jumpHoldTimer += delta;
            _jumpHoldTimer = Mathf.Min(_jumpHoldTimer, _jumpHoldMaxTime);
            
            float t = _jumpHoldTimer / _jumpHoldMaxTime;
            float targetJumpForce = Mathf.Lerp(_minJumpForce, _maxJumpForce, t);
            
            if (_player.Velocity.Y > targetJumpForce)
            {
                _player.Velocity = new Vector2(_player.Velocity.X, targetJumpForce);
            }
        }
        
        // Apply gravity and movement
        Vector2 moveDirection = new Vector2(_player.TargetDirection.X, 0);
        _player.Velocity = _velocity.CalculateVelocity(_player.Velocity, moveDirection, delta, 0.05f);
        _player.MoveAndSlide();
        
        // End jump conditions
        if (Input.IsActionJustReleased("MoveUp") || _jumpHoldTimer >= _jumpHoldMaxTime)
        {
            // Small boost when releasing early
            if (_player.Velocity.Y < _minJumpForce * 0.5f)
            {
                _player.Velocity = new Vector2(_player.Velocity.X, _player.Velocity.Y * 0.75f);
            }
            stateMachineComponent.TransitionTo("PlayerFall");
            return;
        }
        
        // Transition to fall when going down
        if (_player.Velocity.Y >= 0)
        {
            stateMachineComponent.TransitionTo("PlayerFall");
            return;
        }
    }
}