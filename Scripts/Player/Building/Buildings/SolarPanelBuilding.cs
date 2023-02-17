using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarDefense.Attrubutes;
using Godot;

[Building("SolarPanel", "Солнечная панель", 125)]
public class SolarPanelBuilding : BaseBuilding
{
	public override uint MaxHealth => 50;

	public override int Damage => 1;

	public override uint Cost => 125;

	private Timer _timer;

	public override void _Ready()
	{
		_timer = GetChild<Timer>(1);
		_timer.Connect("timeout", this, "_AddEnergy");
		_timer.Start();
	}

	private void _AddEnergy()
	{
		Player.Instance.Energy += 4;
	}

	protected override IEnumerable<MeshInstance> _GetMesh()
	{
		foreach(MeshInstance mesh in GetChild(0).GetChildren())
		{
			yield return mesh;
		}
	}
}
