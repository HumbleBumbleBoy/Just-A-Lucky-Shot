using Godot;
using System;

public partial class GunSlot : ColorRect
{
    [Export] public RichTextLabel GunIndex;
    [Export] public RichTextLabel GunNameLabel;
    [Export] public RichTextLabel CurrentAmmoLabel;
    [Export] public RichTextLabel MaxAmmoLabel;
    public GenericGun WeaponReference;
}
