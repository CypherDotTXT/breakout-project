using Godot;
using System;

public partial class Brick : StaticBody2D
{
    [Export]
    public int HitPoints = 1;
    [Signal]
    public delegate void BrickDestroyedEventHandler(Brick brick);

    public override void _Ready()
    {

    }
    public void OnHit()
    {
        HitPoints--;
        if (HitPoints <= 0)
        {
            EmitSignal(SignalName.BrickDestroyed, this);
            QueueFree();
        }
    }
}
