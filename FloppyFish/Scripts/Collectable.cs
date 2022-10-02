using Godot;
using System;

public class Collectable : RigidBody2D
{
    [Signal]
    public delegate void CollectableCollected(CollectableType type);

    [Export]
    public CollectableType Type = CollectableType.Empty;
    private AnimatedSprite _spriteAnim;

    [Signal]
    public delegate void Collected();

    public override void _Ready()
    {
        _spriteAnim = GetNode<AnimatedSprite>("AnimatedSprite");
    }

   
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("action"))
            EmitSignal(nameof(Collected));
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if(body.IsInGroup("Fish"))
        {
            EmitSignal("CollectableCollected", Type);
        }
    }

    public void Spawn(CollectableType type, string spriteAnimName)
    {
        Type = type;
        _spriteAnim.Animation = spriteAnimName;
    }
}
