using Godot;
using Godot.Collections;
using System;

public partial class Snake : Node2D
{
    [Export] PackedScene sceneHead;
    [Export] PackedScene sceneBody;
    [Export] PackedScene sceneApple;
    [Export] Timer gameTimer;

    Area2D head, body, apple;
    Array<Area2D> snake = [];
    RandomNumberGenerator rand = new RandomNumberGenerator();

    int colomnMax, lineMax;
    int headDirection;
    int cell = 64;
    bool onGame = false;
    bool keyPressUp, keyPressDown, keyPressLeft, keyPressRight;

    Vector2 screenSize;
    Vector2 vectorDirection = Vector2.Up;
    Vector2 headPosition;

    public override void _Ready()
    {
        screenSize = GetWindow().Size;
        colomnMax = (int)screenSize.Y / cell;
        lineMax = (int)screenSize.X / cell;
        gameTimer.Timeout += HeadMove;
        gameTimer.Timeout += BodyMove;

        StartGame();
    }

    public override void _Process(double pDelta)
    {
        Direction();
    }

    void StartGame()
    {
        snake = [head = (Area2D)sceneHead.Instantiate(), body = (Area2D)sceneBody.Instantiate()];
        for (int i = 0; i < 2; i++) AddChild(snake[i]);

        head.Position = new Vector2(lineMax / 2 * cell + 32, colomnMax / 2 * cell + 32);

        apple = (Area2D)sceneApple.Instantiate();
        apple.AreaEntered += EatApple;
        apple.Position = new Vector2(cell * rand.RandiRange(1, lineMax) + 32, cell * rand.RandiRange(1, colomnMax) + 32);
        AddChild(apple);
    }

    void CheckInput()
    {
        keyPressUp = Input.IsKeyPressed(Key.Z);
        keyPressDown = Input.IsKeyPressed(Key.S);
        keyPressRight = Input.IsKeyPressed(Key.D);
        keyPressLeft = Input.IsKeyPressed(Key.Q);
    }

    void Direction()
    {
        CheckInput();

        if (keyPressUp && vectorDirection != Vector2.Down)
        { 
            vectorDirection = Vector2.Up;
            headDirection = 0;
        }
        if (keyPressDown && vectorDirection != Vector2.Up)
        {
            vectorDirection = Vector2.Down;
            headDirection = 180;
        }
        if (keyPressLeft && vectorDirection != Vector2.Right)
        {
            vectorDirection = Vector2.Left;
            headDirection = -90;
        }
        if (keyPressRight && vectorDirection != Vector2.Left)
        {
            vectorDirection = Vector2.Right;
            headDirection = 90;
        }
    }

    void HeadMove()
    {
        headPosition = head.Position;

        if (head.Position.X <= screenSize.X
            && head.Position.Y <= screenSize.Y
            && head.Position.X >= 0 
            && head.Position.Y >= 0)
        {
            head.Position += vectorDirection * cell;
            head.RotationDegrees = headDirection;
        }
    }

    void BodyMove()
    {
        for (int i = 0; i < snake.Count -1; i++)
        {
            if (i == 0)
            {
                snake[i + 1].Position = headPosition;
                snake[i + 1].RotationDegrees = headDirection;
            }
            
            if (i > 0)
            {
                snake[i + 1].Position = snake[i].Position;
                snake[i + 1].RotationDegrees = snake[i].RotationDegrees;
            }
        }
    }

    void EatApple(Area2D pArea)
    {
        apple.Position = new Vector2(cell * rand.RandiRange(1, lineMax - 1) + 32, cell * rand.RandiRange(1, colomnMax - 1) + 32);

        body = (Area2D)sceneBody.Instantiate();
        snake.Add(body);
        AddChild(body);
    }
}
