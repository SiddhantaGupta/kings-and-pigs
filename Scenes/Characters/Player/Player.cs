using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public CollisionShape2D collider;
	public Sprite2D sprite;
	public Area2D attackArea;
	public AnimationTree animationTree;
	// Using AnimationNodeStateMachinePlayback automatically picks one animation to play.
	// Instead of setting all nodes in AnimationTree to true or false you only have to pick the animation you want to play.
	// This also ensures you don't set two items true at the same time which can lead to unintended bugs.
	public AnimationNodeStateMachinePlayback animationPlayback;
	public Vector2 direction;

	public override void _Ready()
	{
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		attackArea = GetNode<Area2D>("AttackArea");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationPlayback = animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();

		attackArea.Monitoring = false;
		animationTree.Active = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		UpdateFacingDirection();
		UpdateAnimationParameters();
		MoveAndSlide();
	}

	public void UpdateAnimationParameters()
	{
		animationTree.Set("parameters/Move/blend_position", direction.X);
		if (!IsOnFloor())
		{
			animationPlayback.Travel("Jump");
		}
		else
		{
			animationPlayback.Travel("Move");
		}

		if (Input.IsActionJustPressed("attack"))
		{
			animationPlayback.Travel("Attack");
		}
	}

	public void UpdateFacingDirection()
	{
		if (direction.X == -1)
		{
			sprite.FlipH = true;
			// The sprite is not centered so the collider (Hitboxes) need to be flipped as well.
			collider.Position = new Vector2(Math.Abs(collider.Position.X), collider.Position.Y);
		}
		else if (direction.X == 1)
		{
			sprite.FlipH = false;
			collider.Position = new Vector2(-Math.Abs(collider.Position.X), collider.Position.Y);
		}
	}
}
