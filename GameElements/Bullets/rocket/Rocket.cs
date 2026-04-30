using Godot;
using System;

public partial class Rocket : GenericBullet
{
	// set size of explosion on enter (use bullet size to scale)
	// enable explosion area on hitting the wall
	// redo art
	[Export] public float ExplosionSize;

	public override void _Ready() {
		// disable explosion area on enter
	}
}
