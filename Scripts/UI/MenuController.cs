using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using StarDefense.Helpers;
using StarDefense.Attrubutes;

public class MenuController : NodeSingletone<MenuController>
{
    public static IEnumerable<BaseMenuCategory> AllCategories { get; private set; }
    private IList<BaseMenuCategory> _menus;
    private Control _currentMenu;

    protected override void OnAwake()
    {
        base.OnAwake();

        AllCategories = TypeHelper.WithClassExtends<BaseMenuCategory>();
        _menus = new List<BaseMenuCategory>();
        foreach (var category in AllCategories)
        {
            var nodeCategory = GetNode<Control>(category.PathName);
            if (nodeCategory == null) GD.PrintErr($"Menu not founded: {category.PathName}");
            category.SetPanel(nodeCategory);
            _menus.Add(category);
        }

        var main = _menus.Where(x => Attribute.IsDefined(x.GetType(), typeof(StartMenuAttribute))).First();
        GD.Print(main);
        main.Click();
    }

    public void RequireClick(string path)
    {
        var menu = _menus.Where(x => x.PathName == path).FirstOrDefault();
        if (menu == null) return;
        menu.Click();
    }

    public override MenuController CreateInstance() => this;
}
