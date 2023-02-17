using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public abstract class SpatialSingletone<T> : Spatial, ISingletone<T>
{
    public static T Instance { get; set; }
    public bool Verbose;

    protected virtual void _OnAwake()
    {

    }

    public abstract T CreateInstance();

    public sealed override void _EnterTree()
    {
        if (Instance == null)
        {
            Instance = CreateInstance();
            if (Verbose) GD.Print("Instance was created");
        }
        _OnAwake();
    }
}
