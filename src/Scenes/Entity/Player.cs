using Godot;
using System;

[GlobalClass]
public partial class Player : Entity
{
    //player stats
    public int ShotSpeed = 300;
    public double FireRate = 0.6;
    public float Range = 2;
    private double FireRateCooldown = 1;

    private double DashCooldown = 1;
    private double ActiveDashCooldown = 0;

    private double HPRegenDelay = 3;
    private double ActiveHPRegenDelay = 1;
    private double HPRegenAmount = 0.1;

    //bullet caster
    private PackedScene bulletScene;
    private Node2D caster;
    [Export] private power_up_ui_facade powerUpUI;
    [Export] private AudioStreamPlayer DashSFX;
    [Export] private AudioStreamPlayer ShotSFX;

    //other
    public bool hasPiercing = false;
    public bool hasDash = false;

    public override void _PhysicsProcess(double delta)
    {
        Dash(delta);
        Shoot(delta);
        RegenerateHP(delta);
        UpdateHealthbar();
        Move();
    }

    public override void _Ready()
    {
        Globals.player = this;
        Damage = 4;
        Speed = Speed * 100;
        base._Ready();
        caster = (Godot.Node2D)GetNode("Caster");
        bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet/bullet.tscn");
    }

    // Allows movement in 4 cardinal directions
    public override void Move()
    {
        if (IsMobile)
        {
            Vector2 velocity = Velocity;

            // Get the input direction and handle the movement/deceleration.
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            if (direction != Vector2.Zero)
            {
                velocity.X = direction.X * Speed;
                velocity.Y = direction.Y * Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
                velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
            }
            Velocity = velocity;
            caster.GlobalPosition = Position;
            MoveAndSlide();
        }
    }

    // Shoots a bullet at cursor
    public void Shoot(double delta)
    {
        FireRateCooldown -= delta;
        if (Input.IsActionPressed("ui_shoot") && FireRateCooldown <= 0)
        {
            FireRateCooldown = FireRate;
            Bullet bullet = bulletScene.Instantiate<Bullet>();
            bullet.piercing = hasPiercing;
            bullet.damage = Damage;
            bullet.lifespan = Range;
            bullet.speed = ShotSpeed;
            bullet.Position = caster.GlobalPosition;
            bullet.RotationDegrees = caster.RotationDegrees;
            GetParent().AddChild(bullet);
            ShotSFX.Play();
        }
        caster.LookAt(GetGlobalMousePosition());

    }

    // Activates player level up function
    public void LevelUp()
    {
        if (powerUpUI != null)
            powerUpUI.Activate();
    }

    // Dashes past enemies while making player briefly invincible
    private void Dash(double delta)
    {
        if (hasDash)
        {
            DashCooldown -= delta;
            if (Input.IsActionJustPressed("Dash") && DashCooldown <= 0)
            {
                DashCooldown = 1;
                IsMobile = false;
                IsDamageable = false;
                SetCollisionMaskValue(3, false);

                DashSFX.Play();
                Velocity = Velocity * 75;
                MoveAndSlide();

                IsMobile = true;
                IsDamageable = true;
                SetCollisionMaskValue(3, true);
            }
        }
    }

    // Activates player level up function
    public void RegenerateHP(double delta)
    {
        ActiveHPRegenDelay -= delta;
        if (ActiveHPRegenDelay <= 0 && HP < MaxHP)
            HP += HPRegenAmount;
    }

    public void ResetHPRegenDelay()
    {
        ActiveHPRegenDelay = HPRegenDelay;
    }

    public override void TakeDamage(double damage)
    {
        ResetHPRegenDelay();
        base.TakeDamage(damage);
    }

    // Marks player ready for damage
    public override void _on_hurtbox_area_entered(Area2D hitbox)
    {
        Enemy enemy = (Enemy)hitbox.GetParent();
        enemy.IsInHitboxRange = true;
    }

    // Unarks player ready for damage
    public override void _on_hurtbox_area_exited(Area2D hitbox)
    {
        Enemy enemy = (Enemy)hitbox.GetParent();
        enemy.IsInHitboxRange = false;
    }

    public override void Die()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Main/game_over.tscn");
    }
}
