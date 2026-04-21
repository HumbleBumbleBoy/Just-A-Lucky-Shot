using Godot;
using System;

public partial class BulletBounceComponent : Node
{
	[Export] public int Bounces;
	public int _bouncesLeft;

	public override void _Ready()
	{
        _bouncesLeft = Bounces;
	}

	// -----------------------------

	public void SetBounces(int amount)
    {
        Bounces = Mathf.Max(amount, 0);    // Cap bounces at 0
    }

    public void IncreaseBounces(int amount)
    {
        Bounces = Mathf.Max(Bounces + amount, 0);
    }

    public void DecreaseBounces(int amount)
    {
        Bounces = Mathf.Max(Bounces - amount, 0);
    }

	// -----------------------------

	public void SetBouncesLeft(int amount)
    {
        _bouncesLeft = Mathf.Max(amount, 0);    // Cap bounces at 0
    }

    public void IncreaseBouncesLeft(int amount)
    {
        _bouncesLeft = Mathf.Max(_bouncesLeft + amount, 0);
    }

    public void DecreaseBouncesLeft(int amount)
    {
        _bouncesLeft = Mathf.Max(_bouncesLeft - amount, 0);
    }

}
