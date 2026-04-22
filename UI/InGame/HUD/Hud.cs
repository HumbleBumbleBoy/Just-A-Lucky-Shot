using Godot;
using System;

public partial class Hud : Control
{
	// Write a script that takes current gun stats and displays them in bottom right, add a function that will get called when shooting at the player script to update these values, also call it on reload, thanks
	[Export] TextureRect CurrentGunSprite;
	[Export] RichTextLabel CurrentGunText;
	[Export] RichTextLabel AmmoInClipText;
	[Export] RichTextLabel AmmoMaxText;
	public Player _player;

	public void InitialiseHud()
	{
		UpdateCurrentGunSprite();
		UpdateCurrentGunText();
		UpdateAmmoInClipText();
		UpdateAmmoMaxText();
	}

	public void UpdateCurrentGunSprite() 
	{
		CurrentGunSprite.Texture = _player._equipedGun.WeaponTexture;
	}

	public void UpdateCurrentGunText()
	{
		CurrentGunText.Text = _player._equipedGun.WeaponName;
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
