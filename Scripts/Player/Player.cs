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

	private Label _displayHealth;
	private Label _displayEnergy;
	private Label _displayCredits;
	private RichTextLabel _displayError;

	private List<IBlueNpcAgent> _buildings = new List<IBlueNpcAgent>();
	public IEnumerable<IBlueNpcAgent> Buildings => _buildings;

	public override void _Ready()
	{
		_displayHealth = GetNode<Label>("UI/PanelContainer/GridContainer/HealthLabel");
		_displayEnergy = GetNode<Label>("UI/PanelContainer/GridContainer/EnergyLabel");
		_displayCredits = GetNode<Label>("UI/PanelContainer/GridContainer/CreditsLabel");
		_displayError = GetNode<RichTextLabel>("UI/ErrorText");
	}

	public override void _Process(float delta)
	{
		_displayHealth.Text = Health.ToString();
		_displayCredits.Text = Credits.ToString();
		_displayEnergy.Text = Energy.ToString();
		if(_tower == null)
		{
			_displayError.Text = "Установите главную башню!";
		}
		else
		{
			_displayError.Text = "";
		}
	}

	public bool IsHasTower() => _tower != null;

	public void SetMainTower(TowerBuilding tower)
	{
		_tower = tower;
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
		if (blueAgent is TowerBuilding) AiBuilder.Instance.IsActive = false;
	}

	public Vector3 GetTowerPosition() => _tower.Translation;

	public void GetDamage(int damage)
	{
		Health -= damage;
	}

	public void Heal(int heals)
	{
		Health += heals;
	}

	public override Player CreateInstance() => this;
}
