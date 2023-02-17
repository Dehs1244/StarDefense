using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarDefense.Attrubutes;
using Godot;

[Building("GunTurret", "Турель", 120)]
public class TurretBuilding : BaseBuilding, IShootable
{
	public override uint MaxHealth => 200;

	public override int Damage => 40;

	public override uint Cost => 120;

	private float _shootTimer;

	private MeshInstance _center;
	private MeshInstance _body;

	private PackedScene _bulletPrefab;
	private List<Spatial> _shootTriggers = new List<Spatial>();

	public override void Init()
	{
		_center = GetChild(0).GetChild<MeshInstance>(0);
		_body = GetChild(0).GetChild<MeshInstance>(1);
		_bulletPrefab = GD.Load<PackedScene>(BaseShootableNpc.BULLET_PREFAB_PATH);

		foreach(Spatial trigger in _center.GetChildren())
		{
			_shootTriggers.Add(trigger);
		}
		base.Init();
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		_shootTimer += 1 * delta;
	}

	public override void InteractionWithEnemy(Spatial agent)
	{
		if (Player.Instance.Energy <= 5) return;
		base.InteractionWithEnemy(agent);
		Shoot(agent.Transform.origin);
		Player.Instance.Energy -= 5;
		if (Player.Instance.Energy < 0) Player.Instance.Energy = 0;
	}

	public override void LookAtPosition(Vector3 position)
	{
		var origin = _center.RotationDegrees;
		_center.LookAt(position, Vector3.Up);
		var degrees = _center.RotationDegrees;
		degrees.z = 0;
		degrees.y += 180;
		degrees.x = origin.x;
		_center.RotationDegrees = degrees;
	}

	protected override IEnumerable<MeshInstance> _GetMesh()
	{
		yield return _center;
		yield return _body;
	}

	public void Shoot(Vector3 dir)
	{
		LookAtPosition(dir);
		var root = GetTree().Root;

		if (_shootTimer > 2)
		{
			BaseShootableNpc.ShootGlobal(dir, _bulletPrefab, this, _shootTriggers, root);
			_shootTimer = 0;
		}
	}

	public void OnShoot(ShootContext context)
	{
		if (context.Building != null) context.Building.GetDamage(Damage);
		if (context.Agent != null) context.Agent.GetDamage(Damage);
	}
}
