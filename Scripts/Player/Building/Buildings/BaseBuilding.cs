using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public abstract class BaseBuilding : Spatial, IBuilding, IBlueNpcAgent
{
    public int Health { get; set; } = 100;

    public abstract uint MaxHealth { get; }

    public abstract int Damage { get; }

    public abstract uint Cost { get; }
    public bool CanPlace { get; set; }
    public bool IsBuildingNow { get; set; }
    public bool IsCollided { get; set; }

    public Area DetectCollision;
    public Area DetectNpcCollision;

    protected SpatialMaterial _ghostMaterial { get; set; }
    protected SpatialMaterial _errorMaterial { get; set; }

    private List<Spatial> _enemies = new List<Spatial>();

    private IList<MeshInstance> _childMeshes { get; set; }
    public bool IsActivatedAgent { get; set; }

    protected abstract IEnumerable<MeshInstance> _GetMesh();

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (_enemies.Any())
        {
            InteractionWithEnemy(_enemies.First());
        }
    }

    public virtual void DisableGhostMode()
    {
        foreach(var mesh in _childMeshes)
        {
            RestoreMaterials(mesh);
        }
    }

    public virtual void EnableGhostMode()
    {
        foreach(var mesh in _childMeshes)
        {
            SetMaterial(mesh, _ghostMaterial);
        }
    }

    public virtual void EnableErrorMode()
    {
        foreach(var mesh in _childMeshes)
        {
            SetMaterial(mesh, _errorMaterial);
        }
    }

    public virtual void Move(Vector3 direction)
    {
    }

    public void InteractionWith(INpcAgent agent)
    {
        if (agent is IBlueNpcAgent) return;
        if (agent is Spatial gameObj)
        {
            if (gameObj is IRedNpcAgent)
            {
                _enemies.Add(gameObj);
            }
        }
    }

    public virtual void InteractionWithEnemy(Spatial agent)
    {
    }

    public void EndInteraction(INpcAgent agent)
    {
        _enemies.RemoveAll(x=> x == agent);
    }

    public virtual void DisableErrorMode()
        => EnableGhostMode();

    private Dictionary<string, List<Material>> _savedMaterials = new Dictionary<string, List<Material>>();

    protected void SaveMaterial(MeshInstance mesh)
        => SaveMaterial(mesh.Name, _GetAllMaterials(mesh));

    protected void SaveMaterial(string name, Material material)
        => SaveMaterial(name, new List<Material>() { material });

    protected void SaveMaterial(string name, List<Material> materials)
    {
        if (_savedMaterials.ContainsKey(name)) return;
        _savedMaterials.Add(name, materials);
    }

    protected void SetMaterial(MeshInstance mesh, Material material)
    {
        _EnumerateMaterials(mesh, (x, y, i) => mesh.SetSurfaceMaterial(i, material));
    }

    public virtual void Init()
    {
        _ghostMaterial = ResourceLoader.Load<SpatialMaterial>("res://Materials/Buildings/GhostBuilding.tres");
        _errorMaterial = ResourceLoader.Load<SpatialMaterial>("res://Materials/Buildings/ErrorBuilding.tres");
        _childMeshes = _GetMesh().ToList();

        foreach(var mesh in _childMeshes)
        {
            SaveMaterial(mesh);
        }

        DetectCollision = GetNode<Area>("DetectArea");
        DetectCollision.Connect("body_entered", this, "_OnEnterBody");
        DetectCollision.Connect("body_exited", this, "_OnLeaveBody");
        DetectCollision.Connect("area_entered", this, "_OnEnterArea");
        DetectCollision.Connect("area_exited", this, "_OnLeaveArea");

        foreach(Area detected in DetectCollision.GetOverlappingAreas())
        {
            _OnEnterArea(detected);
        }

        foreach(Node detected in DetectCollision.GetOverlappingBodies())
        {
            _OnEnterBody(detected);
        }

        DetectNpcCollision = GetNode<Area>("DetectNpcArea");
        BaseNpc.ConnectAsAi(DetectNpcCollision, this);
    }

    protected void RestoreMaterials(MeshInstance mesh)
    {
        if (!_savedMaterials.ContainsKey(mesh.Name)) return;
        _EnumerateMaterials(mesh, (x, y, i) => y.SetSurfaceMaterial(i, _savedMaterials[mesh.Name][i]));
    }

    protected void _EnumerateMaterials(MeshInstance mesh, Action<Material, MeshInstance, int> on)
    {
        for (var i = 0; i < mesh.GetSurfaceMaterialCount(); i++)
        {
            on(mesh.GetSurfaceMaterial(i), mesh, i);
        }
    }

    protected List<Material> _GetAllMaterials(MeshInstance mesh)
    {
        var materials = new List<Material>();
        _EnumerateMaterials(mesh, (x, y, i) => materials.Add(x));
        return materials;
    }

    public void _OnEnterBody(Node body)
    {
        if (body.Name == "DetectNpcArea") return;
        if (IsBuildingNow)
        {
            IsCollided = true;
            EnableErrorMode();
        }

        if (!IsBuildingNow)
        {
            if (body is BulletController bullet && bullet.Sender != this)
            {
                bullet.Collided(this);
            }

            if (body is INpcAgent agent && agent.IsActivatedAgent) InteractionWith(agent);
        }
    }

    private void _OnEnterArea(Area area) => _OnEnterBody(area);
    private void _OnLeaveArea(Area area) => _OnLeaveBody(area);

    public void _OnLeaveBody(Node body)
    {
        //I don't know how to fix it
        if (IsBuildingNow)
        {
            IsCollided = false;
            DisableErrorMode();
        }
    }

    public void _OnAiEnterBody(Node body)
    {
        if (!IsBuildingNow)
        {
            if (body is INpcAgent agent) InteractionWith(agent);
        }
    }

    private void _OnAiEnterArea(Area area) => _OnAiEnterBody(area);
    private void _OnAiLeaveArea(Area area) => _OnAiLeaveBody(area);

    public void _OnAiLeaveBody(Node body)
    {
        if (!IsBuildingNow && body is INpcAgent agent) EndInteraction(agent);
    }

    public virtual void GetDamage(int damage)
    {
        Health -= damage;
        if (Dead()) return;
    }

    public virtual void Heal(int heal)
    {
        Health += heal;
    }

    public virtual void OnSuccessBuild()
    {
        IsActivatedAgent = true;
        Player.Instance.AddBuilding(this);
        NpcController.Instance?.Subscribe(this);
        foreach(Node body in DetectNpcCollision.GetOverlappingBodies())
        {
            _OnAiEnterBody(body);
        }
    }

    public void LookAtAgent(INpcAgent agent)
        => LookAtPosition(agent.GetPosition());

    public virtual void LookAtPosition(Vector3 position)
        => LookAt(position, Vector3.Up);

    public virtual Vector3 GetPosition() => Transform.origin;

    public bool Dead()
    {
        if (Health <= 0)
        {
            QueueFree();
            NpcController.Instance?.UnSubscribe(this);
            Player.Instance.RemoveBuilding(this);
            return true;
        }

        return false;
    }
}