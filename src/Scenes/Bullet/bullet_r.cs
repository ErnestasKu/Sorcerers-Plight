using Godot;
using System;

public partial class bullet_r : RigidBody2D
{
    public float speed = 400;
    public double lifespan = 1;
    public bool hasPiercing = false;
    public PackedScene bullet;
    public Vector2 velocity;
    public Vector2 target;

    public override void _Ready()
    {
        LookAt(GetGlobalMousePosition());
        velocity.X = 0;
        velocity.Y = 0;
        target.X = speed;
        target.Y = 0;
        ApplyImpulse(velocity);
    }

    public override void _PhysicsProcess(double delta)
    {
        if ((lifespan -= delta) <= 0)
            QueueFree();
    }

    public void _on_area_2d_area_entered()
    {
        QueueFree();
    }
}
