using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseShootableNpc : BaseNpc, IShootable
{
	public const string BULLET_PREFAB_PATH = "res://Prefabs/Bullet/Bullet.tscn";

	private IEnumerable<Spatial> _shootingTriggers;
	private PackedScene _bulletPrefab;
	
	public abstract void OnShoot(ShootContext context);

	public override void _Ready()
	{
		base._Ready();
		_bulletPrefab = GD.Load<PackedScene>(BULLET_PREFAB_PATH);

		_shootingTriggers = _InitShootingTriggers();
	}

	protected abstract IEnumerable<Spatial> _InitShootingTriggers();

	public void Shoot(Vector3 dir)
	{
		LookAtPosition(dir);
		var root = GetTree().Root;

		ShootGlobal(dir, _bulletPrefab, this, _shootingTriggers, root);
	}

	public static void ShootGlobal(Vector3 dir, PackedScene bulletPrefab, IShootable sender, IEnumerable<Spatial> triggers, Viewport root)
	{
		foreach (var trigger in triggers)
		{
			var bullet = bulletPrefab.Instance<BulletController>();
			bullet.GlobalTransform = trigger.GlobalTransform;
			bullet.SetSender(sender);
			bullet.SetBulletDirection(dir);
			root.AddChild(bullet);
		}
	}
}
