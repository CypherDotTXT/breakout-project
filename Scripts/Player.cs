using Godot;
using System;

public partial class Player : StaticBody2D
{
    [Export]
    public int speed { get; set; } = 400;

    public Vector2 Screensize;

    public override void _Ready()
    {
        Screensize = GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_left"))
            velocity.X -= 1;

        if (Input.IsActionPressed("move_right"))
            velocity.X += 1;
        Position += velocity * speed * (float)delta;
        Position = new Vector2(
            Mathf.Clamp(Position.X, 50, Screensize.X - 50),
            Mathf.Clamp(Position.Y, 50, Screensize.Y - 50)
        );
    }
}
