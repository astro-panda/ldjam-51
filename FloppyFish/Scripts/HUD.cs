using Godot;
using System;

public class HUD : CanvasLayer
{

    private AnimationPlayer _animation;
    private Label _waterAirValueLabel;
    private Label _collectCountLabel;

    private Sprite _holdingCollectable;

    private Button _startButton;
    private bool _fishCurrentlySuffocating = false;

    [Export]
    public uint CollectGoal {get;set;} = 8;

    private uint CollectedCount = 0;

    [Signal]
    public delegate void AllCollected();

    [Signal]
    public delegate void StartGame();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animation = GetNode<AnimationPlayer>("Animation");
        _waterAirValueLabel = GetNode<Label>("Water Air").GetNode<Label>("Water Air Value");
        _collectCountLabel = GetNode<Label>("Collect Counter").GetNode<Label>("Collect Counter Value");
        _holdingCollectable = GetNode<Sprite>("HasCollectible");
        _startButton = GetNode<Button>("StartButton");
        _holdingCollectable.Hide();
    }

    public void OnSetWaterAirValue(int value)
    {
        SetWaterAir(value);
    }

    public void OnFishSuffocatingStart()
    {
        _fishCurrentlySuffocating = true;
        _animation.PlaybackSpeed = 1;
        _animation.Play("Blackout");
    }

    public void OnFishSuffocatingStop()
    {
        if (_fishCurrentlySuffocating)
        {
            _animation.PlaybackSpeed = 5;
            _animation.PlayBackwards("Blackout");
            _animation.Seek(_animation.CurrentAnimationPosition);
            _fishCurrentlySuffocating = false;
        }

        SetWaterAir(10);
    }

    private void SetWaterAir(int value)
    {
        int finalValue = Mathf.Clamp(value, 0, 10);
        _waterAirValueLabel.Text = $"{finalValue}";
    }

    private void OnCollectableCollected()
    {
        _holdingCollectable.Hide();

        CollectedCount += 1;

        _collectCountLabel.Text = $"{Mathf.Clamp(CollectedCount, 0, CollectGoal)}";

        if(CollectedCount >= CollectGoal)
            FishWin();        
    }

    public void FishDied()
    {
        _animation.Play("Died");
    }

    public void FishWin()
    {
        EmitSignal(nameof(AllCollected));
        _animation.Play("Win");
    }

    public void HoldingCollectible()
    {
        _holdingCollectable.Show();
    }

    public void OnStartButtonPressed()
    {
        _startButton.Hide();
        EmitSignal("StartGame");
    }
}
