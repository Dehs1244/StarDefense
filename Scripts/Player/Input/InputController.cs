using Godot;
using System;

public class InputController : NodeSingletone<InputController>
{
	public event Action<MouseClickContext> OnMouseButton;
	public event Action<Vector2> OnMouseMotion;
	private Node _hoveredUI;
	private bool _isMotion;

	public override InputController CreateInstance()
		=> this;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_isMotion = true;
			OnMouseMotion?.Invoke(mouseMotion.Relative);
		}
		if (@event is InputEventMouseButton mouseButton)
		{
			MouseClickContext context = new MouseClickContext();
			context.Button = (ButtonList)mouseButton.ButtonIndex;
			context.IsPressed = @event.IsPressed();
			context.Factor = mouseButton.Factor;
			context.HoveredUI = _hoveredUI;
			context.IsMouseMotion = _isMotion;

			OnMouseButton?.Invoke(context);
		}
	}

    public override void _Ready()
    {
		//var uiContainer = GetParent().GetNode<Node>("UI");
		//foreach (Node node in uiContainer.GetChildren())
		//{
		//	ConnectAsUI(node);
		//}
		foreach(Node node in GetTree().GetNodesInGroup("UI"))
        {
			ConnectAsUI(node);
		}
	}

	public void ConnectAsUI(Node ui, Node asParameter = null)
    {
		if (asParameter == null) asParameter = ui;
		ui.Connect("mouse_entered", this, "_OnUIEntered", new Godot.Collections.Array(asParameter));
		ui.Connect("mouse_exited", this, "_OnUIExited");
	}

    protected override void OnAwake()
	{
	}

	private void _OnUIEntered(Node ui)
	{
		_hoveredUI = ui;
	}
	
	private void _OnUIExited()
	{
		_hoveredUI = null;
	}
}
