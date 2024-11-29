using Godot;
using System;
using static Godot.TextServer;

public partial class Enemy : Entity
{
    public Player Target;
    public int Score = 1;
    public double HitRate = 1;
    public double HitDelay = 0;
    public bool IsInHitboxRange = false;
    public int DropRate = 5;
    private ShaderMaterial wobble;
    [Export] private PackedScene BloodScene;

    public override void _Ready()
    {
        base._Ready();
        Target = Globals.player;
        //wobble = (ShaderMaterial)GetNode<Sprite2D>("Sprite").Material;

       
        // Ensure each enemy has its unique ShaderMaterial
        Sprite2D sprite = GetNode<Sprite2D>("Sprite");
        sprite.Material = sprite.Material.Duplicate() as ShaderMaterial;
        wobble = (ShaderMaterial)sprite.Material;
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
        // damage
        Bullet bullet = (Bullet)hitbox.GetParent();
        TakeDamage(bullet.damage);

        // deformation parameters
        Vector2 direction = GlobalPosition - hitbox.GlobalPosition;
        Vector2 deformationDirection = direction.Normalized();
        Vector2 deformationScale = 0.3f * deformationDirection;

        // begin deform
        float deformDuration = 0.1f;
        DeformSprite(Vector2.Zero, deformationScale, deformDuration);

        // revert deform
        Timer timer = new Timer();
        AddChild(timer);
        timer.WaitTime = deformDuration;
        timer.OneShot = true;
        timer.Timeout += () => DeformSprite(deformationScale, Vector2.Zero, deformDuration);
        timer.Start();

        // blood splatter
        BloodSplatter blood = BloodScene.Instantiate<BloodSplatter>();
        GetTree().CurrentScene.AddChild(blood);
        blood.GlobalPosition = GlobalPosition;
        blood.Rotation = GlobalPosition.AngleToPoint(hitbox.GlobalPosition) + Mathf.Pi;

        //check for piercing
        if (!bullet.piercing)
            bullet.QueueFree();
    }

    private void DeformSprite(Vector2 start, Vector2 end, float duration)
    {
        Tween tween = CreateTween();
        tween.TweenMethod(Callable.From<Vector2>(DeformShaderCall), start, end, duration);
    }

    private void DeformShaderCall(Vector2 deformationScale)
    {
        wobble.SetShaderParameter("deformation", deformationScale);
    }
}
