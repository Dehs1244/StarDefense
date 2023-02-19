using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarDefense.Helpers;
using StarDefense.Attrubutes;
using Godot;

public sealed class BuildingsController : NodeSingletone<BuildingsController>
{
	public override BuildingsController CreateInstance()
		=> this;
	private IEnumerable<BuildingAttribute> _allBuildingsData { get; set; }
	private HBoxContainer _boxContainer { get; set; }
	private PlayerBuilder _builder { get; set; }

	[Export]
	public PackedScene IconPrefab;

	public void InitializeAllBuildings() =>
		_allBuildingsData = TypeHelper.WithResourceAttributeDefinedAttributes<BuildingAttribute>();

	private BuildingIcon _CreateIcon()
		=> IconPrefab.Instance<BuildingIcon>();

	public void RemoveIcon(string resourceName)
	{
		var icon = _boxContainer.GetNode($"{resourceName}_icon");
		icon.QueueFree();
	}

	protected override void OnAwake()
	{
		InitializeAllBuildings();
		_builder = GetNode<PlayerBuilder>("/root/Spatial/Player/BuilderMaster");

		_boxContainer = GetNode<HBoxContainer>("ScrollContainer/HBoxContainer");
		foreach(var building in _allBuildingsData)
		{
			var buildingIcon = _CreateIcon();
			buildingIcon.Name = $"{building.Name}_icon";
			buildingIcon._EnterTree();
			buildingIcon.SetBuildingResource(building);
			var interactNode = buildingIcon.GetNode("ColorRect/VBoxContainer");
			InputController.Instance.ConnectAsUI(interactNode, buildingIcon);
			_boxContainer.AddChild(buildingIcon);
		}

		InputController.Instance.OnMouseButton += (context) =>
		{
			if (!context.IsPressed) return;
			if (context.IsOnUI && _builder.CurrentBuilding != null)
			{
				_builder.CurrentBuilding.QueueFree();
				_builder.CurrentBuilding = null;
			}

			if(!context.IsOnUI && 
			context.IsMouseMotion &&
			context.Button == ButtonList.Left &&
			_builder.CurrentBuilding != null)
			{
				if (!_builder.CurrentBuilding.CanPlace || _builder.CurrentBuilding.IsCollided) return;
				_builder.Place(_builder.CurrentBuilding);
				Player.Instance.Credits -= (int)_builder.CurrentBuilding.Cost;
				_builder.CurrentBuilding = null;
			}

			if (context.HoveredUI is BuildingIcon icon && context.Button == ButtonList.Left)
			{
				if(Player.Instance.Credits < icon.Cost)
                {
					Player.Instance.AddNotification($"Не хватает кредитов для приобритения {icon.IconName}");
					return;
                }
				if (!Player.Instance.IsHasTower() && icon.GetResourceName() != "Tower") return;

				var building = icon.CreateBuilding();
				_builder.PlaceGhost(building);
				NpcController.Instance.AddChild(building);
				_builder.CurrentBuilding = building;
			}
		};
	}
}
