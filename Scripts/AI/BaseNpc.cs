using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseNpc : KinematicBody, INpcAgent
{
    private Area _interactionArea;
    protected List<INpcAgent> _detectedNpc = new List<INpcAgent>();
    public virtual int Health { get; set; } = 100;
    public virtual float Speed => 10f;

    public bool IsActivatedAgent { get; set; }

    public override void _Ready()
    {
        _interactionArea = GetNode<Area>("DetectNpcArea");
        IsActivatedAgent = true;
        OnActivateAi();

        ConnectAsAi(_interactionArea, this);
    }

    public virtual void OnActivateAi()
    {
        NpcController.Instance?.Subscribe(this);
    }

    public static void ConnectAsAi(Area interactionArea, Node sender)
    {
        interactionArea.Connect("body_entered", sender, "_OnAiEnterBody");
        interactionArea.Connect("body_exited", sender, "_OnAiLeaveBody");
        interactionArea.Connect("area_entered", sender, "_OnAiEnterArea");
        interactionArea.Connect("area_exited", sender, "_OnAiLeaveArea");
    }

    private void _OnAiEnterBody(Node body)
    {
        if (body is CollisionNpcDetector detector) body = detector.GetCollisionOwner();
        if (body is INpcAgent agent)
        {
            if (!agent.IsActivatedAgent) return;
            _detectedNpc.Add(agent);
            InteractionWith(agent);
        }
    }

    private void _OnAiEnterArea(Area area) => _OnAiEnterBody(area);

    private void _OnAiLeaveBody(Node body)
    {
        if (_detectedNpc != null) return;
        if (_detectedNpc.Any(x=> x == body)) EndInteraction(body as INpcAgent);
    }

    private void _OnAiLeaveArea(Area area) => _OnAiLeaveBody(area);

    public abstract void InteractionWith(INpcAgent agent);

    public virtual void Move(Vector3 direction)
    {
        //var a = new Quat(Transform.basis);
        //var 
        //var dir = direction;
        //dir.y = Transform.origin.y;
        //var b = new Quat(Transform.LookingAt(dir, Vector3.Up).basis);
        //var c = a.Slerp(b, 0.1f);
        //var transform = Transform;
        //transform.basis = new Basis(c);
        //Transform = transform;

        //direction.y = GlobalTransform.origin.y;
        
        //I don't know, but this work :p
        try
        {
            var b = GlobalTransform.LookingAt(direction, Vector3.Up);
            GlobalTransform = GlobalTransform.InterpolateWith(b, 0.1f);
        }catch(Exception)
        {
            LookAt(direction, Vector3.Up);
        }

        var dir = (direction - GlobalTransform.origin).Normalized() * Speed;
        MoveAndSlide(dir);
    }

    public void EndInteraction(INpcAgent agent)
    {
        _detectedNpc = null;
    }

    public void LookAtAgent(INpcAgent agent) => LookAtPosition(agent.GetPosition());

    public virtual Vector3 GetPosition() => GlobalTransform.origin;

    public virtual void LookAtPosition(Vector3 position) =>
        LookAt(position, Vector3.Up);

    public virtual void GetDamage(int damage)
    {
        Health -= damage;
        if (Dead()) return;
    }

    public virtual bool Dead()
    {
        if (Health <= 0)
        {
            QueueFree();
            NpcController.Instance?.UnSubscribe(this);
            return true;
        }

        return false;
    }
}
