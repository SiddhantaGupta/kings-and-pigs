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
