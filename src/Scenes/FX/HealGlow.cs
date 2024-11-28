using Godot;
using System;

public partial class HealGlow : Node2D
{
    [Export] CpuParticles2D ForegroundEffect;
    [Export] CpuParticles2D BackgroundEffect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        StopHealParticles();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        BackgroundEffect.GlobalPosition = Globals.player.GlobalPosition;
    }

    public void StartHealParticles()
    {
        ForegroundEffect.Emitting = true;
        BackgroundEffect.Emitting = true;
    }

    public void StopHealParticles()
    {
        ForegroundEffect.Emitting = false;
        BackgroundEffect.Emitting = false;
    }
}
