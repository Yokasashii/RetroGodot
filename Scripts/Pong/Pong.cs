using Godot;
using Godot.Collections;
using System;

public partial class Pong : Node2D
{
    [Export] PackedScene scenePlayer1;
    [Export] PackedScene scenePlayer2;
    [Export] PackedScene sceneBall;
    [Export] Label score;
    [Export] Timer gameTimer;

    Array<Area2D> elements = [];
    Area2D player1, player2, ball;
    RandomNumberGenerator rand = new RandomNumberGenerator();

    Vector2 screenSize;

    bool onGame = false;
    bool keyPressZ, keyPressS, keyPressUp, keyPressDown = false;
    float ballSpeed;
    int playerSpeed = 300;
    int scorePlayer1 = 0;
    int scorePlayer2 = 0;
    int playerHeight;


    public override void _Ready()
    {
        elements = [
            player1 = (Area2D)scenePlayer1.Instantiate(),
            player2 = (Area2D)scenePlayer2.Instantiate(),
            ball = (Area2D)sceneBall.Instantiate(),];
        for (int i = 0; i < 3; i++) { AddChild(elements[i]); }

        screenSize = GetWindow().Size;
        playerHeight = ((Sprite2D)player1.GetChild(0)).Texture.GetHeight();
        gameTimer.Timeout += SpeedUpBall;

        StartRound();
    }

    public override void _Process(double pDelta)
    {
        float lDelta = (float)pDelta;

        if (onGame)
        {
            CheckRound();
            CheckInput();

            ball.Position += new Vector2(ball.Scale.X, ball.Scale.Y) * ballSpeed * lDelta;
            if (ball.Position.Y >= screenSize.Y || ball.Position.Y <= 0) ball.Scale *= new Vector2(1, -1);

            if (keyPressZ && player1.Position.Y >= playerHeight / 2) player1.Position += Vector2.Up * playerSpeed * 2 * lDelta;
            if (keyPressS && player1.Position.Y <= screenSize.Y - playerHeight / 2) player1.Position += Vector2.Down * playerSpeed * 2 * lDelta;
            if (keyPressUp && player2.Position.Y >= playerHeight / 2) player2.Position += Vector2.Up * playerSpeed * 2 * lDelta;
            if (keyPressDown && player2.Position.Y <= screenSize.Y - playerHeight / 2) player2.Position += Vector2.Down * playerSpeed * 2 * lDelta;
        }
    }

    void StartRound()
    {
        player1.Position = new Vector2(50, screenSize.Y / 2);
        player2.Position = new Vector2(screenSize.X - 50, screenSize.Y / 2);
        ball.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2);
        ball.Scale = new Vector2(rand.Randf() > 0.5 ? 1 : -1, rand.Randf() > 0.5 ? 1 : -1);
        ballSpeed = 200;
        score.Text = scorePlayer1 + " : " + scorePlayer2;
        gameTimer.Start(5);
        onGame = true;
    }

    void CheckRound() 
    {
        if (ball.Position.X > screenSize.X) {scorePlayer1++; onGame = false;}
        if (ball.Position.X < 0) {scorePlayer2++; onGame = false;}
        if (!onGame) StartRound();
    }

    void CheckInput()
    {
        keyPressZ = Input.IsKeyPressed(Key.Z);
        keyPressS = Input.IsKeyPressed(Key.S);
        keyPressUp = Input.IsKeyPressed(Key.Up);
        keyPressDown = Input.IsKeyPressed(Key.Down);
    }

    void SpeedUpBall()
    {
        if (onGame) ballSpeed += 50;
    }
}
