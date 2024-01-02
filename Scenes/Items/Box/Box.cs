using Godot;
using System;

public partial class Box : RigidBody2D
{
	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			// reduce player health
			GD.Print("Player collided");
		}
		QueueFree();
	}
}
