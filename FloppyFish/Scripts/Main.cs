using Godot;
using System;
using System.Collections.Generic;

public class Main : Node
{
	[Export]
	public List<PackedScene> PlatformScenes { get; set; } = new List<PackedScene>();

	[Export]
	public float PlatformSpawnInterval = 10;
	private Position2D _platformSpawnPos;
	private Timer _platformSpawnTimer;

	private Timer _collectableSpawnTimer;

	private bool _canSpawnCollectable = false;

	private Fish _fish;
	private List<CollectableType> _collectedCollectables;
	public override void _Ready()
	{
		GD.Randomize();
		_platformSpawnPos = GetNode<Position2D>("PlatformSpawnPosition");
		_platformSpawnTimer = GetNode<Timer>("PlatformSpawnTimer");
		_collectableSpawnTimer = GetNode<Timer>("CollectableSpawnTimer");
		_fish = GetNode<Fish>("Fish");
        // PlatformScenes = new List<PackedScene>(){
        //     GD.Load<PackedScene>("res://Scenes/Platform.tscn"),
        //     GD.Load<PackedScene>("res://Scenes/Platform2.tscn")
        // };

        StartGame(); // We gona start it up.
    }

    // We can call this on button press later
    public void StartGame()
    {
        _platformSpawnTimer.Start(2f);
        _collectableSpawnTimer.Start(2f);
		var startPos = GetNode<Position2D>("FishStartLocation");
		GD.Print($"Start Pos: {startPos}, {startPos.Position}");
        _fish.Start(startPos.Position);
    }

	public void OnPlatformSpawnTimeOut()
	{
		_platformSpawnTimer.Start(PlatformSpawnInterval);
		PackedScene platformScene = PlatformScenes[Mathf.Clamp((int)GD.Randi(), 0, PlatformScenes.Count - 1)];

		GD.Print($"Picked {platformScene.ResourcePath}");

		Platform platform = platformScene.InstanceOrNull<Platform>();

		platform.Position = _platformSpawnPos.Position;
		AddChild(platform);

		PathFollow2D path = new PathFollow2D();
		path.Loop = false;
		var parent = GetNode<Path2D>("PlatformPath");
		parent.AddChild(path);

		platform.Spawn(path, _canSpawnCollectable);
		ResetCollectableSpawnTimer();
	}

	public void CollectableCollected(CollectableType type)
	{
		GD.Print($"I collected a {nameof(type)}");
	}

	public void AllowSpawnCollectable()
	{
		_canSpawnCollectable = true;
	}

	private void ResetCollectableSpawnTimer()
	{
		if(_canSpawnCollectable)
		{            
			_collectableSpawnTimer.WaitTime = (float)GD.RandRange(2d, 16d);
			_collectableSpawnTimer.Start();
			_canSpawnCollectable = false;
		}
	}
}
