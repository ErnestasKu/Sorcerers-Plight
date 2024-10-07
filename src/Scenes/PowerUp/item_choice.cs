using Godot;
using System;

public partial class item_choice : ColorRect
{
    public Power_up power_up;
    [Export] private Label ItemName;
    [Export] private Label Description;
    private Color back_color;
    private power_up_ui_facade MenuRoot;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (Node UI = this; UI.GetParent() != null; UI = UI.GetParent())
        {
            if (UI.Name == "Power Up UI")
            {
                MenuRoot = (power_up_ui_facade)UI;
                break;
            }
        }

        back_color = Color;
    }

    // Load item details
    public void LoadInfo()
    {
        ItemName.Text = power_up.Name;
        Description.Text = power_up.Description;
    }

    // Select item
    public void _on_gui_input(InputEvent input)
    {
        if (Input.IsActionJustPressed("ui_shoot"))
        {
            MenuRoot.ChoosePowerUp(power_up);
            MenuRoot.Deactivate();
        }
    }

    // Hover in - highlight
    public void _on_mouse_entered()
    {
        Color = Color.FromHtml("e23f00");
    }

    // Hover out - return color
    public void _on_mouse_exited()
    {
        Color = back_color;
    }
}
