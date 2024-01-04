using Godot;
using System;

public enum DoorTypes
{
	Enter,
	Exit
}

public partial class Door : Area2D
{
	[Export]
	public int level = 0;
	[Export]
	public DoorTypes type = DoorTypes.Enter;
	public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback animationPlayback;
	private bool isPlayerInArea = false;
	private SignalBus signalBus;
	public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationPlayback = animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		signalBus = GetNode<SignalBus>("/root/SignalBus");
	}
	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			animationPlayback.Travel("Open");
			isPlayerInArea = true;
		}
	}


	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			animationPlayback.Travel("Close");
			isPlayerInArea = false;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("interact") && type == DoorTypes.Exit)
		{
			signalBus.EmitSignal(SignalBus.SignalName.ExitDoorEntered, level);
		}
	}

}



