using Godot;
using System;

public partial class Enemy : Entity
{
	public Player Target;
	public int Score = 1;
	public double HitRate = 1;
	public double HitDelay = 0;
	public bool IsInHitboxRange = false;
	public int DropRate = 5;
	//[Export] public PackedScene BloodScene;// = (PackedScene)ResourceLoader.Load("res://Scenes/FX/blood_splatter.tscn");
	//static private PackedScene BloodScene = GD.Load<PackedScene>("res://Scenes/FX/blood_splatter.tscn");
	[Export] private PackedScene BloodScene;

	public override void _Ready()
	{
		base._Ready();
		Target = Globals.player;
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (HitDelay > 0)
			HitDelay -= _delta;
		else
			HitDelay = 0;

		if (IsInHitboxRange && HitDelay <= 0)
		{
			HitDelay = HitRate;
			Target.TakeDamage(Damage);
		}

		UpdateHealthbar();
		Move();
	}

	public override void Move()
	{
		var direction = GlobalPosition.DirectionTo(Target.GlobalPosition);
		Velocity = direction * Speed;
		base.Move();
	}

	public override void Die()
	{
		Globals.Main.IncreaseScoreBy(Score);
		Globals.Main.UpdateScoreboard();
		Random random = new Random();
		if (random.Next(99) < DropRate)
		{
			PowerUpDrop powerUpDrop = Globals.PowerUpScene.Instantiate<PowerUpDrop>();
			Globals.Main.CallDeferred(Node.MethodName.AddChild, powerUpDrop);
			powerUpDrop.GlobalPosition = GlobalPosition;
		}
		QueueFree();

	}

	public override void _on_hurtbox_area_entered(Area2D hitbox)
	{
		BloodSplatter blood = BloodScene.Instantiate<BloodSplatter>();
		GetTree().CurrentScene.AddChild(blood);
		blood.GlobalPosition = GlobalPosition;
		blood.Rotation = GlobalPosition.AngleToPoint(Globals.player.GlobalPosition) + Mathf.Pi;
		//blood.Scale = new Vector2(25, 25);

		Bullet bullet = (Bullet)hitbox.GetParent();
		TakeDamage(bullet.damage);

		//check for piercing
		if (!bullet.piercing)
			bullet.QueueFree();
	}
}
