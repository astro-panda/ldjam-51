using Godot;
using System;

public class OctopusMech : Area2D
{
    [Signal]
    public delegate void CollectedCollectable(CollectableType type);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void OnOctopusMechBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup("fish"))
        {
            var fish = (Fish)body;
            if (fish.HasCollectible)
            {
                EmitSignal("CollectedCollectable");
                fish.HasCollectible = false;
                fish.CollectedItem = CollectableType.Empty;
            }
        }
    }
}
