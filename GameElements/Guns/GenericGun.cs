using Godot;

public partial class GenericGun : Node2D
{
    [Export] Sprite2D GunSprite;
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
    }
}
