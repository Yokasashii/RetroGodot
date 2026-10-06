using Godot;
using System;

public partial class Main : Node2D
{
    [Export] Button pongButton;

    string pongScene = "res://Scenes/Pong/Pong.tscn";

    public override void _Ready()
    {
        pongButton.Pressed += PongGame;
    }

    void PongGame ()
    {
        GetTree().ChangeSceneToFile(pongScene);
    }
}
