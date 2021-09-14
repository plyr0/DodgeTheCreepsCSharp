using Godot;

public class Mob : RigidBody2D
{
	[Export] public float minSpeed = 150f;
	[Export] public float maxSpeed = 250f;

	public override void _Ready()
	{
		var anim = GetNode<AnimatedSprite>("AnimatedSprite");
		anim.Play();
		var anims = anim.Frames.GetAnimationNames();
		anim.Animation = anims[GD.Randi() % anims.Length];
	}

	public void On_VisibilityNotifier2D_screen_exited() => QueueFree();
}
