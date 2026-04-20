using Godot;

public partial class GenericEntity : CharacterBody2D
{
    [Export] public HealthComponent HealthComponent;
    [Export] VelocityComponent VelocityComponent;
    [Export] PathfindComponent PathfindComponent;

    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
    {
        
    }

    public override void _PhysicsProcess(double delta)
    {
        
    }
}
