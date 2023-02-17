using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public sealed class PlayerBuilder : BaseBuilder
{
    private Camera _playerCamera;

    public override void _Ready()
    {
        _playerCamera = GetViewport().GetCamera();
    }

    protected override void _FollowBuild(float delta)
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        if (CurrentBuilding == null) return;

        var mousePosition = GetViewport().GetMousePosition();
        var cameraOrigin = _playerCamera.ProjectRayOrigin(mousePosition);
        var position = cameraOrigin + _playerCamera.ProjectRayNormal(mousePosition) * 1000;
        var space = GetWorld().DirectSpaceState;
        var rayBuild = space.IntersectRay(cameraOrigin, position);;
        if (!rayBuild.Contains("position"))
        {
            _SetBuildingFollow(_playerCamera.ProjectPosition(mousePosition, 25));
            CurrentBuilding.CanPlace = false;
            CurrentBuilding.EnableErrorMode();
            return;
        }
        var rayPosition = (Vector3)rayBuild["position"];
        if (!CurrentBuilding.IsCollided)
        {
            CurrentBuilding.CanPlace = true;
            CurrentBuilding.DisableErrorMode();
        }
        _SetBuildingFollow(rayPosition);
    }

    private void _SetBuildingFollow(Vector3 position)
    {
        var transform = CurrentBuilding.GlobalTransform;
        transform.origin = position;
        CurrentBuilding.GlobalTransform = transform;
    }
}
