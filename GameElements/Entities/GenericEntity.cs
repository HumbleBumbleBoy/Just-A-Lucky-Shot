using System;
using Godot;

public partial class GenericEntity : CharacterBody2D    // IT IS IMPERATIVE THAT THIS **REMAINS EMPTY** OUTISDE OF THIS ONE FUNCTION AND ALL ENEMY CODE IS HANDLED IN THE GENERIC ENEMY AND PLAYER IS SEPRATE (spaghetti code)
{
    [Export] public HealthComponent HealthComponent;
}
