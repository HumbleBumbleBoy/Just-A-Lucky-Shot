using Godot;
using System;

public partial class GunSlot : ColorRect
{
    [Export] public TextureRect WeaponSpriteSlot;
    [Export] public RichTextLabel GunNameLabel;
    [Export] public RichTextLabel CurrentAmmoLabel;
    [Export] public RichTextLabel MaxAmmoLabel;
    public GenericGun WeaponReference;

    public override void _Ready()
    {
        GD.Print(GetTreeStringPretty());
    }
}
