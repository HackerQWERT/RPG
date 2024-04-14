using Godot;
using System;

public partial class End : Area2D
{
	[Export]
	public Panel WinPanel;
	[Export]
	public Music music;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("End.cs ready");
		BodyEntered += OnBodyEntered;


	}

	private void OnBodyEntered(Node2D body)
	{
		WinPanel.Visible = true;
		GD.Print("End.cs body entered");
		if (music.背景MusicPlayer.Playing == true && music.鸡你太美MusicPlayer.Playing == false)
		{
			music.背景MusicPlayer.Stop();
			music.鸡你太美MusicPlayer.Play();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
