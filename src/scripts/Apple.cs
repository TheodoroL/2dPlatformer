using Godot;

public partial class Apple : Area2D
{

    [Signal]
    public delegate void CollectedEventHandler();
    private AnimatedSprite2D _animatedSprite;
    private AudioStreamPlayer2D _soundCollected;

    private CollisionShape2D _collisionShape;


    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _soundCollected = GetNode<AudioStreamPlayer2D>("SoundCollected");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    // desabilita a colisão para evitar que o player colete a maçã mais de uma vez, e depois de um tempo, remove a maçã da cena
    public void OnBodyEntered(Node2D body)
    {
        _animatedSprite.Play("collected");
        _soundCollected.Play();
        EmitSignal(SignalName.Collected);

        CallDeferred(nameof(OnDisableCollision));
    }

    public void OnDisableCollision()
    {
        _collisionShape.Disabled = true;
    }

    public void OnAnimatedSprite2dAnimationLooped()
    {
        if (_animatedSprite.Animation == "collected")
            QueueFree();
    }
}
