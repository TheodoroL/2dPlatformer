using Godot;
using System;

public partial class Main : Node2D
{

    private int _score = 0;
    private Label _scoreLabel;

    public override void _Ready()
    {
        _scoreLabel = GetNode<Label>("HUD/ScorePanel/ScoreLabel");
        SetupLevl();
    }

    public void SetupLevl()
    {
        var enemies = GetNode<Node2D>("LevelRoot/enemies");

        if (enemies != null)
        {

            foreach (var enemy in enemies.GetChildren())
            {
                if (enemy is Snail snail)
                {
                    snail.PlayerDead += OnPlayerDead;
                }
            }

        }
        var apples = GetNode<Node2D>("LevelRoot/apples");

        if (apples != null)
        {
            foreach (var apple in apples.GetChildren())
            {
                if (apple is Apple appleNode)
                {
                    appleNode.Collected += OnAppleCollected;
                }
            }
        }
    }

    private void OnPlayerDead(Node2D player)
    {
        GD.Print("Player morreu");

        if (player is Player playerNode)
        {
            playerNode.Die();
        }
    }

    private void OnAppleCollected()
    {
        _score++;
        _scoreLabel.Text = $"SCORE: {_score}";
    }
}
