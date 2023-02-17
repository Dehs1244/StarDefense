using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public sealed class BuildingResourceDescriptor : IResourceDescriptor<BaseBuilding>
{
    public BaseBuilding Create(string name)
    {
        var packedScene = GD.Load<PackedScene>($"res://Prefabs/Buildings/{name}.tscn");
        var building = packedScene.Instance<BaseBuilding>();
        building._EnterTree();
        //building._Ready();
        building.Init();
        return building;
    }

    public string GetIconPath(string name, string extension)
        => $"res://Assets/Icons/{name}.{extension}";

    public Texture CreateIcon(string name, string extension)
        => ResourceLoader.Load<Texture>(GetIconPath(name, extension));
}
