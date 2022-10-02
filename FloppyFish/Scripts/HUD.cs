using Godot;
using System;

public class HUD : CanvasLayer
{

    private AnimationPlayer _blackoutAnimation;
    private Label _waterAirValueLabel;
    private Label _collectCountLabel;
    private bool _fishCurrentlySuffocating = false;

    [Export]
    public uint CollectGoal = 20;

    private uint CollectedCount = 0;

    [Signal]
    public delegate void AllCollected();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _blackoutAnimation = GetNode<AnimationPlayer>("Blackout Animation");
        _waterAirValueLabel = GetNode<Label>("Water Air").GetNode<Label>("Water Air Value");
        _collectCountLabel = GetNode<Label>("Collect Counter").GetNode<Label>("Collect Counter Value");
    }

    public void OnSetWaterAirValue(int value)
    {
        SetWaterAir(value);
    }

    public void OnFishSuffocatingStart()
    {
        _fishCurrentlySuffocating = true;
        _blackoutAnimation.Play("Blackout");
    }

    public void OnFishSuffocatingStop()
    {
        if (_fishCurrentlySuffocating)
        {
            _blackoutAnimation.PlayBackwards("Blackout");
            _blackoutAnimation.Seek(_blackoutAnimation.CurrentAnimationPosition);
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
        CollectedCount += 1;

        _collectCountLabel.Text = $"{Mathf.Clamp(CollectedCount, 0, CollectGoal)}";

        if(CollectedCount >= CollectGoal)
            EmitSignal(nameof(AllCollected));
    }

}
