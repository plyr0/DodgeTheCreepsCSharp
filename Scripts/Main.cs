using Godot;

public class Main : Node
{
	[Export] public PackedScene mobScene;

	private int score = 0;

	public async void OnNewGame()
	{
		score = 0;

		GetTree().CallGroup("mobs", "queque_free");
		GetNode<Player>("Player").Start(GetNode<Position2D>("StartPosition").Position);
		GetNode<AudioStreamPlayer2D>("Music").Play();

		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(score);
		hud.ShowMessage("Get ready...");
		
		var startTimer = GetNode<Timer>("StartTimer");
		startTimer.Start();

		await ToSignal(startTimer, "timeout");

		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Timer>("MobTimer").Start();
	}

	public void OnGameOver()
	{
		GetNode<AudioStreamPlayer2D>("Music").Stop();
		GetNode<AudioStreamPlayer2D>("DeathSound").Play();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Timer>("MobTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
	}

	public override void _Ready()
	{
		GD.Randomize();
		GetNode<Button>("HUD/Button").GrabFocus();
	}

	public override void _Process(float delta){}

	public void On_MobTimer_timeout()
	{
		var path = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		path.UnitOffset = GD.Randf();
		var mob = mobScene.Instance<Mob>();
		AddChild(mob);

		mob.Position = path.Position;
		var direction = path.Rotation + Mathf.Deg2Rad(90);
		direction += (float)GD.RandRange(Mathf.Deg2Rad(-45), Mathf.Deg2Rad(45));
		mob.Rotation = direction;

		var velocity = new Vector2((float)GD.RandRange(mob.minSpeed, mob.maxSpeed), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
	}

	public void On_ScoreTimer_timeout()
	{
		++score;
		GetNode<HUD>("HUD").UpdateScore(score);
	}
}
