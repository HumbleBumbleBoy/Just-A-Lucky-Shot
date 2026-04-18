using Godot;
using System;

public partial class HomingComponent : Node
{
    public GenericBullet GenericBullet;

    [Export] public bool CanHome;
    [Export] public float HomingStrength;
    public bool _currentCanHome = false;
    public float _currentHomingStrength;

    public override void _Ready()
    {
        _currentHomingStrength = HomingStrength;
    }

    // lowk write functions that use allat

    // -----------------------------

    public void EnableHoming()
    {
        _currentCanHome = true;
    }

    public void DisableHoming()
    {
        _currentCanHome = true;
    }

    // -----------------------------

    public void SetCurrentHomingStrength(float amount)
    {
        _currentHomingStrength = Mathf.Max(amount, 1f);    // idk how big the values should be yet
    }

    public void IncreaseCurrentHomingStrength(float amount)
    {
        _currentHomingStrength = Mathf.Max(_currentHomingStrength + amount, 1f);
    }

    public void DecreaseCurrentHomingStrength(float amount)
    {
        _currentHomingStrength = Mathf.Max(_currentHomingStrength - amount, 1f);
    }
}
