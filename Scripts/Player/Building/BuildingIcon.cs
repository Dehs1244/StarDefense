using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using StarDefense.Attrubutes;

public class BuildingIcon : Control
{
	[Export]
	public string IconName { get; set; }
	private TextureRect _texture { get; set; }
	private Label _iconDisplayName { get; set; }
	private Label _costDisplay { get; set; }
	private string _buildingResource { get; set; }
	private BuildingResourceDescriptor _descriptor = new BuildingResourceDescriptor();

	[Export]
	public string TexturePath;

	public override void _EnterTree()
	{
		Init();
	}

	public void Init()
	{
		_iconDisplayName = GetNode<Label>("ColorRect/VBoxContainer/Name");
		_texture = GetNode<TextureRect>("ColorRect/VBoxContainer/Icon");
		_costDisplay = GetNode<Label>("ColorRect/VBoxContainer/Cost");
	}

	public override void _Ready()
	{
		if (!string.IsNullOrEmpty(IconName)) 
			SetText(IconName);
		if (!string.IsNullOrEmpty(TexturePath))
			SetIcon(TexturePath);
	}

	public void SetBuildingResource(BuildingAttribute data)
	{
		SetIcon(_descriptor.CreateIcon(data.Name, data.Extension));
		_buildingResource = data.Name;
		SetText(data.BuildingName);
		SetCost(data.Cost.ToString());
	}

	public BaseBuilding CreateBuilding()
		=> _descriptor.Create(_buildingResource);

	public void SetIcon(Texture icon)
	{
		_texture.Texture = icon;
	}

	public string GetResourceName() => _buildingResource;

	public void SetText(string text)
		=> _iconDisplayName.Text = text;

	public void SetCost(string cost)
		=> _costDisplay.Text = $"Cost: {cost}";

	public void SetIcon(string path) =>
		SetIcon(GD.Load<Texture>(path));
}
