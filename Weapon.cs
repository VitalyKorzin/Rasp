using System;

public struct Weapon
{
    private readonly int _bulletsPerShot;
    private readonly int _damage;
    private int _bullets;

    public Weapon(int damage, int bullets, int bulletsPerShot)
    {
        if (damage <= 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (bullets <= 0)
            throw new ArgumentOutOfRangeException(nameof(bullets));

        if (bulletsPerShot <= 0)
            throw new ArgumentOutOfRangeException(nameof(bulletsPerShot));

        _damage = damage;
        _bullets = bullets;
        _bulletsPerShot = bulletsPerShot;
    }

    public void Fire(Player target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        if (target.Alive)
            target.ApplyDamage(_damage);

        _bullets -= _bulletsPerShot;
    }

    public bool CanFire() => _bullets >= _bulletsPerShot;
}

public class Player
{
    private int _health;

    public Player(int health)
    {
        if (health <= 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        _health = health;
    }

    public bool Alive => _health > 0;

    public void ApplyDamage(int damage)
    {
        if (damage <= 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (_health < damage)
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
    {
        if (_weapon.CanFire)
            _weapon.Fire(player);
    }
}