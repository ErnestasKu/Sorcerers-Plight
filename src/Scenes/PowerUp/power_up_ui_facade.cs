using Godot;
using System;
using System.Collections.Generic;

public partial class power_up_ui_facade : Panel
{
    [Export] private item_choice IC1;
    [Export] private item_choice IC2;
    [Export] private item_choice IC3;
    private List<item_choice> ICList;
    private pause_ui PauseUI;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PauseUI = (pause_ui)GetParent().GetNode("Pause UI");
        ICList = new List<item_choice>();
        ICList.Add(IC1);
        ICList.Add(IC2);
        ICList.Add(IC3);
    }

    public void Activate()
    {
        // disables manual pause function while level up ui is active
        PauseUI.Disable();

        // activates ui and pauses game
        Visible = true;
        GetTree().Paused = true;

        // creates power ups
        foreach (item_choice IC in ICList)
        {
            IC.power_up = power_up_proxy.CreatePowerUp();
            IC.LoadInfo();
        }
    }

    public void ChoosePowerUp(Power_up power_Up)
    {
        foreach (item_choice IC in ICList)
        {
            if (IC.power_up == power_Up)
            {
                power_Up.ApplyEffect();
                // if power up is repeatable, it gets added back into item pool
                if (power_Up.Repeatable)
                    Globals.PowerUpPool.Add(power_Up);
            }
            else
                Globals.PowerUpPool.Add(IC.power_up);
        }
    }
    public void Deactivate()
    {
        PauseUI.Enable();
        Visible = false;
        GetTree().Paused = false;
    }
}
