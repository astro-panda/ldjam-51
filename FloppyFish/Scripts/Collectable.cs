using Godot;
using System;

public class Collectable : Area2D
{
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

    public void Spawn(CollectableType type, string spriteAnimName)
    {
        Type = type;
        _spriteAnim.Animation = spriteAnimName;
    }
}
