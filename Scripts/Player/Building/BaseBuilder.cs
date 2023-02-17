using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public abstract class BaseBuilder : Spatial, IBuilder<BaseBuilding>
{
    public BaseBuilding CurrentBuilding { get; set; }

    protected abstract void _FollowBuild(float delta);

    public override void _Process(float delta)
    {
        if (CurrentBuilding == null) return;
        _FollowBuild(delta);
    }

    public virtual void Place(BaseBuilding building)
    {
        if (!building.IsBuildingNow || building.IsCollided) return;
        if (!building.CanPlace) return;

        building.IsBuildingNow = false;
        building.DisableGhostMode();
        building.OnSuccessBuild();
    }

    public void PlaceGhost(BaseBuilding building)
    {
        if (building.IsBuildingNow) return;

        building.IsBuildingNow = true;
        building.EnableGhostMode();
    }
}