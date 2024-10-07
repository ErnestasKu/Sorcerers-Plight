using Godot;
using System;


public partial class RegularEnemy : Enemy
{
    RegularEnemy()
    {
        Name = "Snake";
        MaxHP = 10;
        Damage = 2;
        Speed = 2;
    }
}