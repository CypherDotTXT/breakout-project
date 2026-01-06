using Godot;
using System;

public partial class BlockSpawner : Node2D
{
    [Export]
    public PackedScene BrickScene;
    [Export]
    public int Rows = 8;
    [Export]
    public int Columns = 12;

    [Export]
    public Vector2 BrickSize = new Vector2(50, 24);

    public override void _Ready()
    {
        SpawnBricks();
    }

    private void SpawnBricks()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                var brickInstance = BrickScene.Instantiate<Brick>();
                brickInstance.Position = new Vector2(col * BrickSize.X, row * BrickSize.Y);
                AddChild(brickInstance);
            }
        }
    }
}
