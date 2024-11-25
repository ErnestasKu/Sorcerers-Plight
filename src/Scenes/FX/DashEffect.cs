using Godot;
using System;

public partial class DashEffect : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var tween = GetTree().CreateTween();

        tween.SetTrans(Tween.TransitionType.Quart);
        tween.SetEase(Tween.EaseType.Out);
        tween.TweenProperty(this, "modulate:a", 0.0f, 0.5f);

        //tween.TweenCallback(Callable.From(() => OnTweenFinished()));
    }

    //private void OnTweenFinished()
    //{
    //    GD.Print("Tween finished!");
    //}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
