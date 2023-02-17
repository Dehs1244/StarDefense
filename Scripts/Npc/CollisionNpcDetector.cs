using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class CollisionNpcDetector : Area
{
    public Node CollisionOwner { get; set; }

    public override void _EnterTree()
    {
        var parent = GetParent();
        while(parent != null && !(parent is INpcAgent))
        {
            parent = GetParent();
        }

        if (parent == null) throw new Exception("CollisionNpcDetector has not INpcAgent owner, please use parent as INpcAgent owner");
        CollisionOwner = parent;
    }

    public Node GetCollisionOwner() =>
        CollisionOwner;
        
} 