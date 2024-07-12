using Godot;
using System;

public partial class Button : Godot.Button

{
	[Export]
	public Music music;
	[Export]
	public DeathArea deathArea;
	[Export]
	public Node2D 菜鸟点;

	[Export]
	public CharacterBody2D player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Button.cs ready");
		Pressed += OnButtonPressed;

	}

	private void OnButtonPressed()
	{
		GD.Print("Button.cs button pressed");

		player.Position = 菜鸟点.Position;
		music.你干嘛哎呦MusicPlayer.Play();
		this.Visible = false;
		deathArea.deathCount = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

