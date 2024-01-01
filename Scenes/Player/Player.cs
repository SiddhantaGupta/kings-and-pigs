using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public AnimatedSprite2D animation;
	public AnimatedSprite2D sprite;
	public CollisionShape2D collider;

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		animation.Play("Idle");
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
			animation.Play("Jump");
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			if (direction.X != 0)
			{
				if (direction.X == -1)
				{
					sprite.FlipH = true;
					// collider position needs to be changed as the king sprite has empty space in the front
					collider.Position = new Vector2(Math.Abs(collider.Position.X), collider.Position.Y);
				}
				else if (direction.X == 1)
				{
					sprite.FlipH = false;
					collider.Position = new Vector2(-Math.Abs(collider.Position.X), collider.Position.Y);
				}
				velocity.X = direction.X * Speed;
				animation.Play("Run");
			}

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			animation.Play("Idle");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
