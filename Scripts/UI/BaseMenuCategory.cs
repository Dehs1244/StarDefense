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
    private Control _panel;
    public static Control MainPanel { get; private set; }

    public void Click()
    {
        OnActivate();
        OnClicked?.Invoke();
        if (MainPanel == null) MainPanel = _panel;
        MainPanel.Visible = false;
        _panel.Visible = true;
        MainPanel = _panel;
    }

    public void SetPanel(Control panel)
    {
        _panel = panel;
    }

    public virtual void OnActivate()
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
