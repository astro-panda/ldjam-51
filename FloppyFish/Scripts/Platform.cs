using Godot;

public class Platform : RigidBody2D
{   
    public bool IsInWater = false;
    [Export]
    public float Damping = -1f;
    [Export]
    public float PullForce = 10f;
    [Export]
    public float Speed = 80f;

    public Vector2 WaterFlow = new Vector2((float)-0.946,(float)-0.326);

    private Vector2 _targetPos = Vector2.Zero;
    private PathFollow2D _followPos;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(_followPos != null)
        {
            if(_followPos.UnitOffset == 1)
            {
                // Hide();
                // GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
                QueueFree();
            }
            _followPos.Offset += Speed * delta;
            _targetPos = _followPos.Transform.origin;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        ApplyTorqueImpulse((0 - Rotation) * 10000f - (0 - AngularVelocity) * -15f);

        ApplyCentralImpulse(CalculateForce(_targetPos));
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

    public void Spawn(PathFollow2D followPos)
    {
        _followPos = followPos;
    }

    private Vector2 CalculateForce(Vector2 wantPos)
    {
        return (wantPos - Position) * PullForce - (Vector2.Zero - LinearVelocity) * Damping;
    }
}
