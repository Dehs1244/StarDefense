using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class DroneNpc : BaseRedShootableNpc
{
	private INpcAgent _shootTarget;
	private float _timer;

	public override void _Ready()
	{
		base._Ready();
		Health = 300;
	}

	public override void OnActivateAi()
	{
		base.OnActivateAi();
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (_shootTarget != null && _shootTarget.Health < 20) _shootTarget = null;
		if (_shootTarget == null) return;

		_timer += 1 * delta;

		if(_timer > 2)
		{
			Shoot(_shootTarget.GetPosition());
			_timer = 0;
		}
	}

	protected override bool _CanChangeTarget()
	{
		return _shootTarget == null;
	}

	public override void InteractWithEnemy(IBlueNpcAgent npc)
	{
		StopMove();
		LookAtAgent(npc);
		_shootTarget = npc;
	}

	public override void InteractWithFriendly(IRedNpcAgent npc)
	{
	}

	public override void OnShoot(ShootContext context)
	{
		INpcAgent agent = context.Agent;
		if (context.NpcDetector != null) agent = context.NpcDetector.GetCollisionOwner() as INpcAgent;
		if (agent == null) return;
		agent.GetDamage(10);
		if (agent.Health <= 0 && _shootTarget == agent)
		{
			_shootTarget = null;
			_target = null;
		}
	}

	protected override IEnumerable<Spatial> _InitShootingTriggers()
	{
		foreach(Spatial trigger in GetNode("ShootTriggers").GetChildren())
		{
			yield return trigger;
		}
	}
}
