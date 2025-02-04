using Godot;
using Microsoft.VisualBasic;
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
    [Export] private ShaderMaterial deathShader;
    [Export] private PackedScene BloodScene;

    public override void _Ready()
    {
        base._Ready();
        Target = Globals.player;

        // each enemy has its unique ShaderMaterial
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

        if (IsInHitboxRange && HitDelay <= 0 && isAlive)
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
        // begin death
        IsMobile = false;
        isAlive = false;

        Globals.Main.IncreaseScoreBy(Score);
        Globals.Main.UpdateScoreboard();
        Random random = new Random();
        if (random.Next(99) < DropRate)
        {
            PowerUpDrop powerUpDrop = Globals.PowerUpScene.Instantiate<PowerUpDrop>();
            Globals.Main.CallDeferred(Node.MethodName.AddChild, powerUpDrop);
            powerUpDrop.GlobalPosition = GlobalPosition;
        }

        // get shader
        Sprite2D sprite = GetNode<Sprite2D>("Sprite");
        sprite.Material = deathShader;
        sprite.Material = sprite.Material.Duplicate() as ShaderMaterial;
        deathShader = (ShaderMaterial)sprite.Material;

        // duration
        float animTime1 = 0.4f;
        float animTime2 = 0.8f;

        // implode
        Implode(animTime1);

        // disintegrate
        Timer timer = new Timer();
        AddChild(timer);
        timer.WaitTime = animTime1;
        timer.OneShot = true;
        timer.Start();
        timer.Timeout += () => Explode(animTime2 - 0.2f);

        // queue free
        Timer timerQ = new Timer();
        AddChild(timerQ);
        timerQ.WaitTime = animTime2;
        timerQ.OneShot = true;
        timerQ.Start();
        timerQ.Timeout += QueueFree;
    }

    public override void _on_hurtbox_area_entered(Area2D hitbox)
    {
        if (!isAlive)
            return;

        // damage
        Bullet bullet = (Bullet)hitbox.GetParent();
        TakeDamage(bullet.damage);

        // deformation parameters
        Vector2 direction = (GlobalPosition - hitbox.GlobalPosition).Normalized();
        Vector2 deformation = direction * 0.3f;
        float deformDuration = 0.1f;

        // begin deform
        DeformSprite(Vector2.Zero, deformation, deformDuration);

        // revert deform
        Timer timer = new Timer();
        AddChild(timer);
        timer.WaitTime = deformDuration;
        timer.OneShot = true;
        timer.Timeout += () => DeformSprite(deformation, Vector2.Zero, deformDuration);
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

    // deformation
    private void DeformSprite(Vector2 start, Vector2 end, float duration)
    {
        Tween tween = CreateTween();
        tween.TweenMethod(Callable.From<Vector2>(DeformShaderCall), start, end, duration);
    }

    private void DeformShaderCall(Vector2 deformationScale)
    {
        wobble.SetShaderParameter("deformation", deformationScale);
    }

    // implosion
    private void Implode(float duration)
    {
        Tween tween1 = CreateTween();
        Tween tween2 = CreateTween();
        tween1.TweenMethod(Callable.From<float>(SwirlShaderCall), 0f, 1f, duration);
        tween2.TweenMethod(Callable.From<float>(ScaleShaderCall), 1f, 0f, duration);
        //tween.TweenMethod(Callable.From<float>(IMPLODE_CALL), start, end, duration);
    }

    // disintegration
    private void Explode(float duration)
    {
        //Tween tween = CreateTween();
        //SwirlShaderCall(0);
        //ScaleShaderCall(1);
        //tween.TweenMethod(Callable.From<float>(EXPLODE_CALL), start, end, duration);

        SwirlShaderCall(0f);
        Tween tween0 = CreateTween();
        tween0.TweenMethod(Callable.From<float>(SwirlShaderCall), 0f, 0f, duration);
        Tween tween1 = CreateTween();
        Tween tween2 = CreateTween();
        tween1.TweenMethod(Callable.From<float>(ScaleShaderCall), 0f, 4f, duration);
        tween2.TweenMethod(Callable.From<float>(DisintegrateShaderCall), 1f, -1f, duration);
    }

    private void IMPLODE_CALL(float value)
    {
        SwirlShaderCall(value);
        ScaleShaderCall(1f - value);
    }

    private void EXPLODE_CALL(float value)
    {
        ScaleShaderCall(2 - (value + 1));
        DisintegrateShaderCall(value);
    }

    private void ScaleShaderCall(float value)
    {
        deathShader.SetShaderParameter("scale", value);
    }

    private void SwirlShaderCall(float value)
    {
        deathShader.SetShaderParameter("swirl", value);
    }

    private void DisintegrateShaderCall(float value)
    {
        deathShader.SetShaderParameter("dissolve", value);
    }
}
