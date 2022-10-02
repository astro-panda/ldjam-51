using Godot;
using System;

public class Platform : RigidBody2D
{   
    public bool IsInWater = false;
    [Export]
    public float Damping = -1f;
    [Export]
    public float PullForce = 10f;

    public Vector2 WaterFlow = new Vector2((float)-0.946,(float)-0.326);

    private Position2D _targetPos;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _targetPos = GetNode<Position2D>("TargetPosition");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Rotation != 0)
        {
            ApplyTorqueImpulse(-15000 * Rotation);
        }

        if (IsInWater)
        {
            ApplyCentralImpulse(20 * WaterFlow);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        AddCentralForce((_targetPos.Position - Position) * PullForce - (Vector2.Zero - LinearVelocity) * Damping);
    }

    public void OnWaterBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup("platforms"))
        {
            IsInWater = true;
            AppliedForce = Vector2.Zero;
            AddCentralForce(3000 * Vector2.Up);
        }
    }

    public void OnWaterBodyExited(PhysicsBody2D body)
    {
        if (body.IsInGroup("platforms"))
        {
            IsInWater = false;
            AppliedForce = Vector2.Zero;
            AddCentralForce(5 * Vector2.Down);
        }
    }
}
