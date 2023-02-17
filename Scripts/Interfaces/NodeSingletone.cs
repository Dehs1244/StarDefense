using Godot;
using System;

public abstract class NodeSingletone<T> : Node, ISingletone<T>
    where T : Node
{
    private static NodeSingletone<T> _instance;
    public static T Instance { get; set; }
    public bool Verbose;

    protected virtual void OnAwake()
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
        OnAwake();
    }

}
