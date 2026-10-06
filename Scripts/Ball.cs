using Godot;
using System;

public partial class Ball : Area2D
{
    void OnEntered(Area2D pArea)
    {
        Scale *= new Vector2(-1, 1);
    }
}
