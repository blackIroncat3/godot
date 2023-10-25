using Godot;
using System;

public class spring : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override async void _PhysicsProcess(float delta)
    {
        var bodies = GetOverlappingBodies();
        var player = ((AnimationPlayer)GetNode("AnimationPlayer"));
        foreach (Node2D body in bodies)
        {
            if (body.Name == "Player")
            {
                player.Play("active");
                await ToSignal(player, "animation_finished");
                player.Play("idle");
            }
            else
            {
                player.Play("idle");
            }
        }
    }
}
