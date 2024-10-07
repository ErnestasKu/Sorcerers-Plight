using Godot;
using System;
using System.Collections.Generic;

public partial class power_up_proxy : Node
{
    // Creates a random power up and removes it from the available item pool
    public static Power_up CreatePowerUp()
    {
        Power_up power_Up = Globals.CreatePowerUp();

        // removes power up from pool of items
        Globals.PowerUpPool.Remove(power_Up);
        return power_Up;
    }
}
