using Godot;
using System;

public partial class Main : Node2D
{
    public override void _Ready()
    {
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
    }

    private void OnPlayerDead(Node2D player)
    {
        GD.Print("Player morreu");

        if (player is Player playerNode)
        {
            playerNode.Die();
        }
    }
}
