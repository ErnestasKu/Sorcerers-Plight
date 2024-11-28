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

    //private double DashCooldown = 1;
    //private double ActiveDashCooldown = 0;
    private float dashSpeed = 1500;
    public bool CanDash = true;
    private bool IsDashing = false;

    private double HPRegenDelay = 3;
    private double ActiveHPRegenDelay = 1;
    private double HPRegenAmount = 0.02;
    private bool healParticlesEnabled = false;

    private float MoveSpeed;

    //bullet caster
    private PackedScene bulletScene;
    private Node2D caster;
    [Export] private Timer DashTimer;
    [Export] private Timer DashCooldownTimer;
    [Export] private Timer DashGhostTimer;

    [Export] private Timer HealDelayTimer;
    [Export] private Timer HealTickTimer;
    [Export] private HealGlow HealEffect;

    [Export] private power_up_ui_facade powerUpUI;
    [Export] private AudioStreamPlayer DashSFX;
    [Export] private AudioStreamPlayer ShotSFX;

    [Export] private PackedScene DashScene;
    [Export] private Sprite2D Sprite;

    //other
    public bool hasPiercing = false;
    public bool hasDash = false;

    public override void _PhysicsProcess(double delta)
    {
        Dash(delta);
        Shoot(delta);
        //RegenerateHP(delta);
        if (HP < MaxHP && HealDelayTimer.IsStopped())
        {
            HealDelayTimer.Start();
            Console.WriteLine("Timer started!!!!!!!!!!!!!!!!!!!");

        }
        UpdateHealthbar();
        Move();
    }

    public override void _Ready()
    {
        Globals.player = this;
        Damage = 4;
        Speed = 200;
        base._Ready();
        caster = (Godot.Node2D)GetNode("Caster");
        bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet/bullet.tscn");
        DashScene = GD.Load<PackedScene>("res://Scenes/FX/dashEffect.tscn");
    }

    // Allows movement in 4 cardinal directions
    public override void Move()
    {
        if (IsMobile)
        {
            Vector2 velocity = Velocity;

            if (IsDashing)
                MoveSpeed = dashSpeed;
            else
                MoveSpeed = Speed;


            // Get the input direction and handle the movement/deceleration.
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            if (direction != Vector2.Zero)
            {
                velocity.X = direction.X * MoveSpeed;
                velocity.Y = direction.Y * MoveSpeed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, MoveSpeed);
                velocity.Y = Mathf.MoveToward(Velocity.Y, 0, MoveSpeed);
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
        if (Input.IsActionJustPressed("Dash")
            //&& DashCooldown <= 0
            && CanDash
            && hasDash)
        {
            CanDash = false;
            IsDashing = true;
            DashTimer.Start();
            DashGhostTimer.Start();
        }
    }

    private void InstantiateGhost()
    {
        DashEffect ghost = DashScene.Instantiate<DashEffect>();
        GetParent().AddChild(ghost);

        ghost.GlobalPosition = GlobalPosition;
        ghost.Texture = Sprite.Texture;
    }

    public void _on_dash_timer_timeout()
    {
        IsDashing = false;
        DashGhostTimer.Stop();
        DashCooldownTimer.Start();
    }

    public void _on_dash_cooldown_timer_timeout()
    {
        CanDash = true;
    }

    public void _on_dash_repeat_timer_timeout()
    {
        InstantiateGhost();
    }

    //// Activates player level up function
    //public void RegenerateHP(double delta)
    //{
    //    ActiveHPRegenDelay -= delta;
    //    if (ActiveHPRegenDelay <= 0 && HP < MaxHP)
    //    {
    //        HP += HPRegenAmount;
    //        healParticlesEnabled = true;
    //    }
    //}

    public void ResetHPRegenDelay()
    {
        HealDelayTimer.Stop();
        HealDelayTimer.Start();
        HealTickTimer.Stop();
        //ActiveHPRegenDelay = HPRegenDelay;
    }

    public void _on_heal_tick_timer_timeout()
    {
        double hp = MaxHP * HPRegenAmount;

        if (MaxHP - HP <= hp)
        {
            HP = MaxHP;
            HealTickTimer.Stop();
            HealEffect.StopHealParticles();
        }
        else
            HP += hp;
    }

    public void _on_heal_delay_timer_timeout()
    {
        HealDelayTimer.Stop();
        BeginHealthRegen();
    }

    public void BeginHealthRegen()
    {
        if (HealTickTimer.IsStopped())
        {
            HealTickTimer.Start();
            HealEffect.StartHealParticles();
        }
    }

    public override void TakeDamage(double damage)
    {
        if (IsDashing)
            return;

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
