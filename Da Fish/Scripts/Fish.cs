using Godot;
using System;

public class Fish : RigidBody2D
{
   [Export]
    public int Speed = 20;

    public Vector2 ScreenSize; // Size of the game window.

    public bool IsInWater = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var velocity = Vector2.Zero; // The player's movement vector.

        if (IsInWater)
        {
            if (Input.IsActionPressed("move_right"))
            {
                velocity.x += 1;
            }

            if (Input.IsActionPressed("move_left"))
            {
                velocity.x -= 1;
            }

            if (Input.IsActionPressed("move_down"))
            {
                velocity.y += 1;
            }

            if (Input.IsActionPressed("move_up"))
            {
                velocity.y -= 1;
            }
        }

        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        ApplyCentralImpulse(velocity);
    }

    public void OnWaterBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup("fish"))
        {
            IsInWater = true;
        }
    }

    public void OnWaterBodyExited(PhysicsBody2D body)
    {
        if (body.IsInGroup("fish"))
        {
            IsInWater = false;
        }
    }
}
