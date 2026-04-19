using Godot;
using System;

public partial class AmmoComponent : Node   // Add signals later
{
    public GenericGun GenericGun;
    [Export] public int TotalAmmo;  // How many bullets a gun has ie. 100 (If i want to implement infinite ammo for an enemy i can just set it to 99999 or something)
    [Export] public int ClipSize;   // How many bullets you can hold at a time ie. 20 cap with 100 total would be 20/80
    [Export] public float ReloadTime;   // In seconds
    public int _currentAmmo;
    private bool _isReloading = false;
    private Timer _reloadTimer;

    public override void _Ready()
    {
        _currentAmmo = ClipSize;

        _reloadTimer = new Timer(); // Setup timed reload behaviour
        AddChild(_reloadTimer);
        _reloadTimer.OneShot = true;
        _reloadTimer.Timeout += OnReloadComplete;
    }

    public override void _ExitTree()
    {
        if (_reloadTimer != null)   // If you're reloading but switch guns, enable reload on weapon again so it doesn't become unreloadable
        {
            _reloadTimer.Stop();
            _reloadTimer.Timeout -= OnReloadComplete;
            _isReloading = false;
        }
    }

    public void Reload()
    {
        if (_isReloading) return;
        if (_currentAmmo >= ClipSize) { /* Play fail reload animation */ return; };
        if (TotalAmmo <= 0) { /* Play fail reload animation */ return; };
        
        _isReloading = true;
        _reloadTimer.Start(ReloadTime);
        // Play reload animation
    }

    private void OnReloadComplete()
    {
        if (!IsInsideTree()) return;    // Check if gun still exists
        if (TotalAmmo <= 0) { _isReloading = false; return;}    // If the gun SOMEHOW runs out of ammo mid reload, don't reload

        int needed = ClipSize - _currentAmmo;
        int toReload = Math.Min(needed, TotalAmmo);
        DecreaseTotalAmmo(toReload);
        IncreaseCurrentAmmo(toReload);
        _isReloading = false;
    }

    // -----------------------------

    public void SetCurrentAmmo(int amount)
    {
        _currentAmmo = Mathf.Clamp(amount, 0, ClipSize);
    }

    public void IncreaseCurrentAmmo(int amount)
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, ClipSize);
    }

    public void DecreaseCurrentAmmo(int amount)
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo - amount, 0, ClipSize);
    }

    // -----------------------------

    public void SetClipSize(int amount)
    {
        ClipSize = Math.Max(1, amount); // You always have at least 1 bullet in the mag;
    }

    public void IncreaseClipSize(int amount)
    {
        ClipSize += amount;
    }

    public void DecreaseClipSize(int amount)
    {
        ClipSize = Math.Max(1, ClipSize - amount); // You always have at least 1 bullet in the mag
    }

    // -----------------------------

    public void SetTotalAmmo(int amount)
    {
        TotalAmmo = Mathf.Max(0, amount);
    }

    public void IncreaseTotalAmmo(int amount)
    {
        TotalAmmo += amount;
    }

    public void DecreaseTotalAmmo(int amount)
    {
        TotalAmmo = Math.Max(0, TotalAmmo - amount);
    }
}
