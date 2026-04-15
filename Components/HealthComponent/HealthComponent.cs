using Godot;
using System;

public partial class HealthComponent : Node  // Note to self for this and all future components, keep them STRICT. This will just increase and decrese health PERIOD, no death managing or other effects.
{
    [Export] int MaxHealth;
    [Export] int DamageResistance;
    [Export] bool IsPercentageValue;
}
