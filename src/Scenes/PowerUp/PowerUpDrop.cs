using Godot;
using System;

public partial class PowerUpDrop : Area2D
{
    [Export] float NucleusSpinSpeed;
    [Export] Sprite2D Nucleus;
    [Export] Timer NucleusTimer;

    public override void _Ready()
    {
        NucleusTimer.WaitTime = NucleusSpinSpeed;
        NucleusTimer.Timeout += LayerChange;
        ShaderMaterial sm = Nucleus.Material as ShaderMaterial;
        sm.SetShaderParameter("speed", NucleusSpinSpeed);
    }

    public void _on_area_entered(Area2D hitbox)
    {
        Globals.player.LevelUp();
        QueueFree();
    }

    private void LayerChange()
    {
        //Nucleus.ZIndex *= -1;
    }
}
