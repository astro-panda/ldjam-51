using Godot;
using System;

public class Fish : RigidBody2D
{
    [Export]
    public int Speed = 20;

    [Export]
    public CollectableType CollectedItem = CollectableType.Empty;

    public Vector2 ScreenSize; // Size of the game window.

    public bool IsInWater = false;

    public Vector2 WaterFlow = new Vector2((float)-0.946,(float)-0.326);

    public Vector2 PreviousPosition;

    public bool canFlop = true;

    public Vector2 vectorToDraw = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print($"Rotation: {Rotation}");
        ScreenSize = GetViewportRect().Size;
        PreviousPosition = Position;
        GetNode<Timer>("FlopCooldownTimer").Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var velocity = Vector2.Zero; // The player's movement vector.
        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        if (IsInWater)
        {
            if (Input.IsActionPressed("move_right"))
            {
                velocity.x += 1;
                animatedSprite.FlipH = true;
            }

            if (Input.IsActionPressed("move_left"))
            {
                velocity.x -= 1;
                animatedSprite.FlipH = false;
            }

            if (Input.IsActionPressed("move_down"))
            {
                velocity.y += 1;
                ApplyTorqueImpulse(animatedSprite.FlipH ? 80 : -80);
            }

            if (Input.IsActionPressed("move_up"))
            {
                velocity.y -= 1;
                ApplyTorqueImpulse(animatedSprite.FlipH ? -80 : 80);
            }
        }


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

        if (IsInWater)
        {
            if (Rotation != 0)
            {
                ApplyTorqueImpulse(-50 * Rotation);
            }

            ApplyCentralImpulse(WaterFlow);
        }

        var locChange = Math.Abs(Position.DistanceTo(PreviousPosition));

        if (!IsInWater && locChange < 1 && canFlop)
        {
            var flopDirection = 0.0;
            var flopRot = 0f;

            var moving = false;

            if (Input.IsActionPressed("move_right"))
            {
                flopDirection = GD.RandRange((Mathf.Pi / 6), (Mathf.Pi / 3));
                flopRot = 5000.0f;
                moving = true;
            }

            if (Input.IsActionPressed("move_left"))
            {
                flopDirection = GD.RandRange((Mathf.Pi * (2/(float)3)), (Mathf.Pi * (5/(float)6)));
                flopRot = -5000.0f;
                moving = true;
            }

            if (Input.IsActionPressed("move_up"))
            {
                flopDirection = GD.RandRange((Mathf.Pi * (1/(float)3)), (Mathf.Pi * (2/(float)3)));
                moving = true;
            }

            if (moving)
            {
                var flopVector = new Vector2(500 * Mathf.Cos(flopDirection != 0 ?  (float)flopDirection : (float)(Mathf.Pi / 2)),
                                             -500 * Mathf.Sin((float)flopDirection));

                ApplyTorqueImpulse(flopRot);
                ApplyCentralImpulse(flopVector);

                canFlop = false;
            }
        }

        PreviousPosition = Position;
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

    public void OnFlopCooldownTimerTimeout()
    {
        canFlop = true;
    }

    public override void _Draw()
    {
        // DrawLine(Position, vectorToDraw, Color.ColorN("Red", 1));
    }
}
