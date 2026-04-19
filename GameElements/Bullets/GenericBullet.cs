using Godot;
using System;

public partial class GenericBullet : CharacterBody2D
{
    public GenericGun FiredFrom;
    public VelocityComponent VelocityComponent;
    public HomingComponent HomingComponent;
    public BulletDamageComponent BulletDamageComponent;
    public BulletSizeComponent BulletSizeComponent;
    public float _trueDamage;

    public override void _Ready()
    {
        FiredFrom = GetParent() as GenericGun;
        VelocityComponent = GetNode<VelocityComponent>("VelocityComponent");
        HomingComponent = GetNode<HomingComponent>("HomingComponent");
        BulletDamageComponent = GetNode<BulletDamageComponent>("BulletDamageComponent");
        BulletSizeComponent = GetNode<BulletSizeComponent>("BulletSizeComponent");
        
        // Set the GenericBullet reference in each component
        if (HomingComponent != null) HomingComponent.GenericBullet = this;
        if (BulletDamageComponent != null) BulletDamageComponent.GenericBullet = this;
        if (BulletSizeComponent != null) BulletSizeComponent.GenericBullet = this;
    }

    public override void _PhysicsProcess(double delta)
    {
        FlyForward(delta);
    }

    public void OnDetectionBoxAreaEntered(Area2D area)
    {
        if (area is HurtBoxComponent) // If colides with an entity
        {
            // note to self: implement another variable for the bullet so it checks who its fired from, so enemies dont shoot themselves nor the player shoot themself
            // Check how many pierce left, if 0 the play hit animation and disapear
        }
    }

    public void OnDetectionBoxBodyEntered(Node2D node)
    {
        if (node is Node2D SolidSurface)
        {
            // Check how many bounces left, if 0 then play a crash animation and disapear the bullet
        }
    }

    private void FlyForward(double delta)
    {
        float deltaF = (float)delta;
        Vector2 moveDirection = Vector2.Right.Rotated(Rotation); // 
        Velocity = VelocityComponent.CalculateVelocity(Velocity, moveDirection, deltaF);
        Rotation = Velocity.Angle();
        MoveAndSlide();
    }
}
