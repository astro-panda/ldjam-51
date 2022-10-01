using Godot;
using System;

public class Collectable : Area2D
{
    [Export]
    public CollectableType Type = CollectableType.Empty;
    private AnimatedSprite _spriteAnim;

    public override void _Ready()
    {
        _spriteAnim = GetNode<AnimatedSprite>("AnimatedSprite");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Spawn(CollectableType type, string spriteAnimName)
    {
        Type = type;
        _spriteAnim.Animation = spriteAnimName;
    }
}
