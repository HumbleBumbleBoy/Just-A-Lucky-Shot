using Godot;
using System;

public partial class GameScene : Node2D
{
    [Export] Hud hud;
    private Player player;
   
    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
        
        // Get ALL guns in the scene and assign player reference to each
        Godot.Collections.Array<Node> allGuns = GetTree().GetNodesInGroup("Gun");
        foreach (Node gunNode in allGuns)
        {
            if (gunNode is GenericGun gun)
            {
                gun.player = player;
            }
        }
        
        player.hud = hud;
        hud._player = player;
        hud.InitialiseHud();
    }
}