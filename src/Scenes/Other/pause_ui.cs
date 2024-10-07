using Godot;
using System;

public partial class pause_ui : Panel
{
    [Export] private Label MaxHP;
    [Export] private Label Damage;
    [Export] private Label Speed;
    [Export] private Label FireRate;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_pause"))
        {
            if (!Visible)
                PauseGame();
            else
                UnpauseGame();
        }
    }

    // Pauses game
    public void PauseGame()
    {
        this.Visible = true;
        GetTree().Paused = true;
        UpdateStats();
    }

    // Unpauses game
    public void UnpauseGame()
    {
        this.Visible = false;
        GetTree().Paused = false;
    }

    // Shows player stats on pause
    public void UpdateStats()
    {
        MaxHP.Text = "Max HP: " + Globals.player.MaxHP.ToString();
        Damage.Text = "Damage: " + Globals.player.Damage.ToString();
        Speed.Text = "Speed: " + (Math.Round(Globals.player.Speed / 100, 1)).ToString();
        FireRate.Text = "Fire rate: " + (Math.Round(Globals.player.FireRate, 1)).ToString();
    }

    public void Disable()
    {
        ProcessMode = ProcessModeEnum.Disabled;
    }


    public void Enable()
    {
        ProcessMode = ProcessModeEnum.Always;
    }

    public void _on_resume_pressed()
    {
        UnpauseGame();
    }

    public void _on_quit_pressed()
    {
        UnpauseGame();
        GetTree().ChangeSceneToFile("res://Scenes/Main/main_menu.tscn");
    }
}
