using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseRedShootableNpc : BaseRedNpc, IShootable
{
	private IEnumerable<Spatial> _shootingTriggers;
	private PackedScene _bulletPrefab;

	public abstract void OnShoot(ShootContext context);

	public override void _Ready()
	{
		base._Ready();
		_bulletPrefab = GD.Load<PackedScene>(BaseShootableNpc.BULLET_PREFAB_PATH);

		_shootingTriggers = _InitShootingTriggers();
	}

	protected abstract IEnumerable<Spatial> _InitShootingTriggers();

	public void Shoot(Vector3 dir)
	{
		LookAtPosition(dir);
		var root = GetTree().Root;

		BaseShootableNpc.ShootGlobal(dir, _bulletPrefab, this, _shootingTriggers, root);
	}
}
