using Godot;
using System;
using System.Collections.Generic;

public class AudioController : Node
{
	private const string Fish = nameof(Fish);
	private const string Log = nameof(Log);
	private const string Trash = nameof(Trash);
	private const string Collectible = nameof(Collectible);

	private Dictionary<string, OneShotFoleyController> FoleyControllers {get;set;} = new Dictionary<string, OneShotFoleyController>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Func<string, OneShotFoleyController> getControllerInstance = (key) =>
		{
			return GetNode<OneShotFoleyController>($"{key}Foley");
		};

		FoleyControllers.Add(Fish, getControllerInstance(Fish));
		FoleyControllers.Add(Log, getControllerInstance(Log));
		FoleyControllers.Add(Trash, getControllerInstance(Trash));
		FoleyControllers.Add(Collectible, getControllerInstance(Collectible));
	}

	public void OnFoleyTriggered(string foleyGroup, string foleyKey, Vector2 actionVelocity)
	{
		GD.Print($"Foley {foleyGroup}-{foleyKey}");

		if(FoleyControllers.TryGetValue(foleyGroup, out OneShotFoleyController controller))
		{
			controller.Play(foleyKey, actionVelocity);
		}
		else 
		{
			GD.Print($"Unable to find One Shot Foley Controller '{foleyGroup}'");
		}
	}
}
