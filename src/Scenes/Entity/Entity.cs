using Godot;
using System;

// Acts as an abstract class
// -----------------------------------------------------------------------------
// However Godot doesn't support abstract classes, so the 'abstract' keyword is
// not usable
// -----------------------------------------------------------------------------
public partial class Entity : CharacterBody2D
{
    public string Name;
    public double MaxHP = 10;
    public double HP;
    public double Damage;
    public float Speed = 2;
    public bool IsDamageable = true;
    public bool IsMobile = true;
    public ProgressBar Healthbar;

    public virtual void Die()
    {
        // does nothing in base class
    }

    public virtual void Move()
    {
        if (IsMobile)
            MoveAndCollide(Velocity);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Healthbar = GetNode<ProgressBar>("Healthbar");
        HP = MaxHP;
    }

    public virtual void _on_hurtbox_area_entered(Area2D hitbox)
    {
        // to implement
    }
    public virtual void _on_hurtbox_area_exited(Area2D hitbox)
    {
        // to implement
    }
    
    public void UpdateHealthbar()
    {
        Healthbar.MaxValue = MaxHP;
        if (HP >= MaxHP)
            Healthbar.Visible = false;
        else
        {
            Healthbar.Visible = true;
            Healthbar.Value = HP;
        }
    }

    public virtual void TakeDamage(double damage)
    {
        if (IsDamageable)
        {
            HP -= Math.Abs(damage);

            if (HP <= 0)
                Die();
        }
    }
}
