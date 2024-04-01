using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{


	[Export]
	public CharacterBody2D Target;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position = Target.Position;
	}
}
