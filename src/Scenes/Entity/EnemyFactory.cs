using Godot;
using System;

public partial class EnemyFactory : Node
{
    static private PackedScene RegularEnemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/regular_enemy.tscn");
    static private PackedScene SpeedyEnemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/speedy_enemy.tscn");
    static private PackedScene BossEnemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/boss_enemy.tscn");

    public Enemy CreateRegularEnemy()
    {
        Enemy enemy = RegularEnemyScene.Instantiate<RegularEnemy>();
        return enemy;
    }

    public Enemy CreateSpeedyEnemy()
    {
        Enemy enemy = SpeedyEnemyScene.Instantiate<SpeedyEnemy>();
        return enemy;
    }

    public Enemy CreateBossEnemy()
    {
        Enemy enemy = BossEnemyScene.Instantiate<BossEnemy>();
        return enemy;
    }
}