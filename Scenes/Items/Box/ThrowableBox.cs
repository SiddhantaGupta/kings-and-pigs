using Godot;
using System;

public partial class ThrowableBox : RigidBody2D
{
	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			// reduce player health
			QueueFree();
			GD.Print("Player collided");
		}
	}
}
