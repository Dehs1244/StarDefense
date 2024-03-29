﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class AiBuilder : SpatialSingletone<AiBuilder>
{
    [Export]
    public IEnumerable<PackedScene> Placing;
    private RandomNumberGenerator _random;
    [Export]
    private float _timeout;
    private float _timer;

    public int SpawnedEnemies { get; private set; }

    [Export]
    public int MaxEnemies { get; set; }
    [Export]
    public int MaxSpawningEnemies { get; set; }
    private int _spawnedForNow;

    public bool IsActive { get; set; }

    private Area _spawnArea;
    private BoxShape _shape;

    public override void _Ready()
    {
        base._Ready();
        _spawnArea = GetNode<Area>("Spawn");
        _random = new RandomNumberGenerator();
        _random.Randomize();
        _shape = (BoxShape)_spawnArea.ShapeOwnerGetShape(0, 0);
        IsActive = false;
        WaveController.Instance.OnNewWave += () =>
        {
            SpawnedEnemies = 0;
            MaxEnemies += 3;
            MaxSpawningEnemies++;
        };
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (!Player.Instance.IsHasTower() || IsAllSpawned()) return;
        _timer += 1 * delta;
        if (_timer < _timeout) return;
        _timer = 0;
        float fromZ = _shape.Extents.z;
        float fromX = _shape.Extents.x;

        float toZ;
        float toX;

        toX = _random.RandfRange(GlobalTransform.origin.x - fromX, GlobalTransform.origin.x + fromX);
        toZ = _random.RandfRange(GlobalTransform.origin.z - fromZ, GlobalTransform.origin.z + fromZ);

        var placeIndx = Placing.Count() > 1 ? _random.RandiRange(0, Placing.Count()) : 0;

        var prefab = Placing.ElementAt(placeIndx).Instance<BaseNpc>();
        NpcController.Instance.AddChild(prefab);
        var transform = prefab.GlobalTransform;
        transform.origin = new Vector3(toX, transform.origin.y + 4, toZ);
        prefab.GlobalTransform = transform;
        SpawnedEnemies++;
        _spawnedForNow++;
        if (_spawnedForNow < MaxSpawningEnemies) _timer = _timeout;
        else _spawnedForNow = 0;
        //NpcController.Instance.Subscribe(prefab);
    }

    public bool IsAllSpawned() => SpawnedEnemies >= MaxEnemies;

    public override AiBuilder CreateInstance()
        => this;
}
