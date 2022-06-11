using System;

public struct Weapon
{
    private readonly int _damage;
    private int _bullets;

    public Weapon(int damage, int bullets)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        if (bullets < 0)
            throw new ArgumentOutOfRangeException();

        _damage = damage;
        _bullets = bullets;
    }

    public void Fire(IDamageable target)
    {
        if (_bullets > 0)
        {
            target.ApplyDamage(_damage);
            _bullets--;
        }
    }
}

public interface IDamageable 
{
    public void ApplyDamage(int damage);
}

public class Player : IDamageable
{
    private int _health;

    public Player(int health)
    {
        if (health < 0)
            throw new ArgumentOutOfRangeException();

        _health = health;
    }

    public void ApplyDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        if (_health - damage <= 0)
            _health = 0;
        else
            _health -= damage;
    }
}

public class Bot
{
    private Weapon _weapon;

    public Bot(Weapon weapon) 
        => _weapon = weapon;

    public void OnSeePlayer(Player player) 
        => _weapon.Fire(player);
}