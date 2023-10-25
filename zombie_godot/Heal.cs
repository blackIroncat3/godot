using Godot;
using System;

public class Heal : Node2D
{
    ProgressBar sajt;
    private Timer timer;
    public void _on_Area2D_area_entered(KinematicBody2D entered)
    {
        if (entered.GetParent() is Robot robot) 
        {
            if (robot.Health < 10)
            {
                //GD.Print("életerő megszerezve");
                robot.Health += 2;
                robot.Bar = 20;
                QueueFree();
            }
            
            
        }
    }

    public override void _Ready()
    {
        var robot = GetNodeOrNull<Robot>("../Robot");
        //sajt = (ProgressBar)GetNode("../Robot/ProgressBar");
        //sajt = GetNode<ProgressBar>("ProgressBar");
        //timer.Connect("timeout", this,nameof(OnTimerTimeout));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
