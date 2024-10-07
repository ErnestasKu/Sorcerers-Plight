using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using static System.Formats.Asn1.AsnWriter;

public partial class main : Node2D
{
    [Export] public Label Scoreboard;
    private ScoreSingleton scoreObject;
    private Vector2 Resolution;
    private double SpawnDelay = 1;
    private double ActiveDelay = 3;
    private int ScoreScaling = 50;
    Random rand = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Globals.player = (Player)GetNode("Player");
        Globals.Main = this;
        scoreObject = GetNode<ScoreSingleton>("/root/ScoreSingleton");
        scoreObject.score = 0;
        UpdateScoreboard();
        Globals.PowerUpScene = GD.Load<PackedScene>("res://Scenes/PowerUp/power_up.tscn");
        Resolution = GetViewport().GetVisibleRect().Size;
        LoadPowerUpPool();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ActiveDelay -= delta;

        if (ActiveDelay <= 0)
        {
            ActiveDelay = SpawnDelay;

            EnemyFactory factory = new EnemyFactory();
            Enemy enemy;
            int r_index = rand.Next(99);

            if (r_index < 5)
                enemy = factory.CreateBossEnemy();
            else if (r_index < 30)
                enemy = factory.CreateSpeedyEnemy();
            else
                enemy = factory.CreateRegularEnemy();

            enemy.GlobalPosition = GetRandomSpawnLocation();
            AddChild(enemy);
        }
    }

    public Vector2 GetRandomSpawnLocation()
    {
        int x = (int)Resolution.X / 2;
        int y = (int)Resolution.Y / 2;

        float randX = Globals.player.GlobalPosition.X;
        float randY = Globals.player.GlobalPosition.Y;

        var rand = new Random();
        int edge = rand.Next(1, 4);
        switch (edge)
        {
            case 1:     //left
                randX += -x;
                randY += rand.Next(-y, y);
                break;
            case 2:     //right
                randX += x;
                randY += rand.Next(-y, y);
                break;
            case 3:     //top
                randX += rand.Next(-x, x);
                randY += -600;
                break;
            default:    //bottom
                randX += rand.Next(-x, x);
                randY += -600;
                break;
        }
        return new Vector2(randX, randY);
    }

    private List<string> GetResourceList(string path)
    {
        List<string> fileList = new List<string>();
        using var dir = DirAccess.Open(path);
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (!dir.CurrentIsDir())
                    fileList.Add(fileName);

                fileName = dir.GetNext();
            }
        }
        else
        {
            GD.Print("An error occurred when trying to access the path");
        }
        return fileList;
    }

    private void LoadPowerUpPool()
    {
        List<string> fileList = GetResourceList("res://Scenes/PowerUp/PowerUpList");
        Globals.PowerUpPool = new List<Power_up>();
        foreach (string fileName in fileList)
        {
            Globals.PowerUpPool.Add(GD.Load<Power_up>("res://Scenes/PowerUp/PowerUpList/" + fileName.TrimSuffix(".remap")));
        }
    }

    public void IncreaseScoreBy(int scoreAmount)
    {
        scoreObject.score += scoreAmount;
    }

    public void UpdateScoreboard()
    {
        Scoreboard.Text = "SCORE:" + scoreObject.score;

        if (scoreObject.score >= ScoreScaling && SpawnDelay > 0.1)
        {
            ScoreScaling *= 2;
            SpawnDelay /= 2;
        }
    }
}