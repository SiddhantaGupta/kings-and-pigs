using Godot;
using System;
using System.ComponentModel;

public partial class DropHeart : Node
{
	public PackedScene heartScene;
	public SceneTree tree;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		heartScene = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Collectables/Heart/heart.tscn");
		tree = GetTree();
	}

	public override void _Notification(int what)
	{
		if (what == NotificationUnparented)
		{
			Vector2 playerGlobalPosition = Vector2.Zero;
			foreach (Node child in tree.CurrentScene.GetChildren())
			{
				if (child is Player player)
				{
					playerGlobalPosition = player.GlobalPosition;
				}
			}
			Node2D parent = GetParent<CharacterBody2D>();
			Vector2 dir = Vector2.Zero;
			if (playerGlobalPosition.X > parent.GlobalPosition.X)
			{
				dir = Vector2.Left;
			}
			else
			{
				dir = Vector2.Right;
			}
			Heart heart = heartScene.Instantiate<Heart>();
			heart.GlobalPosition = parent.GlobalPosition;
			heart.ApplyCentralImpulse(new Vector2(dir.X * 200, -200));
			tree.CurrentScene.AddChild(heart);
		}
	}
}
