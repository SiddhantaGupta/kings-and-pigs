using Godot;
using System;

public partial class ThrowableBox : Area2D
{
	public int Speed = 100;
	public Vector2 destination = Vector2.Zero;
	public Vector2 velocity = Vector2.Zero;

	public override void _PhysicsProcess(double delta)
	{
		if (velocity == Vector2.Zero && destination != Vector2.Zero)
		{
			// Both of the below approaches give the same result in different way
			// set speed in x direction then rotate x to match destination
			// velocity =
			// 	new Vector2(Speed, 0)
			// 	.Rotated(GlobalPosition.AngleToPoint(destination));
			// get direction then add speed 
			velocity = (destination - GlobalPosition).Normalized() * Speed;
		}

		if (velocity != Vector2.Zero)
		{
			Position += velocity * (float)delta;
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body == GetParent()) { return; }
		if (body is Player)
		{
			// reduce player health
			foreach (var child in body.GetChildren())
			{
				if (child is Health)
				{
					((Health)child).hit(20);

				}
			}
		}
		QueueFree();
	}
}
