using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class WeaponSwitchComponent : Node
{
    [Export] public Node2D WeaponStorageNode;	// where all unused weapons are
    [Export] public int WeaponSlots;
    [Export] public int MaxWeaponSlots;
    public List<GenericGun> AvailableWeapons = [];
    public GenericGun _equipedGun;
    private readonly Dictionary<string, Action> _weaponActions = [];
    public int CurrentWeaponSlot;
    private Player _player;
    
    public override void _Ready()
    {
        _player = GetParent<Player>();
        foreach (Node child in WeaponStorageNode.GetChildren()) // Get all children of WeaponStorageNode that are GenericGun
        {
            if (child is GenericGun gun)
            {
                AvailableWeapons.Add(gun);
            }
        }

        // Equip the first weapon if available
        if (AvailableWeapons.Count > 0)
        {
            _equipedGun = AvailableWeapons[0];
            CurrentWeaponSlot = 1;
            
            // Reparent from WeaponStorage to GunPivot
            _equipedGun.GetParent().RemoveChild(_equipedGun);
            _player.GetNode<Node2D>("GunPivot").AddChild(_equipedGun);
            
            // Position it at (50,0) relative to GunPivot
            _equipedGun.Position = new Vector2(50, 0);
        }
    }
    
    public void SwitchWeaponByOffset(int offset)
    {
        int newSlot = (CurrentWeaponSlot + offset - 1 + MaxWeaponSlots) % MaxWeaponSlots + 1;
        SwitchToSlot(newSlot);
    }

    public void SwitchToSlot(int slot)
    {
        if (slot > MaxWeaponSlots) return;
        if (slot < 1 || slot > AvailableWeapons.Count) return;
        
        CurrentWeaponSlot = slot;
        GD.Print($"Switched to slot {CurrentWeaponSlot}");
        
        GenericGun newWeapon = AvailableWeapons[slot - 1];
        
        if (_equipedGun == newWeapon) return;
        
        // Move current weapon back to storage
        if (_equipedGun != null)
        {
            _equipedGun.GetParent().RemoveChild(_equipedGun);
            WeaponStorageNode.AddChild(_equipedGun);
            _equipedGun.Position = new Vector2(50,0);
        }
        
        // Move new weapon to GunPivot
        newWeapon.GetParent().RemoveChild(newWeapon);
        _player.GetNode<Node2D>("GunPivot").AddChild(newWeapon);
        newWeapon.Position = new Vector2(50,0);
        
        _equipedGun = newWeapon;
        
        _player._equipedGun = _equipedGun;
        _player.hud.InitialiseHud();
            
        ColorRect reloadPlaceholder = _player.hud.GetNode<ColorRect>("CurrentGun").GetNode<ColorRect>("ReloadPlaceholder");
        reloadPlaceholder.Hide();
        
    }
}