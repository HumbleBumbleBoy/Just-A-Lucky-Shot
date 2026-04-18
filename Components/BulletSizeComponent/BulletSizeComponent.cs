using Godot;
using System;

public partial class BulletSizeComponent : Node
{
    public GenericBullet GenericBullet;
    [Export] public float MinBulletSize;
    [Export] public float BulletSize;
    [Export] public float MaxBulletSize;
    public float CurrentBulletSize;

    public override void _Ready()
    {
        CurrentBulletSize = BulletSize;
    }

    public void ApplyBulletSize()
    {
        // Yeah, write this when you have a bain again, thanks and have fun
    }
    
    // -----------------------------

    public void SetCurrentBulletSize(float amount)
    {
        CurrentBulletSize = Mathf.Clamp(amount, MinBulletSize, MaxBulletSize);    // Clamp bullet size
    }

    public void IncreaseCurrentBulletSize(float amount)
    {
        CurrentBulletSize = Mathf.Clamp(CurrentBulletSize + amount, MinBulletSize, MaxBulletSize);
    }

    public void DecreaseCurrentBulletSize(float amount)
    {
        CurrentBulletSize = Mathf.Clamp(CurrentBulletSize - amount, MinBulletSize, MaxBulletSize);
    }

}
