using Godot;
using System;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnPlayButtonPressed()
	{
		GD.Print("pressed");
		GetTree().ChangeSceneToFile("res://Scenes/Level1/level_1.tscn");
	}
}





