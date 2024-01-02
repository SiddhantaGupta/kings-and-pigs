using Godot;
using System;

public partial class Damageable : Node
{
	[Export]
	int health = 20;

	public void hit(int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			GetParent().QueueFree();
		}
	}
}
