using Godot;
using System;

public partial class DeathArea : Area2D
{
    [Export]
    public Music music;
    [Export]
    public Node2D 复活点;

    [Export]
    public CharacterBody2D player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("DeathArea.cs ready");
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print("DeathArea.cs body entered");
        //随机播放鸡MusicPlayer或你干嘛哎呦MusicPlayer
        Random random = new Random();
        int randomNumber = random.Next(0, 2);
        if (randomNumber == 0)
            music.你干嘛哎呦MusicPlayer.Play();
        else
            music.鸡MusicPlayer.Play();

        player.Position = 复活点.Position;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
