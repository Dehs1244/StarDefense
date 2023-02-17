using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public sealed class Spawner : Spatial
{
	[Export]
	public string NpcRes { get; set; }

	private Timer _timer;

	public override void _Ready()
	{
		base._Ready();
		_timer = GetNode<Timer>("Timer");
		_timer.Connect("timeout", this, "Spawn");
	}

	public void Spawn()
	{
		PackedScene prefab = GD.Load<PackedScene>(NpcRes);
		var gameObj = prefab.Instance<BaseNpc>();
		//gameObj.SetNavigation(_navigation);
		gameObj.GlobalTransform = GlobalTransform;
		NpcController.Instance.AddChild(gameObj);
	}
}
