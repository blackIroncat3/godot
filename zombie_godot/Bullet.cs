using Godot;
using System;

public class Bullet : Node2D
{
    public float Range = 500;
    private float megtett_ut = 0;
    
    public override void _Ready()
    {
        var area = GetNode<Area2D>("Area2D");
        area.Connect("area_entered",this,"OnCollision");
        area.Connect("body_entered",this,"OnCollision");
    }

    public override void _Process(float delta)
    {
        float speed = 200;
        float sebesseg = speed * delta;
        Position += Transform.x.Normalized() * sebesseg;
        megtett_ut += sebesseg;
        if (megtett_ut > Range)
        {
            QueueFree();
        }
    }
    private void OnCollision(Node with)
    {
        if (with.GetParent() is Enemy enemy) //SIMON DÁVID MUNKÁJA ÉS ([{FELFÖLDI ÁRON A MEGMENTŐ}])
        {
            //robot.Health = robot.Health - 2;
            enemy.Health = enemy.Health - 2;  
            //GD.Print("Bullet collided!");
            QueueFree();
        }
        
    }
}
