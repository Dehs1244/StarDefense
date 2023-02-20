using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class ButtonMenu : Button
{
    [Export]
    public string PathCategory { get; set; }

    public override void _EnterTree()
    {
        base._EnterTree();

        Connect("button_up", this, "_Click");
    }

    protected virtual void _Click()
    {
        MenuController.Instance.RequireClick(PathCategory, this);
    }
}