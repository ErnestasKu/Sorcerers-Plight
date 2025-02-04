using Godot;
using System;

public partial class Bullet : Node2D
{
	[Export] public RigidBody2D body;
	public double damage;
	public float speed;
	public double lifespan;
	public bool piercing;
	public PackedScene bullet;
	public Vector2 velocity;
	public Vector2 target;

    [Export] private Sprite2D sprite;

    public override void _Ready()
	{
        //body.LookAt(GetGlobalMousePosition());
        //target = GetGlobalMousePosition();

        //sprite = GetNode<Sprite2D>("Sprite");
    }

    public override void _PhysicsProcess (double delta)
	{
		if ((lifespan -= delta) <= 0)
			QueueFree();

		GlobalPosition += Transform.X.Normalized() * (float)delta * speed;

        sprite.Rotate((float)delta * Mathf.DegToRad(120));
    }
}
