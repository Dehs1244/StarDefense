using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public abstract class BaseMenuCategory : IMenuCategory
{
    public abstract string PathName { get; }
    public event Action OnClicked;
    protected Control _panel;
    public static Control MainPanel { get; private set; }

    public void Click(ButtonMenu sender)
    {
        if (!_OnActivate(sender)) return;
        OnClicked?.Invoke();
        if (MainPanel == null) MainPanel = _panel;
        MainPanel.Visible = false;
        _panel.Visible = true;
        MainPanel = _panel;
        _OnShow(sender);
    }

    public void SetPanel(Control panel)
    {
        _panel = panel;
    }

    protected virtual bool _OnActivate(ButtonMenu sender)
    {
        return true;
    }

    protected virtual void _OnShow(ButtonMenu sender)
    {

    }

    public void WhenClicked(Action func)
    {
        OnClicked += func;
    }

    public void WhenClicked()
    {
        throw new NotImplementedException();
    }
}
