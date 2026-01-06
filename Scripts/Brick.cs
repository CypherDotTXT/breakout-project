using Godot;
using System;

public partial class Brick : StaticBody2D
{
    [Export]
    public int HitPoints = 1;

    public void Hit()
    {
        HitPoints--;
        if (HitPoints <= 0)
            QueueFree();
    }
}