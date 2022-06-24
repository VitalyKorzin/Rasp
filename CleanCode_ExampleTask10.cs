using System;

public class Weapon
{
	private readonly int _bulletsInShot = 1;
	private int _bullets;

	public bool CanShoot() => _bullets >= _bulletsInShot;

	public void Shoot() => _bullets -= _bulletsInShot;
}