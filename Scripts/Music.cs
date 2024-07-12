using Godot;
using System;

public partial class Music : Node2D
{
	[Export]
	public AudioStreamPlayer 你干嘛哎呦MusicPlayer;
	[Export]
	public AudioStreamPlayer 鸡MusicPlayer;
	[Export]
	public AudioStreamPlayer 鸡你太美MusicPlayer;
	[Export]
	public AudioStreamPlayer 哇真的是你呀MusicPlayer;
	[Export]
	public AudioStreamPlayer 背景MusicPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (哇真的是你呀MusicPlayer.Playing == false &&
		 背景MusicPlayer.Playing == false &&
		  鸡你太美MusicPlayer.Playing == false &&
		   鸡MusicPlayer.Playing == false &&
			你干嘛哎呦MusicPlayer.Playing == false)
		{
			背景MusicPlayer.Play();
		}
	}
}
