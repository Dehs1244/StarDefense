using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class BulletController : KinematicBody
{
	private IShootable _sender;
	public IShootable Sender => _sender;
	private Vector3 _direction;
	private float _timer;

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(float delta)
	{
		_timer += 1 * delta;
		//_direction.y = _sender.GetPosition().y;
		//_direction.y += 1;
		var toDirection = (_direction - Transform.origin).Normalized();
		var reciever = MoveAndCollide(toDirection, false, false);
		if(_sender != null && reciever != null)
		{
			Collided(reciever.Collider);
		}
		if (_timer >= 30) QueueFree();
	}

	public void Collided(Godot.Object body)
	{
		if (!(body is Spatial)) return;
		if (body == _sender) return;
		if (body is CollisionNpcDetector detector && detector.GetCollisionOwner() == body) return;

		ShootContext context = new ShootContext();
		context.Reciever = body as Spatial;
		_sender.OnShoot(context);
		QueueFree();
	}

	public void SetSender(IShootable sender)
	{
		if (_sender != null) return;
		_sender = sender;
	}

	public void SetBulletDirection(Vector3 direction)
	{
		_direction = direction;
	}
}
