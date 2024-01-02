using Godot;
using System;

public partial class BoxPig : CharacterBody2D
{
	[Export]
	public CharacterBody2D player;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public bool isPlayerInRange = false;

	public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback animationPlayback;
	public Sprite2D sprite;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationPlayback = animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		animationTree.Active = true;
		animationPlayback.Travel("Idle");

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		Velocity = velocity;
		// MoveAndSlide();
		UpdateFacingDirection();
		MoveAndCollide(Vector2.Zero);
	}

	public void UpdateFacingDirection()
	{
		if (player.Position.X < Position.X)
		{
			sprite.FlipH = false;
		}
		else if (player.Position.X > Position.X)
		{
			sprite.FlipH = true;
		}
	}

	private void OnPlayerDetectionBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			isPlayerInRange = true;
			animationPlayback.Travel("Throw");
		}
	}

	private void OnPlayerDetectionBodyExited(Node2D body)
	{
		if (body is Player)
		{
			isPlayerInRange = false;
		}
	}

	private void OnAnimationTreeAnimationFinished(StringName anim_name)
	{
		switch (anim_name)
		{
			case "Throw":
				animationPlayback.Travel("Pick");
				// Throw the box
				break;

			case "Pick":
				if (isPlayerInRange)
				{
					animationPlayback.Travel("Throw");
				}
				else
				{
					animationPlayback.Travel("Idle");
				}
				break;

			default:
				break;
		}
		if (anim_name == "Pick")
		{

		}
	}
}
