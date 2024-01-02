using Godot;
using System;

public partial class AttackArea : Area2D
{

	private void OnBodyEntered(Node2D body)
	{
		foreach (var child in body.GetChildren())
		{
			if (child is Damageable)
			{
				((Damageable)child).hit(10);
			}
		}
	}

}


