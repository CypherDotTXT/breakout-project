using Godot;
using System;
using System.Xml.Resolvers;

public partial class Main : Node2D
{
    private Ball ball;
    private StaticBody2D player;
    private int score = 0;
    private int lives = 3;

    public override void _Ready()
    {
        ball = GetNode<Ball>("Ball");
        player = GetNode<StaticBody2D>("Player");
        ball.OnBallLoss += OnBallLoss;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (ball.State == BallState.AttachedToPlayer)
        {
            ball.GlobalPosition = new Vector2(player.GlobalPosition.X, player.GlobalPosition.Y - 25);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("close_game"))
            GetTree().Quit();

        if (Input.IsActionPressed("ball_shot"))
        {
            ball.Launch();
        }
    }

    private void OnBallLoss()
    {
        lives--;
        if (lives <= 0)
        {
            GD.Print("Game Over");
            // Handle game over logic here
        }
        else
        {
            GD.Print("Ball lost! Lives remaining: " + lives);
            NextRound();
        }
    }

    private void NextRound()
    {
        ball.AttachToPlayer();
        player.Position = GetNode<Marker2D>("PlayerStart").Position;
    }
}
