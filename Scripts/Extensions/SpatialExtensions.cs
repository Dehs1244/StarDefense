using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public static class SpatialExtensions
{
    public static void LookAtX(this Spatial node, Spatial target)
    {
        Transform selfState = node.Transform;
        var selfQuaturnation = new Quat(selfState.basis);
        var targetPosition = target.Transform.origin;
        //targetPosition.x = node.Transform.origin.y;

        var toQuat = new Quat(selfState.LookingAt(targetPosition, Vector3.Up).basis);
        var quatResultLerp = selfQuaturnation.Slerp(toQuat, 0.05f);
        selfState.basis = new Basis(quatResultLerp);
        node.Transform = selfState;
    }
}
