using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using StarDefense.Attrubutes;

[Building("Mine", "Шахта", 20)]
public class MineBuilding : BaseBuilding
{
	public override uint MaxHealth => 40;

	public override int Damage => 1;

	public override uint Cost => 20;
	private RandomNumberGenerator _random;

	private MeshInstance _mainMesh;
	private Timer _timer;

	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_random = new RandomNumberGenerator();
		_timer.Connect("timeout", this, "_AddMoney");
		_timer.Start();
	}

	public override void Init()
	{
		_mainMesh = GetNode<MeshInstance>("RootNode/Sketchfab_model/Collada visual scene group/root/LowPolyCrate/defaultMaterial");
		base.Init();
	}

	public void _AddMoney()
	{
		Player.Instance.Credits += _random.RandiRange(5, 15);
	}

	protected override IEnumerable<MeshInstance> _GetMesh()
	{
		yield return _mainMesh;
	}
}
