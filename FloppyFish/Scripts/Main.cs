using Godot;
using System;
using System.Collections.Generic;

public class Main : Node
{
    [Export]
    public PackedScene PlatformScene;
    [Export]
    public float PlatformSpawnInterval = 10;
    private Position2D _platformSpawnPos;
    private Timer _platformSpawnTimer;

    private Fish _fish;

    private List<CollectableType> _collectedCollectables;

    public override void _Ready()
    {
        _platformSpawnPos = GetNode<Position2D>("PlatformSpawnPosition");
        _platformSpawnTimer = GetNode<Timer>("PlatformSpawnTimer");
        _fish = GetNode<Fish>("Fish");
        
        StartGame();
    }

    // We can call this on button press later
    public void StartGame()
    {
        _platformSpawnTimer.Start(2f);
        _fish.Show();
    }

    public void OnPlatformSpawnTimeOut()
    {
        _platformSpawnTimer.Start(PlatformSpawnInterval);
        Platform plat = (Platform)PlatformScene.Instance();
        plat.Position = _platformSpawnPos.Position;
        AddChild(plat);

        PathFollow2D path = new PathFollow2D();
        path.Loop = false;
        var parent = GetNode<Path2D>("PlatformPath");
        parent.AddChild(path);
        plat.Spawn(path);
    }

    public void CollectableCollected(CollectableType type)
    {
        GD.Print($"I collected a {nameof(type)}");
    }
}
