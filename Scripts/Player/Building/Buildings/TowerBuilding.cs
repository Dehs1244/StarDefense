using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using StarDefense.Attrubutes;

[Building("Tower", "Башня", 0)]
public class TowerBuilding : BaseBuilding
{
	public override uint MaxHealth => 500;

	public override int Damage => 25;

	public override uint Cost => 0;
	private Node _root;

	public override void Init()
	{
		_root = GetChild(0);
		base.Init();
	}

	protected override IEnumerable<MeshInstance> _GetMesh()
	{
		foreach(var child in _root.GetChildren())
		{
			if (child is MeshInstance) yield return child as MeshInstance;
		}
	}

	public override void OnSuccessBuild()
	{
		base.OnSuccessBuild();
		Player.Instance.SetMainTower(this);
		BuildingsController.Instance.RemoveIcon("Tower");
	}
}
