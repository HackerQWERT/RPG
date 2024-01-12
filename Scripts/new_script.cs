using Godot;

public partial class Node : Godot.Node
{
	private AnimatedSprite2D _animatedSprite;

	public override void _Input(InputEvent @event)
	{
		PlayerState playerState = PlayerState.Idle;

		GD.Print(@event.AsText());
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
	}
}