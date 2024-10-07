using Godot;
using System;

public enum Effect
{
    None = 1,
    MaxHPUp = 2,
    DamageUp = 3,
    SpeedUp = 4,
    FireRateUp = 5,
    Piercing = 6,
    Dash = 7
}

[GlobalClass]
public partial class Power_up : Resource
{
    public Power_up() { }

    [Export] public string Name;
    [Export] public Effect effect;
    [Export] public string Description;
    [Export] public bool Repeatable = true;
    [Export] public bool Available = true;

    public void ApplyEffect()
    {
        //Player player = (Player)GetTree().Root.GetNode("Main/Player");
        Player player = Globals.player;
        switch (effect)
        {
            case Effect.Piercing:
                player.hasPiercing = true;
                break;
            case Effect.Dash:
                player.hasDash = true;
                break;
            case Effect.DamageUp:
                player.Damage += 2;
                break;
            case Effect.MaxHPUp:
                player.MaxHP += 5;
                player.HP = player.MaxHP;
                break;
            case Effect.SpeedUp:
                player.Speed += 20f;
                break;
            case Effect.FireRateUp:
                if (player.FireRate >= 0.19)
                    player.FireRate -= 0.1;
                else
                    player.FireRate = 0.1;
                break;
            default:
                break;
        }
    }
}
