using Godot;
using System;

public partial class GameScene : Node2D
{
    [Export] Hud hud;
    private Player player;
    private GenericGun genericGun;
   
    public override void _Ready()
    {
        genericGun = GetTree().GetFirstNodeInGroup("Gun") as GenericGun;
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
        genericGun.player = player;
        player.hud = hud;
        hud._player = player;
        hud.InitialiseHud();

        
        
    }
}
