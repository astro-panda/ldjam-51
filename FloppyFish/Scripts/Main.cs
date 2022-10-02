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

    [Export]
    public PackedScene Fish;
    private Position2D _fishSpawnPos;

    private List<CollectableType> _collectedCollectables;

    public override void _Ready()
    {
        _platformSpawnPos = GetNode<Position2D>("PlatformSpawnPosition");
        _platformSpawnTimer = GetNode<Timer>("PlatformSpawnTimer");
        _fishSpawnPos = GetNode<Position2D>("FishSpawnPosition");

        StartGame();
    }

    // We can call this on button press later
    public void StartGame()
    {
        _platformSpawnTimer.Start(2f);
    }

    public void OnPlatformSpawnTimeOut()
    {
        _platformSpawnTimer.Start(PlatformSpawnInterval);
        Platform plat = (Platform)PlatformScene.Instance();
        plat.Position = _platformSpawnPos.Position;
        AddChild(plat);
    }
}
