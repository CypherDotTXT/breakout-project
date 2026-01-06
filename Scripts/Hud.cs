using Godot;
using System;

public partial class Hud : CanvasLayer
{
    private Label scoreLabel;
    private Label livesLabel;
    private Button startButton;
    private Button quitButton;
    private ColorRect menuBackdrop;
    [Signal]
    public delegate void StartButtonPressedEventHandler();
    [Signal]
    private delegate void QuitButtonPressedEventHandler();

    public override void _Ready()
    {
        scoreLabel = GetNode<Label>("ScoreLabel");
        livesLabel = GetNode<Label>("LivesLabel");
        startButton = GetNode<Button>("StartButton");
        quitButton = GetNode<Button>("QuitButton");
        menuBackdrop = GetNode<ColorRect>("menuBackdrop");
        startButton.Pressed += OnStartButtonPressed;
        quitButton.Pressed += OnQuitButtonPressed;
    }

    private void OnStartButtonPressed()
    {
        EmitSignal(SignalName.StartButtonPressed);
        SetMenuVisibility(false);
    }

    private void OnQuitButtonPressed()
    {
        EmitSignal(SignalName.QuitButtonPressed);
        GetTree().Quit();
    }

    public void SetMenuVisibility(bool isVisible)
    {
        menuBackdrop.Visible = isVisible;
        startButton.Visible = isVisible;
        quitButton.Visible = isVisible;
    }

    public void SetScore(int score)
    {
        scoreLabel.Text = $"Score: {score}";
    }

    public void SetLives(int lives)
    {
        livesLabel.Text = $"Lives: {lives}";
    }
}
