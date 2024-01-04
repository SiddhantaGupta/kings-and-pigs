using Godot;
using System;
using System.Collections;

public partial class Diamond : Area2D
{
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback animationPlayback;
	private SignalBus signalBus;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationPlayback = animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		signalBus = GetNode<SignalBus>("/root/SignalBus");
	}

	private void Hit()
	{
		animationPlayback.Travel("Hit");
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			signalBus.EmitSignal(SignalBus.SignalName.DiamondCollected, 1);
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(this, "position", Position - new Vector2(0, 40), 0.2);
			// Create a new tween to change two properties at the same time.
			// Otherwise one will happen after the other.
			// Tween tween2 = GetTree().CreateTween();
			// tween2.TweenProperty(this, "modulate:a", 0, 0.3);
			tween.TweenCallback(new Callable(this, MethodName.Hit));
		}
	}

	private void OnAnimationTreeAnimationFinished(StringName anim_name)
	{
		if (anim_name == "Hit")
		{
			QueueFree();
		}
	}

}
