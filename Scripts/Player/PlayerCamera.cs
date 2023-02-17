using Godot;
using System;

public class PlayerCamera : Camera
{
    private bool _isActive { get; set; }
    private Player _parent;

    public override void _EnterTree()
    {
        //_translator = GetParent<Area>();
        _parent = GetParent<Player>();
    }

    public override void _Ready()
    {

        InputController.Instance.OnMouseButton += (context) =>
        {
            if (context.IsOnUI)
            {
                _isActive = false;
                return;
            }

            if (context.Button == ButtonList.Left) _isActive = context.IsPressed;
            var scrollFactor = context.Factor;

            if(context.Button == ButtonList.WheelUp)
            {
                _parent.Translate(new Vector3(0, scrollFactor * -1, scrollFactor * -1));
            }
            if(context.Button == ButtonList.WheelDown)
            {
                _parent.Translate(new Vector3(0, scrollFactor, scrollFactor));
            }
        };

        InputController.Instance.OnMouseMotion += (relative) =>
        {
            if (_isActive)
            {
                relative = relative.Normalized() * 2f;
                //var relative3 = new Vector3(relative.x * -1, relative.y, 0);
                var relative3 = new Vector3(relative.x * -1, 0, relative.y * -1);
                _parent.Translate(_parent.Transform.basis.Xform(relative3));
            }
        };
    }

    public override void _Process(float delta)
    {
        //var transform = Transform;
        //if (_cameraColider.IsColliding())
        //    transform.origin = _cameraColider.GetCollisionPoint();
        //Transform = transform;
    }
}
