using Godot;
using System;
using System.Threading.Tasks;

public partial class Main : Node2D
{
    private int _level = 1;
    private int _score = 0;

    private Label _scoreLabel;
    private Node2D _levelRoot;

    private ColorRect _fade;
    public override async void _Ready()
    {
        _scoreLabel = GetNode<Label>("HUD/ScorePanel/ScoreLabel");
        _levelRoot = GetNode<Node2D>("LevelRoot");
        _fade = GetNode<ColorRect>("HUD/Fade");

        var color = _fade.Modulate;
        color.A = 1f;
        _fade.Modulate = color;

        await LoadLevel(_level, true);
    }

    private async Task LoadLevel(int level, bool firstLoad = false, bool scoreReset = false)
    {

        if (!firstLoad)
        {
            await Fadein(1f);
        }

        if (scoreReset)
        {
            _score = 0;
            _scoreLabel.Text = $"SCORE: {_score}";
        }

        // Remove level anterior (seguro)
        if (IsInstanceValid(_levelRoot))
        {
            _levelRoot.QueueFree();
        }
        var levelPath = $"res://src/scenes/level{level}.tscn";


        var packedScene = GD.Load<PackedScene>(levelPath);
        if (packedScene == null)
        {
            GD.PrintErr($"Erro ao carregar cena: {levelPath}");
            return;
        }

        var levelScene = packedScene.Instantiate<Node2D>();
        levelScene.Name = "LevelRoot";

        AddChild(levelScene);
        _levelRoot = levelScene;

        SetupLevel(_levelRoot);

        await Fadein(0f);
    }

    private void SetupLevel(Node2D level)
    {
        if (level == null)
            return;

        // ===== EXIT =====
        var exit = level.GetNodeOrNull<Area2D>("Exit");

        if (exit != null)
        {
            exit.BodyEntered += OnExitBodyEntered;
        }

        // ===== ENEMIES =====
        var enemies = level.GetNodeOrNull<Node2D>("enemies");

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

        // ===== APPLES =====
        var apples = level.GetNodeOrNull<Node2D>("apples");

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

        if (player is Player playerNode)
        {
            playerNode.Die();
            _ = LoadLevel(_level, scoreReset: true);

        }
    }

    private void OnAppleCollected()
    {
        _score++;
        _scoreLabel.Text = $"SCORE: {_score}";
    }

    private void OnExitBodyEntered(Node body)
    {
        if (body is Player player && player.IsAlive)
        {
            GD.Print($"Colidiu com {body.Name}");

            _level++;
            player.IsCanMove = false;

            // Carrega próximo nível automaticamente
            _ = LoadLevel(_level);
        }
    }

    private async Task Fadein(float toAlpha)
    {
        // Implementação de fade-in usando Tween para suavizar a transição entre os níveis
        var tween = CreateTween();
        tween.TweenProperty(_fade, "modulate:a", toAlpha, 1.5f);

        await ToSignal(tween, Tween.SignalName.Finished);
    }
}