using Godot;
using System;

public partial class HomingComponent : Node
{
    public GenericBullet GenericBullet;

    [Export] public float HomingStrength;
    public float _currentHomingStrength;

    public override void _Ready()
    {
        _currentHomingStrength = HomingStrength;
    }

    // lowk write functions that use allat

    // -----------------------------

    public void SetCurrentHomingStrength(float amount)
    {
        _currentHomingStrength = Mathf.Max(amount, 0f);    // idk how big the values should be yet
    }

    public void IncreaseCurrentHomingStrength(float amount)
    {
        _currentHomingStrength = Mathf.Max(_currentHomingStrength + amount, 0f);
    }

    public void DecreaseCurrentHomingStrength(float amount)
    {
        _currentHomingStrength = Mathf.Max(_currentHomingStrength - amount, 0f);
    }
}
