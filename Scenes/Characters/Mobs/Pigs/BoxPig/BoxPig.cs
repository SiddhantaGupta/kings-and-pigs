using Godot;
using System;

public partial class BoxPig : CharacterBody2D
{
	[Export]
	public CharacterBody2D player;
	public PackedScene throwableBoxScene;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public const int throwStrength = 100;
	public bool isPlayerInRange = false;

	public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback animationPlayback;
	public Sprite2D sprite;
	public Marker2D boxMarker;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		throwableBoxScene = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Box/throwable_box.tscn");
		sprite = GetNode<Sprite2D>("Sprite2D");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		boxMarker = GetNode<Marker2D>("BoxMarker");
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
			boxMarker.Position = new Vector2(-Math.Abs(boxMarker.Position.X), boxMarker.Position.Y);

		}
		else if (player.Position.X > Position.X)
		{
			sprite.FlipH = true;
			boxMarker.Position = new Vector2(Math.Abs(boxMarker.Position.X), boxMarker.Position.Y);
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
				ThrowableBox throwableBox = throwableBoxScene.Instantiate<ThrowableBox>();
				throwableBox.Position = boxMarker.Position;
				AddChild(throwableBox);
				// The LinearVelocity needs to be set after the call to AddChild
				// because no GlobalPosition exists for box before it.
				// both the below statement will give the same result.
				throwableBox.LinearVelocity =
					new Vector2(throwStrength, 0)
					.Rotated(throwableBox.GlobalPosition.AngleToPoint(player.GlobalPosition));
				// throwableBox.LinearVelocity = (player.GlobalPosition - throwableBox.GlobalPosition).Normalized() * throwStrength;

				animationPlayback.Travel("Pick");
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
	}
}
