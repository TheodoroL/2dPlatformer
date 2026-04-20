using Godot;
using System;

public partial class Snail : Area2D
{
    [Signal]
    public delegate void PlayerDeadEventHandler(Node2D player);
    private int _speed = 50;
    private int _direction = -1;

    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        var position = Position;
        position.X += _direction * _speed * (float)delta;
        Position = position;
    }

    public void OnTimerTimeout()
    {
        _direction *= -1;
        _animatedSprite.FlipH = !_animatedSprite.FlipH;
    }

    public void OnBodyEntered(Node2D body)
    {

        GD.Print($"colidiu com {body.Name}");

        if (body.Name == "Player" && body is Player player && player.IsAlive)
        {
            EmitSignal(SignalName.PlayerDead, body);
        }
    }
}
