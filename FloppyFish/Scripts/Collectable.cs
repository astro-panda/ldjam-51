using Godot;
using System;

public class Collectable : RigidBody2D
{
    [Signal]
    public delegate void CollectableCollected(CollectableType type);

    [Export]
    public CollectableType Type = CollectableType.Empty;
    private AnimatedSprite _spriteAnim;   

    public override void _Ready()
    {
        _spriteAnim = GetNode<AnimatedSprite>("AnimatedSprite");
    }

   
    public override void _Process(float delta)
    {
        
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if(body.IsInGroup("fish"))
        {
            GD.Print("Collected collectable");
            EmitSignal("CollectableCollected", Type);            
        }
    }

    public void Spawn(CollectableType type, string spriteAnimName)
    {
        Type = type;
        _spriteAnim.Animation = spriteAnimName;
    }
}
