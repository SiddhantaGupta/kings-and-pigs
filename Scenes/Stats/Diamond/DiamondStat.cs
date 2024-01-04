using Godot;
using System;

public partial class DiamondStat : CanvasLayer
{
	private int diamondsStat = 0;
	private Label diamondStatLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		diamondStatLabel = GetNode<Label>("Label");
		setDiamondStat(0);
		SignalBus signalBus = GetNode<SignalBus>("/root/SignalBus");
		signalBus.Connect(SignalBus.SignalName.DiamondCollected, new Callable(this, MethodName.OnDiamondCollected));
	}

	private void OnDiamondCollected(int count)
	{
		setDiamondStat(diamondsStat + count);
	}

	private void setDiamondStat(int diamondsCount)
	{
		diamondsStat = diamondsCount;
		diamondStatLabel.Text = $"Diamond: {diamondsStat}";
	}
}
