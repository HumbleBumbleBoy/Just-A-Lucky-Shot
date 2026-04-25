using Godot;
using System;

public partial class GenericBullet : CharacterBody2D
{
    [Export] public string BulletName;
    [Export] public string BulletDescription;
    [Export] public int Rarity; // 1 - most common to 5 - most rare 
    public GenericGun FiredFrom;
    public HomingComponent HomingComponent;
    public BulletVelocityComponent BulletVelocityComponent;
    public BulletDamageComponent BulletDamageComponent;
    public BulletSizeComponent BulletSizeComponent;
    public BulletBounceComponent BulletBounceComponent;
    public float _trueDamage;
    public string firedBy;

    public override void _Ready()
    {
        FiredFrom = GetParent() as GenericGun;
        BulletVelocityComponent = GetNode<BulletVelocityComponent>("BulletVelocityComponent");
        HomingComponent = GetNode<HomingComponent>("HomingComponent");
        BulletDamageComponent = GetNode<BulletDamageComponent>("BulletDamageComponent");
        BulletSizeComponent = GetNode<BulletSizeComponent>("BulletSizeComponent");
        BulletBounceComponent = GetNode<BulletBounceComponent>("BulletBounceComponent");
        
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
        if (area.GetParent() is GenericEntity genericEntity) // If colides with an entity
        {
            if ((genericEntity.IsInGroup("Player") && firedBy == "Enemy") ||
                (genericEntity.IsInGroup("Enemy") && firedBy == "Player"))
            {
                BulletDamageComponent.DecreasePierce(1);

                if (BulletDamageComponent._pierceLeft <= 0) { KillBullet(); }
            }
        }
    }

    private void KillBullet() 
    {
        // also play animation
        QueueFree();
    }

    public void OnTimeoutEnd() 
    {
        KillBullet();
    }

    private void FlyForward(double delta)
    {
        float deltaF = (float)delta;
        Vector2 moveDirection = Transform.X;
        Velocity = BulletVelocityComponent.CalculateVelocity(Velocity, moveDirection, deltaF);
        Rotation = Velocity.Angle();
        
        // Use MoveAndCollide instead of MoveAndSlide
        KinematicCollision2D collision = MoveAndCollide(Velocity * deltaF);
        if (collision != null)
        {
            // Handle collision immediately with the correct normal
            OnCollision(collision);
        }
    }

    private void OnCollision(KinematicCollision2D collision)
    {
        Node2D collider = collision.GetCollider() as Node2D;
        if (collider != null && collider.IsInGroup("SolidWall"))
        {
            if (BulletBounceComponent._bouncesLeft <= 0) 
            {
                KillBullet();
                return;
            }
            
            Vector2 wallNormal = collision.GetNormal();
            Velocity = BulletVelocityComponent.CalculateBounce(Velocity, wallNormal);
            BulletBounceComponent.DecreaseBouncesLeft(1);
        }
    }
}