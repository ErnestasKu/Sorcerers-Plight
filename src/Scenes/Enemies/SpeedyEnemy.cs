using Godot;
using System;

public partial class SpeedyEnemy : Enemy
{
    SpeedyEnemy()
    {
        Name = "Bat";
        MaxHP = 1;
        Damage = 4;
        Speed = 4;
    }
}