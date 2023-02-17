using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class NpcController : NodeSingletone<NpcController>
{
	private List<INpcAgent> _agents = new List<INpcAgent>();
	public IEnumerable<INpcAgent> Agents => _agents;

	public void Subscribe(INpcAgent agent)
	{
		_agents.Add(agent);
	}

	public bool UnSubscribe(INpcAgent agent)
	{
		return _agents.Remove(agent);
	}

	public void OnCreateBuilding(IBlueNpcAgent agent)
	{
		_agents.ForEach(x =>
		{
			if (x is IRedNpcAgent red) red.InvokeOnCreateEnemy(agent);
		});
	}

	protected override void OnAwake()
	{
	}

	public override NpcController CreateInstance()
		=> this;
}
