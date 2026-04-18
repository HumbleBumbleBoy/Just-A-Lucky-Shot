using Godot;
using System;

public partial class FiringComponent : Node
{
    [Export] public float FireRate;
    [Export] public int BulletsPerShot;
    [Export] public float SpreadDegrees;
}
