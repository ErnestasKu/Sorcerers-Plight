using Godot;
using System;

public partial class main_menu : Control
{
    [Export] private VBoxContainer Menu;
    [Export] private VBoxContainer Options;

    [Export] private HSlider MasterSlider;
    [Export] private HSlider MusicSlider;
    [Export] private HSlider SFXSlider;

    private int MasterAudio;
    private int MusicAudio;
    private int SFXAudio;

    public override void _Ready()
    {
        base._Ready();
        MasterAudio = AudioServer.GetBusIndex("Master");
        MusicAudio = AudioServer.GetBusIndex("Music");
        SFXAudio = AudioServer.GetBusIndex("SFX");

        MasterSlider.Value = Mathf.DbToLinear( AudioServer.GetBusVolumeDb(MasterAudio));
        MusicSlider.Value = Mathf.DbToLinear( AudioServer.GetBusVolumeDb(MusicAudio));
        SFXSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(SFXAudio));
    }

    public void _on_play_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Main/main.tscn");
    }

    public void _on_options_pressed()
    {
        Menu.Visible = false;
        Options.Visible = true;
    }

    public void _on_quit_pressed()
    {
        GetTree().Quit();
    }

    public void _on_master_slider_value_changed(float value)
    {
        AudioServer.SetBusVolumeDb(MasterAudio, Mathf.LinearToDb(value));
    }

    public void _on_music_slider_value_changed(float value)
    {
        AudioServer.SetBusVolumeDb(MusicAudio, Mathf.LinearToDb(value));
    }

    public void _on_sfx_slider_value_changed(float value)
    {
        AudioServer.SetBusVolumeDb(SFXAudio, Mathf.LinearToDb(value));
    }

    public void _on_back_pressed()
    {
        Options.Visible = false;
        Menu.Visible = true;
    }
}
