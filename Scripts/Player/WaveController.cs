using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class WaveController : NodeSingletone<WaveController>
{
    private int _totalEnemies;
    public int TotalEnemies
    {
        get => _totalEnemies; set
        {
            _totalEnemies = value;
            Update();
        }
    }

    private int _destroyed;
    public int Destroyed { get => _destroyed; set
        {
            _destroyed = value;
            Update();
        }
    }

    public int Wave { get; private set; }

    private Label _destroyedDisplay;
    private Label _enemiesDisplay;
    private Label _waveDisplay;

    private string _savedDestroyedText;
    private string _savedEnemiesText;
    private string _savedWaveText;

    public event Action OnNewWave;

    public override void _Ready()
    {
        base._Ready();
        GD.Print(GetPath());
        var parent = GetParent();
        _destroyedDisplay = parent.GetNode<Label>("UI/Waves/VBoxContainer/DestroyedLabel");
        _enemiesDisplay = parent.GetNode<Label>("UI/Waves/VBoxContainer/TotalEnemiesLabel");
        _waveDisplay = parent.GetNode<Label>("UI/Waves/VBoxContainer/WavesLabel");

        _savedDestroyedText = _destroyedDisplay.Text;
        _savedEnemiesText = _enemiesDisplay.Text;
        _savedWaveText = _enemiesDisplay.Text;
    }

    public void Update()
    {
        if (TotalEnemies < 1)
        {
            Wave += 1;
            OnNewWave?.Invoke();
        }
        _destroyedDisplay.Text = $"{_savedDestroyedText} {Destroyed}";
        _enemiesDisplay.Text = $"{_savedEnemiesText} {TotalEnemies}";
        _waveDisplay.Text = $"{_savedWaveText} {Wave}";
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public override WaveController CreateInstance() => this;
}
