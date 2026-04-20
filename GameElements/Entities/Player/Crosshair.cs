using Godot;
using System;

public partial class Crosshair : Node2D
{
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }
    public override void _Process(double delta)
    {
        GlobalPosition = GetGlobalMousePosition();
    }
}
