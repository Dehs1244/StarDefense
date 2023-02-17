using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public struct ShootContext
{
    public Spatial Reciever { get; set; }
    public CollisionNpcDetector NpcDetector => Reciever is CollisionNpcDetector ? Reciever as CollisionNpcDetector : null;
    public IBuilding Building => Reciever is IBuilding ? Reciever as IBuilding : null;
    public INpcAgent Agent => Reciever is INpcAgent ? Reciever as INpcAgent : null;
}
