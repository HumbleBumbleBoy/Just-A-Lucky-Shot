using Godot;
using System;
using System.Collections.Generic;

public partial class Hud : Control
{
    [Export] TextureRect CurrentGunSprite;
    [Export] public RichTextLabel CurrentGunIndexText;
    [Export] RichTextLabel CurrentGunText;
    [Export] RichTextLabel AmmoInClipText;
    [Export] RichTextLabel AmmoMaxText;
    [Export] public VBoxContainer WeaponSlotsContainer;
    [Export] PackedScene WeaponSlotScene;

    public Player _player;
    private List<GunSlot> _weaponSlotUIs = [];

    public void InitialiseHud()
    {
        UpdateCurrentGunSprite();
        UpdateCurrentGunText();
        UpdateCurrentGunIndex();
        UpdateAmmoInClipText();
        UpdateAmmoMaxText();
        RefreshWeaponSlots();
    }

    public void RefreshWeaponSlots()
    {
        // Clear existing slots
        foreach (var slot in _weaponSlotUIs)
        {
            if (IsInstanceValid(slot))
                slot.QueueFree();
        }
        _weaponSlotUIs.Clear();
        
        var weaponSwitch = _player.GetNode<WeaponSwitchComponent>("WeaponSwitchComponent");
        if (weaponSwitch == null) return;
        
        // Get the currently equipped gun
        GenericGun equippedGun = weaponSwitch._equipedGun;
        
        // Fill the HBox with all weapons EXCEPT the current one
        for (int i = 0; i < weaponSwitch.AvailableWeapons.Count; i++)
        {
            var weapon = weaponSwitch.AvailableWeapons[i];
            
            // Skip the currently equipped weapon
            if (weapon == equippedGun) continue;
            
            var slotInstance = WeaponSlotScene.Instantiate<GunSlot>();
            
            // Set weapon name
            if (slotInstance.GunNameLabel != null)
                slotInstance.GunNameLabel.Text = weapon.WeaponName;

            // Set weapon index
            if (slotInstance.GunIndex != null)
                slotInstance.GunIndex.Text = (i + 1).ToString();

            // Set weapon ammo in clip
            if (slotInstance.CurrentAmmoLabel != null)
                slotInstance.CurrentAmmoLabel.Text = weapon.AmmoComponent._currentAmmo.ToString();

            // Set weapon max ammo
            if (slotInstance.MaxAmmoLabel != null)
                slotInstance.MaxAmmoLabel.Text = weapon.AmmoComponent.TotalAmmo.ToString();
            
            // Store the weapon reference in the slot
            slotInstance.WeaponReference = weapon;
            
            _weaponSlotUIs.Add(slotInstance);
            WeaponSlotsContainer.AddChild(slotInstance);
        }
        
        // Make all slots expand to fill the container equally
        foreach (var slot in _weaponSlotUIs)
        {
            slot.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        }
    }

    public void UpdateCurrentGunSprite() 
    {
        CurrentGunSprite.Texture = _player._equipedGun.WeaponTexture;
    }

    public void UpdateCurrentGunText()
    {
        CurrentGunText.Text = _player._equipedGun.WeaponName;
    }

    public void UpdateCurrentGunIndex()
    {
        CurrentGunIndexText.Text = (_player.WeaponSwitchComponent.AvailableWeapons.IndexOf(_player._equipedGun) + 1).ToString(); // goofy ah way to do it
    }

    public void UpdateAmmoInClipText()
    {
        AmmoInClipText.Text = _player._equipedGun.AmmoComponent._currentAmmo.ToString();
    }

    public void UpdateAmmoMaxText()
    {
        AmmoMaxText.Text = _player._equipedGun.AmmoComponent.TotalAmmo.ToString();
    }
}