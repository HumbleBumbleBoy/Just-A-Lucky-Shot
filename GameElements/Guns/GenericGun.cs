using System;
using System.ComponentModel;
using Godot;

public partial class GenericGun : Node2D
{
    [Export] public Node2D GunPivot;
    [Export] public Marker2D ProjectileSpawnPoint;
    [Export] public string BulletTypeEquiped;
    public AmmoComponent AmmoComponent;
    public FiringComponent FiringComponent;
    public GunDamageComponent DamageComponent;

    public override void _Ready()
    {
        AmmoComponent = GetNode<AmmoComponent>("AmmoComponent");
        FiringComponent = GetNode<FiringComponent>("FiringComponent");
        DamageComponent = GetNode<GunDamageComponent>("GunDamageComponent");
        
        // Set the GenericGun reference in each component
        if (AmmoComponent != null) AmmoComponent.GenericGun = this;
        if (FiringComponent != null) FiringComponent.GenericGun = this;
        if (DamageComponent != null) DamageComponent.GenericGun = this;

        GunPivot ??= GetParent().GetParent().GetNode<Node2D>("GunPivot");
    }

    public override void _Process(double delta)
    {
        LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 gunPos = GunPivot.GlobalPosition;
        Vector2 direction = mousePos - gunPos;
        
        // Calculate the angle to the mouse
        float angleToMouse = direction.Angle();
        GunPivot.Rotation = angleToMouse;
        
        // Flip based on the direction's x component
        if (direction.X < 0)
        {
            Scale = new Vector2(1, -1);
        }
        else
        {
            Scale = new Vector2(1, 1);
        }
    }
}
