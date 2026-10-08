using Godot;
using System;

public partial class Main : Node2D
{
    [Export] Button pongButton;
    [Export] Button snakeButton;

    string pongScene = "res://Scenes/Pong/Pong.tscn";
    string snakeScene = "res://Scenes/Snake/Snake.tscn";

    public override void _Ready()
    {
        pongButton.Pressed += PongGame;
        snakeButton.Pressed += SnakeGame;
    }

    void PongGame ()
    {
        GetTree().ChangeSceneToFile(pongScene);
    }

    void SnakeGame()
    {
        GetTree().ChangeSceneToFile(snakeScene);
    }
}
