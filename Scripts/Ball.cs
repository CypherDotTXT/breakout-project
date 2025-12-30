using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Ball : CharacterBody2D
{
    [Export]
    public int Speed = 600;
    private Vector2 direction;
    public override void _Ready()
    {
        //direction = Vector2.Zero;
        StartMoving();
    }

    public void StartMoving()
    {
        float startAngleDegrees = (float)GD.RandRange(260.0, 340.0);
        float angleRadians = Mathf.DegToRad(startAngleDegrees);
        direction = new Vector2(
            (float)Math.Cos(angleRadians),
            (float)Math.Sin(angleRadians)
        );
        if (GD.Randf() > 0.5f) direction.X *= -1;

        Velocity = direction * Speed;
    }
    public override void _PhysicsProcess(double delta)
    {
        // Called every frame. 'delta' is the elapsed time since the previous frame.
        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

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
                HandlePlayerCollision(collision, collider);
                GD.Print($"Ball collided with: {collider.Name}");
            }
            else
            {
                HandleBallOutOfBounds(collider);
            }
        }
    }

    private void HandlePlayerCollision(KinematicCollision2D collision, Node Player)
    {
        var playerBody = Player as StaticBody2D;
        if (playerBody == null) return;

        var shape = playerBody.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;

        if (shape == null) return;

        float playerX = playerBody.GlobalPosition.X;
        float ballX = GlobalPosition.X;

        float halfWidth = shape.Size.X / 2f;
        float hitPosition = (ballX - playerX) / halfWidth;
        hitPosition = Mathf.Clamp(hitPosition, -1f, 1f);

        float maxAngle = Mathf.DegToRad(75);
        float bounceAngle = hitPosition * maxAngle;

        Vector2 normal = collision.GetNormal();

        Vector2 newDirection = new Vector2(Mathf.Sin(bounceAngle), -Mathf.Cos(bounceAngle)).Normalized();

        // 🔒 GUARANTEE separation
        if (newDirection.Dot(normal) >= 0)
        {
            newDirection = newDirection.Bounce(normal);
        }

        Velocity = newDirection.Normalized() * Speed;
    }
    private void HandleBallOutOfBounds(Node BottomBoundary)
    {
        var ballLoss = BottomBoundary as StaticBody2D;

        GD.Print($"Ball went out of bounds");
        // Implement additional logic for when the ball goes out of bounds
    }
}
