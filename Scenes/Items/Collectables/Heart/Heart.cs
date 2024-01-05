using Godot;
using System;

public partial class Heart : RigidBody2D
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

	// A RigidBody2D's OnBodyEntered event is only fired when it collides with another body being controlled
	// by the Physics Engine therefore Area2D is being used.
	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			signalBus.EmitSignal(SignalBus.SignalName.HeartCollected, 20);
			animationPlayback.Travel("Hit");
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
