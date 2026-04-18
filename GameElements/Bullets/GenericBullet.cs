using Godot;
using System;

public partial class GenericBullet : CharacterBody2D
{
    [Export] Sprite2D BulletSprite;
    public VelocityComponent VelocityComponent;
    public HomingComponent HomingComponent;
    public BulletDamageComponent BulletDamageComponent;
    public BulletSizeComponent BulletSizeComponent;

    public override void _Ready()
    {
        VelocityComponent = GetNode<VelocityComponent>("VelocityComponent");
        HomingComponent = GetNode<HomingComponent>("HomingComponent");
        BulletDamageComponent = GetNode<BulletDamageComponent>("BulletDamageComponent");
        BulletSizeComponent = GetNode<BulletSizeComponent>("BulletSizeComponent");
        
        // Set the GenericBullet reference in each component
        if (HomingComponent != null) HomingComponent.GenericBullet = this;
        if (BulletDamageComponent != null) BulletDamageComponent.GenericBullet = this;
        if (BulletSizeComponent != null) BulletSizeComponent.GenericBullet = this;
    }
}
