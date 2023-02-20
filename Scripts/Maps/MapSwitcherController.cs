using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class MapSwitcherController : NodeSingletone<MapSwitcherController>
{
    public string MapToLoad { get; set; }

    public void Load(string map)
    {
        MapToLoad = map;
        Load();
    }

    public void Load()
    {
        try
        {
            var scene = GD.Load<PackedScene>(MapToLoad);
            Load(scene);
        }
        catch (Exception)
        {
            GD.PrintErr($"Scene not exist ({MapToLoad})");
        }
    }

    public void Load(PackedScene scene)
    {
        GetTree().ChangeSceneTo(scene);
    }

    public override MapSwitcherController CreateInstance() => this;
}