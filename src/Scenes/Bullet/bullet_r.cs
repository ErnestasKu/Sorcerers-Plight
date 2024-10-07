using Godot;
using System;

public partial class bullet_r : RigidBody2D
{
    //[Export] public RigidBody2D body;
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
        //target = GetGlobalMousePosition();
        //LinearVelocity = (GetViewport().GetMousePosition() - Position).Normalized() * speed;
        //GD.Print(GetViewport().GetMousePosition().X + "   " + GetViewport().GetMousePosition().Y);
    }

    public override void _PhysicsProcess(double delta)
    {
        if ((lifespan -= delta) <= 0)
            QueueFree();

        //GlobalPosition += Transform.X.Normalized() * (float)delta * speed;
    }

    public void _on_area_2d_area_entered()
    {
        QueueFree();
        GD.Print("AAAA");
    }

    //public void _on_rigid_body_2d_body_shape_entered()
    //{
    //	QueueFree();
    //       GD.Print("AAAA");
    //   }
}
