using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Ball : CharacterBody2D
{
    [Export]
    public int Speed = 500;
    private Vector2 direction;
    public override void _Ready()
    {
        direction = Vector2.Zero;
        StartMoving();
    }

    public void StartMoving()
    {
        float startAngleDegrees = (float)GD.RandRange(15.0, 165.0);
        direction = new Vector2(
            (float)Math.Cos(startAngleDegrees * Math.PI / 180),
            (float)Math.Sin(startAngleDegrees * Math.PI / 180)
        );
    }
    public override void _PhysicsProcess(double delta)
    {
        // Called every frame. 'delta' is the elapsed time since the previous frame.
        Vector2 velocity = Velocity * Speed;
        KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta);

        if (collision == null)
        {
            // No collision occurred
            return;
        }

        if (collision != null)
        {
            Node collider = collision.GetCollider() as Node;
            string colliderName = collider.Name;

            if (colliderName == "Left Boundary" || colliderName == "Right Boundary" || colliderName == "Top Boundary")
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
            }
            else if (colliderName == "Player")
            {
                HandlePlayerCollision(collider);
            }
            else
            {
                HandleBallOutOfBounds(collider);
            }
        }
    }

    private void HandlePlayerCollision(Node Player)
    {
        var playerBody = Player as StaticBody2D;
        if (playerBody == null) return;

        var shape = playerBody.GetNode<CollisionShape2D>("CollisionShape2D");
        if (shape == null) return;


        GD.Print($"Ball collided with Player: {playerBody.Name}");
        // Implement additional logic for player collision
    }
    private void HandleBallOutOfBounds(Node BottomBoundary)
    {
        var ballLoss = BottomBoundary as StaticBody2D;

        GD.Print($"Ball went out of bounds");
        // Implement additional logic for when the ball goes out of bounds
    }
}
