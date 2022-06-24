using System;

public class Player
{
    private readonly Weapon _weapon;
    private readonly Movement _movement;

    public string Name { get; private set; }
    public int Age { get; private set; }

    public void Move()
    {
        //Do move
    }

    public void Attack()
    {
        //attack
    }

    public bool IsReloading()
    {
        throw new NotImplementedException();
    }
}

public class Weapon
{
    public float Cooldown { get; private set; }
    public int Damage { get; private set; }
}

public class Movement
{
    public float DirectionX { get; private set; }
    public float DirectionY { get; private set; }
    public float Speed { get; private set; }
}