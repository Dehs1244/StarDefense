using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public abstract class BaseRedNpc : BaseNpc, IRedNpcAgent
{
    public IBlueNpcAgent _target;
    private bool _isMoving;

    protected NavigationAgent _agent;

    public override void _Ready()
    {
        base._Ready();
        _agent = GetNode<NavigationAgent>("NpcAgent");
        _agent.Connect("navigation_finished", this, "_OnEndMoving");

        var target = _FindTarget();
        if (target == null) return;
        OnDetectEnemy(target);
    }

    public override void OnActivateAi()
    {
        base.OnActivateAi();
        WaveController.Instance.TotalEnemies += 1;
    }

    protected IBlueNpcAgent _FindTarget() => Player.Instance.Buildings.FirstOrDefault();

    public override void InteractionWith(INpcAgent agent)
    {
        if (agent is IRedNpcAgent red) InteractWithFriendly(red);
        if (agent is IBlueNpcAgent blue) InteractWithEnemy(blue);
    }

    protected virtual bool _CanChangeTarget()
    {
        return true;
    }

    public virtual void OnDetectEnemy(IBlueNpcAgent agent)
    {
        if (agent == null) return;
        if(_target == null || !_isMoving)
        {
            if (!_CanChangeTarget()) return;
            OnDetectCloseEnemy(agent);
            return;
        }
        try
        {
            var closetTarget = _target == null ? 0f : Transform.origin.DistanceTo(_target.GetPosition());
            if (Transform.origin.DistanceTo(agent.GetPosition()) < closetTarget)
            {
                OnDetectCloseEnemy(agent);
            }
        }catch(ObjectDisposedException)
        {
            OnDetectCloseEnemy(agent);
        }
    }

    public virtual void OnDetectCloseEnemy(IBlueNpcAgent closeAgent)
    {
        if (closeAgent == null) return;
        _target = closeAgent;
        _agent.SetTargetLocation(closeAgent.GetPosition());
        _isMoving = true;
    }

    protected void StopMove()
    {
        _isMoving = false;
        _agent.SetTargetLocation(Vector3.Zero);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        //Move(Player.Instance.GetTowerPosition());
        if (_target == null || !_isMoving)
        {
            OnDetectEnemy(_FindTarget());
            return;
        }
        var location = _agent.GetNextLocation();

        Move(location);
        
        //StopMove();
    }

    private void _OnEndMoving()
    { 
        StopMove();
    }

    public override bool Dead()
    {
        var isDead = base.Dead();
        if (isDead)
        {
            WaveController.Instance.TotalEnemies -= 1;
            WaveController.Instance.Destroyed += 1;
        }

        return isDead;
    }

    public abstract void InteractWithFriendly(IRedNpcAgent npc);
    public abstract void InteractWithEnemy(IBlueNpcAgent npc);

    public void InvokeOnCreateEnemy(IBlueNpcAgent blueAgent)
    {
        OnDetectEnemy(blueAgent);
    }
}