using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class ExitButton : Button
{
	public override void _EnterTree()
	{
		base._EnterTree();

		Connect("button_up", this, "_Exit");
	}

	private void _Exit()
	{
		GetTree().Quit();
	}
} 
