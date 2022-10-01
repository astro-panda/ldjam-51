using Godot;
using System;

public class Platform2 : RigidBody2D
{
    public bool IsInWater = false;

    public Vector2 WaterFlow = new Vector2((float)-0.946,(float)-0.326);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Rotation != 0)
        {
            ApplyTorqueImpulse(-15000 * Rotation);
        }

        ApplyCentralImpulse(20 * WaterFlow);
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
