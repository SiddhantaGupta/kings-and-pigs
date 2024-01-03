using Godot;
using System;

public partial class ThrowableBox : RigidBody2D
{
	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			// reduce player health
			foreach (var child in body.GetChildren())
			{
				if (child is Health)
				{
					((Health)child).hit(20);
					QueueFree();
				}
			}
		}
	}
}
