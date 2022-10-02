using Godot;
using System;

public class HUD : CanvasLayer
{
    
    private AnimationPlayer _blackoutAnimation;
    private bool _fishCurrentlySuffocating = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _blackoutAnimation = GetNode<AnimationPlayer>("Blackout Animation");       
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void OnFishSuffocatingStart()
    {
        _fishCurrentlySuffocating = true;
        _blackoutAnimation.Play("Blackout");
    }

    public void OnFishSuffocatingStop()
    {
        if(_fishCurrentlySuffocating)
        {
            _blackoutAnimation.PlayBackwards("Blackout");
            _blackoutAnimation.Seek(_blackoutAnimation.CurrentAnimationPosition);
            _fishCurrentlySuffocating = false;
        }
    }

}
