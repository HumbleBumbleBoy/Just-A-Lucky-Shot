using Godot;

public partial class GenericGun : Node2D
{
    [Export] public Node2D GunPivot;
    [Export] public Marker2D ProjectileSpawnPoint;
    [Export] public Texture2D WeaponTexture;
    [Export] public string WeaponName;
    [Export] public string WeaponDescription;
    [Export] public int Rarity; // 1 - most common to 5 - most rare 
    [Export] public string BulletTypeEquiped;
    [Export] public string[] EquipedItems;
    public AmmoComponent AmmoComponent;
    public FiringComponent FiringComponent;
    public GunDamageComponent DamageComponent;
    private GenericEntity _gunOwner;
    private bool _isHeldByPlayer;

    public Player player; // This reference to the player is used only twice, here and inside the ammoComponent for updating UI when reload done, it's the simplest way i can think of doing it as of now. (referece set at gamescene)

    public override void _Ready()
    {
        AmmoComponent = GetNode<AmmoComponent>("AmmoComponent");
        FiringComponent = GetNode<FiringComponent>("FiringComponent");
        DamageComponent = GetNode<GunDamageComponent>("GunDamageComponent");
        
        // Set the GenericGun reference in each component
        if (AmmoComponent != null) AmmoComponent.GenericGun = this;
        if (FiringComponent != null) FiringComponent.GenericGun = this;
        if (DamageComponent != null) DamageComponent.GenericGun = this;

        GunPivot ??= GetParent().GetParent().GetNode<Node2D>("GunPivot");   // not for future, if i don't initilise guns PRECISELY 2 nodes below root of player this might break, so i should add a node to player for storing guns in and just reference them from there, or rework this
        _gunOwner = GetParent().GetParent() as GenericEntity;   // same issue here
        if (_gunOwner.IsInGroup("Player")) _isHeldByPlayer = true;
    }

    public override void _Process(double delta)
    {
        if (_isHeldByPlayer) LookAtMouse();
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
