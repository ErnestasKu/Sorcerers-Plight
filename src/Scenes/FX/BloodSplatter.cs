using Godot;
using System;
using System.Diagnostics;

public partial class BloodSplatter : CpuParticles2D
{
    [Export] public Timer fadeTimer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        OneShot = true;
        Emitting = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void _on_timer_timeout()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        SetProcessInput(false);
        SetProcessInternal(false);
        SetProcessUnhandledInput(false);
        SetProcessUnhandledKeyInput(false);

        fadeTimer.Start();

        Tween tween = CreateTween();
        tween.TweenProperty(
            this,
            "modulate:a",
            0.0f,
            5
        );
    }


    public void _on_timer_fade_timeout()
    {
        QueueFree();
    }
}
