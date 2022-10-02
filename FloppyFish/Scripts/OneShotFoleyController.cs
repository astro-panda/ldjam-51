using Godot;
using System.Collections.Generic;
using System;

public class OneShotFoleyController : AudioStreamPlayer
{
    [Export]
    public Dictionary<string, AudioStream> Streams {get;set;}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void Play(string streamKey, Vector2 velocity)
    {
        if(string.IsNullOrWhiteSpace(streamKey))
        {
            GD.PrintErr($"Attempted to play a null stream key on '{Name}'");
            return;
        }

        if(Streams.TryGetValue(streamKey, out AudioStream audio))
        {            
            Stream = audio;
            base.Play();         
        }
        else{
            GD.PrintErr($"Cannot find audio by key '{streamKey}'");
        }
    }
}
