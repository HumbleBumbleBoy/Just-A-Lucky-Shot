using Godot;
using System;
using System.Text.RegularExpressions;

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
            // note to self: implement another variable for the bullet so it checks who its fired from, so enemies dont shoot themselves nor the player shoot themself
            // Check how many pierce left, if 0 the play hit animation and disapear
        }
    }

    public void OnDetectionBoxBodyEntered(Node2D node)
    {
        if (node is Node2D Colider) // Note for later, check if colided node is in group "SolidWall"
        {
            if (!Colider.IsInGroup("SolidWall")) return;

            // Check how many bounces left, if 0 then play a crash animation and disapear the bullet otherwise bounce the bullet
            if (BulletBounceComponent._bouncesLeft <= 0) {
                KillBullet();
            }
            
            // TODO: Calculate angle to bounce bullet
            BulletBounceComponent.DecreaseBouncesLeft(1);
        }
    }

    public void OnTimeoutEnd() {
        KillBullet();
    }

    private void FlyForward(double delta)
    {
        float deltaF = (float)delta;
        Vector2 moveDirection = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
        Velocity = BulletVelocityComponent.CalculateVelocity(Velocity, moveDirection, deltaF);
        MoveAndSlide();
    }

    private void KillBullet() {
        // also play animation
        QueueFree();
    }
}
