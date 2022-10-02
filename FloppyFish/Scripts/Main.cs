using Godot;
using System;
using System.Collections.Generic;

public class Main : Node
{
    [Export]
    public PackedScene PlatformScene;
    private Position2D _platformSpawnPos;

    [Export]
    public PackedScene Fish;
    private Position2D _fishSpawnPos;

    private List<CollectableType> _collectedCollectables;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _platformSpawnPos = GetNode<Position2D>("PlatformSpawnPosition");
        _fishSpawnPos = GetNode<Position2D>("FishSpawnPosition");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void StartGame()
    {

    }
}
