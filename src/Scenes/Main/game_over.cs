using Godot;
using System;

public partial class game_over : Control
{
    [Export] Label Scoreboard;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        ScoreSingleton scoreObject = GetNode<ScoreSingleton>("/root/ScoreSingleton");
        Scoreboard.Text = ("SCORE:" + scoreObject.score);
	}

	public void _on_restart_pressed()
	{
        GetTree().ChangeSceneToFile("res://Scenes/Main/main.tscn");
    }

    public void _on_to_main_menu_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Main/main_menu.tscn");
    }

    public void _on_quit_pressed()
    {
        GetTree().Quit();
    }
}
