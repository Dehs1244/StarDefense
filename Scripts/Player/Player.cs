using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class Player : SpatialSingletone<Player>, IPlayer
{
	private TowerBuilding _tower;
	public int Health { get => _tower != null ? _tower.Health : 1; set {
			if (_tower == null) return;
			_tower.Health = value;
		} }

	public int Energy { get; set; }
	public int Credits { get; set; }
	public bool IsEndGame => Health <= 0;

	[Export]
	public int NotificationTimeout { get; set; }
	private float _timer;

	private Label _displayHealth;
	private Label _displayEnergy;
	private Label _displayCredits;
	private RichTextLabel _displayError;
	private VBoxContainer _notificationPool;
	[Export]
	public NodePath EndGamePath;
	private PanelContainer _EndGamePanel;

	[Export]
	private PackedScene _specimenNotification;

	private List<IBlueNpcAgent> _buildings = new List<IBlueNpcAgent>();
	public IEnumerable<IBlueNpcAgent> Buildings => _buildings;

	public override void _Ready()
	{
		_displayHealth = GetNode<Label>("UI/PanelContainer/GridContainer/HealthLabel");
		_displayEnergy = GetNode<Label>("UI/PanelContainer/GridContainer/EnergyLabel");
		_displayCredits = GetNode<Label>("UI/PanelContainer/GridContainer/CreditsLabel");
		_displayError = GetNode<RichTextLabel>("UI/ErrorText");
		_EndGamePanel = GetNode<PanelContainer>(EndGamePath);
	}

	private void _GetCredits()
	{
		Credits += 50;
	}

	protected override void _OnAwake()
	{
		_notificationPool = GetNode<VBoxContainer>("UI/NotificationPool/VBoxContainer");
	}

	public override void _Process(float delta)
	{
		_displayHealth.Text = Health.ToString();
		_displayCredits.Text = Credits.ToString();
		_displayEnergy.Text = Energy.ToString();
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		if (_tower == null)
		{
			_displayError.Text = "Установите главную башню!";
		}
		else
		{
			_displayError.Text = "";
		}

		if (_notificationPool.GetChildCount() > 0)
		{
			_timer -= 1 * delta;
			if (_timer < 1)
			{
				_notificationPool.GetChild(0).QueueFree();
				_timer = NotificationTimeout;
			}
		}
		else _timer = NotificationTimeout;
	}

	public bool IsHasTower() => _tower != null;

	public void SetMainTower(TowerBuilding tower)
	{
		_tower = tower;
		_GetCredits();
		AiBuilder.Instance.IsActive = true;
	}

	public void AddBuilding(IBlueNpcAgent blueAgent)
	{
		_buildings.Add(blueAgent);
		NpcController.Instance?.OnCreateBuilding(blueAgent);
	
	}

	public void RemoveBuilding(IBlueNpcAgent blueAgent)
	{
		_buildings.RemoveAll(x=> x == blueAgent);
		if (blueAgent is TowerBuilding)
		{
			Player.Instance.GetDamage(10);
			AiBuilder.Instance.IsActive = false;
		}
	}

	public Vector3 GetTowerPosition() => _tower.Translation;

	public void GetDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			_EndGamePanel.Show();
		}
		else _EndGamePanel.Hide();
	}

	public void AddNotification(string notification)
	{
		Label copyCreated = _specimenNotification.Instance<Label>();
		copyCreated.Text = notification;
		_notificationPool.AddChild(copyCreated);
	}

	public void Heal(int heals)
	{
		Health += heals;
	}

	public override Player CreateInstance() => this;
}
