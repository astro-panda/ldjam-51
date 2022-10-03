using Godot;
using System;

public class Fish : RigidBody2D
{
    [Export]
    public int Speed = 20;

    [Export]
    public CollectableType CollectedItem = CollectableType.Empty;

    [Signal]
    public delegate void Die();

    [Signal]
    public delegate void SuffcationStop();

    [Signal]
    public delegate void SuffcationStart();

    [Signal]
    public delegate void SetWaterAirValue(int value);

    [Signal]
    public delegate void HoldingCollectible();

    [Signal]
    public delegate void Collected();

    [Signal]
    public delegate void WaterEntered(Vector2 velocity);

    [Signal]
    public delegate void MovementChanged(string foleyGroup, string foleyKey);

    public Vector2 ScreenSize; // Size of the game window.

    public bool IsInWater = false;

    public Vector2 WaterFlow = new Vector2((float)-0.946,(float)-0.326);

    public bool canFlop = true;

    public Vector2 vectorToDraw = Vector2.Zero;

    public Timer AirTimer;

    public Timer InvFlopTimer;

    [Export]
    public int AirCountDown = 10;

    public bool HasCollectible = false;

    public bool OnPlatform = false;
    private string CurrentPlatformKey;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        GetNode<Timer>("FlopCooldownTimer").Start();
        AirTimer = GetNode<Timer>("AirTimer");
        InvFlopTimer = GetNode<Timer>("InvoluntaryFlopTimer");
        InvFlopTimer.Start();
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

        if (!IsInWater && OnPlatform && canFlop)
        {
            var flopDirection = 0.0;
            var flopRot = 0f;

            var moving = false;

            if (Input.IsActionJustPressed("move_right"))
            {
                flopDirection = GD.RandRange((Mathf.Pi / 6), (Mathf.Pi / 3));
                flopRot = 5000.0f;
                moving = true;
            }

            if (Input.IsActionJustPressed("move_left"))
            {
                flopDirection = GD.RandRange((Mathf.Pi * (2/(float)3)), (Mathf.Pi * (5/(float)6)));
                flopRot = -5000.0f;
                moving = true;
            }

            if (Input.IsActionJustPressed("move_up"))
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
                OnPlatform = false;
                
                if(!string.IsNullOrWhiteSpace(CurrentPlatformKey))
                    EmitSignal(nameof(MovementChanged), CurrentPlatformKey, "MoveFlop");

                canFlop = false;
            }
        }
    }

    public void OnFishBodyEntered(PhysicsBody2D body)
    {
        if(body.IsInGroup("collectable") && !HasCollectible)
        {
            var collectable = (Collectable)body;
            CollectedItem = collectable.Type;
            HasCollectible = true;
            collectable.Hide();
            collectable.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
            EmitSignal("HoldingCollectible");            
            EmitSignal(nameof(Collected));
        }

        if (body.IsInGroup("platforms"))
        {
            if (Position.y < body.Position.y)
            {
                OnPlatform = true;
            }            
        }

        if(body.IsInGroup("log"))
        {
            CurrentPlatformKey = "Log";
        }
        else if (body.IsInGroup("trash"))
        {
            CurrentPlatformKey = "Trash";
        }        

    }

    public void OnWaterBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup("fish"))
        {
            IsInWater = true;

            AirTimer.Stop();
            EmitSignal(nameof(SuffcationStop));
            EmitSignal(nameof(WaterEntered));
            AirCountDown = 10;
        }
    }

    public void OnWaterBodyExited(PhysicsBody2D body)
    {
        if (body.IsInGroup("fish"))
        {
            IsInWater = false;

            AirTimer.Start();
            EmitSignal(nameof(SuffcationStart));
        }
    }

    public void OnFlopCooldownTimerTimeout()
    {
        canFlop = true;
    }

    public void OnAirTimerTimeout()
    {
        if (AirCountDown > 0)
        {
            AirCountDown -= 1;
            EmitSignal(nameof(SetWaterAirValue), AirCountDown);

            if (AirCountDown == 0)
            {
                Hide();
                GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
                EmitSignal("Die");
            }
        }
        
    }

    public void InvoluntaryFlopTimeout()
    {
        if (!IsInWater && OnPlatform)
        {
            var involuntaryFlopDirection = GD.RandRange((Mathf.Pi * (1/(float)3)), (Mathf.Pi * (2/(float)3)));
            var invFlopVector = new Vector2(200 * Mathf.Cos(involuntaryFlopDirection != 0 ?  (float)involuntaryFlopDirection : (float)(Mathf.Pi / 2)),
                                            -200 * Mathf.Sin((float)involuntaryFlopDirection));
            ApplyCentralImpulse(invFlopVector);
            OnPlatform = false;

            if(!string.IsNullOrWhiteSpace(CurrentPlatformKey))
                EmitSignal(nameof(MovementChanged), CurrentPlatformKey, "Flop");
        }

        InvFlopTimer.WaitTime = (float)GD.RandRange(0.5, 2.0);
        InvFlopTimer.Start();
    }

    

    public void OnBodyExited(PhysicsBody2D node)
    {
        GD.Print("Exit");

        if(node.IsInGroup("log") || node.IsInGroup("trash"))        
            CurrentPlatformKey = null;
    }
}
