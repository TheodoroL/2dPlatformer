using System;
using Godot;

public partial class Player : CharacterBody2D
{
    private int _speed = 400;
    private int _jumpVelocity = -850;

    private bool _isAlive = true;
    private bool _isCanMove = true;

    public bool IsAlive => _isAlive;
    public bool IsCanMove
    {
        get => _isCanMove;
        set
        {
            _isCanMove = value;
        }
    }

    private AnimatedSprite2D _animatedSprite;

    private AudioStreamPlayer2D _soundJump;
    private AudioStreamPlayer2D _soundHit;

    private float aceleretion = 2000f;
    private float decceleration = 2000f;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _soundJump = GetNode<AudioStreamPlayer2D>("JumpSound");
        _soundHit = GetNode<AudioStreamPlayer2D>("hitSound");
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (!IsAlive)
            return;

        if (velocity.X > 1 || velocity.X < -1)
            _animatedSprite.Play("run");

        else
            _animatedSprite.Play("idle");

        //se ele o player não estiver no chão, ele vai aplicar a gravidade e tocar a animação de pulo
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
            _animatedSprite.Play("jump");

        }
        if (IsCanMove)
        {
            if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
            {
                velocity.Y = _jumpVelocity;
                _soundJump.Play();

            }

            //pega o input do jogador, se ele estiver pressionando para a direita ou para a esquerda, ele vai multiplicar o valor do input pela velocidade do player
            var direction = Input.GetAxis("move_left", "move_right");

            //se o jogador estiver pressionando para a direita ou para a esquerda, ele vai aplicar a velocidade do player na direção do input, caso contrário, ele vai diminuir a velocidade do player até chegar em 0
            if (direction != 0)
                velocity.X = Mathf.MoveToward(velocity.X, direction * _speed, aceleretion * (float)delta);
            else
                velocity.X = Mathf.MoveToward(velocity.X, 0, decceleration * (float)delta);

            Velocity = velocity;

            MoveAndSlide();

            if (direction > 0)
                _animatedSprite.FlipH = false;

            else if (direction < 0)
                _animatedSprite.FlipH = true;
        }

    }


    public void Die()
    {
        _animatedSprite.Play("hit");
        _soundHit.Play();
        _isAlive = false;
    }
}
