using Godot;
using System;

public class HUD : CanvasLayer
{
    
    private AnimationPlayer _blackoutAnimation;
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
        _blackoutAnimation.Play("Blackout");
    }

    public void OnFishSuffocatingStop()
    {
        _blackoutAnimation.Stop();
    }

}
