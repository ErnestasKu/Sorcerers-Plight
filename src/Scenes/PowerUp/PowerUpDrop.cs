using Godot;
using System;

public partial class PowerUpDrop : Area2D
{
    public void _on_area_entered(Area2D hitbox)
    {
        Globals.player.LevelUp();
        QueueFree();
    }
}
