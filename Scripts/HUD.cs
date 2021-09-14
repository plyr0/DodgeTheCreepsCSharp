using Godot;

public class HUD : CanvasLayer
{
	[Signal] public delegate void Start();

	public void UpdateScore(int newScore) => GetNode<Label>("ScoreLabel").Text = newScore.ToString();

	public void ShowMessage(string message)
	{
		GetNode<Label>("MessageLabel").Text = message;
		GetNode<Label>("MessageLabel").Show();
		GetNode<Timer>("MessageTimer").Start();
	}

	public async void ShowGameOver()
	{
		ShowMessage("Game Over");
		await ToSignal(GetNode<Timer>("MessageTimer"), "timeout");
		GetNode<Label>("MessageLabel").Text = "Dodge the C#reeps";
		GetNode<Label>("MessageLabel").Show();
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		GetNode<Button>("Button").Show();
	}

	public void On_Button_pressed()
	{
		GetNode<Button>("Button").Hide();
		EmitSignal(nameof(Start));
	}

	public void On_MessageTimer_timeout()
	{
		GetNode<Label>("MessageLabel").Hide();
	}
}
