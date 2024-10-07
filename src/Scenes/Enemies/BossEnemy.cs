using Godot;
using System;

public partial class BossEnemy : Enemy
{
    BossEnemy()
    {
        Name = "Cerberus";
        MaxHP = 50;
        Damage = 5;
        Speed = 2.5f;
        DropRate = 100;
        Score = 10;
    }
}