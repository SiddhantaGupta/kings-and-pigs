using Godot;
using System;

public partial class Health : CanvasLayer
{
	[Signal]
	public delegate void HealthDepletedEventHandler();
	private int health = 100;
	private Label healthLabel;

	public override void _Ready()
	{
		healthLabel = GetNode<Label>("Label");
		setHealth(100);
		SignalBus signalBus = GetNode<SignalBus>("/root/SignalBus");
		signalBus.Connect(SignalBus.SignalName.HeartCollected, new Callable(this, MethodName.OnDiamondCollected));
	}

	public void OnDiamondCollected(int count)
	{
		setHealth(health + count);

	}

	public void hit(int damage)
	{
		setHealth(health - damage);
		if (health <= 0)
		{
			EmitSignal(SignalName.HealthDepleted);
		}
	}

	private void setHealth(int hp)
	{
		health = hp;
		healthLabel.Text = $"Health: {health}";
	}
}
