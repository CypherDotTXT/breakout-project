using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Ball : CharacterBody2D
{
    [Export]
    public int Speed = 600;
    private Vector2 direction;
    public BallState State { get; private set; } = BallState.AttachedToPlayer;
    [Signal]
    public delegate void OnBallLossEventHandler();
    [Signal]
    public delegate void BrickHitEventHandler();

    public override void _Ready()
    {
        direction = Vector2.Zero;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (State != BallState.Moving)
            return;

        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if (collision == null)
            return;

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
                HandlePlayerCollision(collider, collision);
            }
            else if (colliderName == "Bottom Boundary")
            {
                AttachToPlayer();
                EmitSignal(SignalName.OnBallLoss);
            }
            else if (colliderName == "Brick")
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
                EmitSignal(SignalName.BrickHit);
            }
        }
    }

    public void Launch()
    {
        if (State != BallState.AttachedToPlayer)
            return;

        float startAngleDegrees = (float)GD.RandRange(260.0, 340.0);
        float angleRadians = Mathf.DegToRad(startAngleDegrees);

        Vector2 direction = new(
            Mathf.Cos(angleRadians),
            Mathf.Sin(angleRadians)
        );

        Velocity = direction * Speed;
        State = BallState.Moving;
    }

    public void AttachToPlayer()
    {
        Velocity = Vector2.Zero;
        State = BallState.AttachedToPlayer;
    }

    private void HandlePlayerCollision(Node Player, KinematicCollision2D collision)
    {
        var playerBody = Player as StaticBody2D;
        if (playerBody == null) return;

        var shape = playerBody.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
        if (shape == null) return;

        float playerX = playerBody.GlobalPosition.X;
        float hitX = collision.GetPosition().X;

        float halfWidth = shape.Size.X / 2f;
        float hitPosition = (hitX - playerX) / halfWidth;
        hitPosition = Mathf.Clamp(hitPosition, -1f, 1f);

        float maxAngle = Mathf.DegToRad(60);
        float curvedHit = hitPosition * hitPosition * hitPosition;
        float bounceAngle = curvedHit * maxAngle;
        float directionY = -Mathf.Sign(Velocity.Y);

        Vector2 newDirection = new Vector2(Mathf.Sin(bounceAngle), directionY * Mathf.Cos(bounceAngle));

        Velocity = newDirection * Speed;
    }
}
