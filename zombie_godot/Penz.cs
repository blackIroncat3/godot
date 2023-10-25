using Godot;
using System;

public class Penz : Node2D
{
    
    [Signal]
    delegate void gyujt();    
    RandomNumberGenerator pos;

    public override void _Ready()
    {
        pos = new RandomNumberGenerator();
        pos.Randomize();
    }

    public void _on_Area2D_body_entered(KinematicBody2D entered)
    {
        if (entered.GetParent() is Robot robot) 
        {
            //Position = new Vector2(pos.RandiRange(52, 975), pos.RandiRange(44, 44));
            //GD.Print("begyűjtve");
            EmitSignal("gyujt");
            QueueFree();
            
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
