using Godot;
using System;
using System.Collections.Generic;

// A bunch of global variables to make my life easier
public partial class Globals : Node
{
    public static main Main;
    public static Player player;
    public static List<Power_up> PowerUpPool;
    public static PackedScene PowerUpScene;

    // get random power up
    public static Power_up CreatePowerUp()
    {
        Random rand = new Random();
        int r_index = rand.Next(0, PowerUpPool.Count);
        return PowerUpPool[r_index];
    }
}
