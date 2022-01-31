using Godot;

public class Player : Area2D
{
	[Export] public float speed = 400f;

	[Signal] public delegate void Hit(); 

	private Vector2 screenSize = Vector2.Zero;
	private bool touchHeld = false;
	private Vector2 touchPosition = new Vector2();

	public override void _Ready()
	{
		screenSize = GetViewportRect().Size;
		Hide();
	}

	public override void _Input(InputEvent ev)
	{
		if (ev is InputEventMouseMotion motion)
		{
			touchHeld = motion.Pressure > 0;
			touchPosition = motion.Position;
		}
		else
		{
			base._Input(ev);
		}
	}

	public override void _Process(float delta)
	{
		var anim = GetNode<AnimatedSprite>("AnimatedSprite");

		var direction = Vector2.Zero;
		if (Input.IsActionPressed("move_up")) direction.y -= 1;
		if (Input.IsActionPressed("move_down")) direction.y += 1;
		if (Input.IsActionPressed("move_left")) direction.x -= 1;
		if (Input.IsActionPressed("move_right")) direction.x += 1;
		if (touchHeld)
		{
			var x = touchPosition.x - Position.x;
			if (Mathf.Abs(x) > 27)
				direction.x = x;

			var y = touchPosition.y - Position.y;
			if (Mathf.Abs(y) > 27)
				direction.y = y;
		}

		if (direction.Length() > 0)
		{
			direction = direction.Normalized();
			anim.Play();
		}
		else
		{
			anim.Stop();
		}

		Position += speed * direction * delta;

		Position = new Vector2(
			Mathf.Clamp(Position.x, 0, screenSize.x),
			Mathf.Clamp(Position.y, 0, screenSize.y));

		if (direction.x != 0)
		{
			anim.Animation = "right";
			anim.FlipH = direction.x < 0;
		}
		if (direction.y != 0)
		{
			anim.Animation = "up";
			anim.FlipV = direction.y > 0;
		}
	}

	public void Start(Vector2 newPosition)
	{
		Position = newPosition;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	public void On_Player_body_entered(PhysicsBody2D _)
	{
		Hide();
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
		EmitSignal(nameof(Hit));
	}
}
