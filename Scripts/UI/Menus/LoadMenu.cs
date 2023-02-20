using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoadMenu : BaseMenuCategory
{
    public override string PathName => "LoadMenu";

    private ButtonLoadMenu _sender;

    protected override bool _OnActivate(ButtonMenu sender)
    {
        base._OnActivate(sender);
        var loadSender = sender as ButtonLoadMenu;
        if (loadSender == null) return false;
        _sender = loadSender;

        return true;
    }

    protected override void _OnShow(ButtonMenu sender)
    {
        base._OnShow(sender);
        MapSwitcherController.Instance.Load(_sender.LoadingMap);
    }
}
