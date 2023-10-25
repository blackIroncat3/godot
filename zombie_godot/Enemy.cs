using Godot;
using System;

public class Enemy : Node2D
{
    [Export]
    PackedScene robbanas;
    private Timer timer;
    private float health = 10;
    AnimatedSprite alak;
    
    public void Kill()
    {
        var particle = (Particles2D)robbanas.Instance();
        particle.Position = Position;
        particle.Rotation = Rotation;
        particle.Emitting = true;
        GetTree().CurrentScene.AddChild(particle);
        QueueFree();

    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if(health <= 0)
            {
                Kill();
                //QueueFree();
            }
        }
    }
    public override void _Ready()
    {
        var area = GetNode<Area2D>("Area2D");
        area.Connect("area_entered", this, nameof(OnCollision));
        area.Connect("area_exited", this, nameof(OnCollisionNoMore));

        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this,nameof(OnTimerTimeout));

        alak = (AnimatedSprite)GetNode("AnimatedSprite");
    }
    private bool attack = false;
    public override void _Process(float delta)
    {
        
        var robot = GetNodeOrNull<Robot>("../Robot");
        if (robot != null)
        {
            float speed = 50;
            float sebesseg = speed * delta;
            Vector2 irany = (robot.Position - Position).Normalized();
            Position += irany * sebesseg;
            //GD.Print(Position);            
            
            //GD.Print(Position);
            if (!attack)
            {
                if(robot.Position < this.Position)
                {
                    //GD.Print("Lefutok1");
                    alak.Animation = "enemy_mozgas_x";
                    alak.FlipH = true;
                }
                else
                {
                    //GD.Print("Lefutok2");
                    alak.Animation = "enemy_mozgas_x";
                    alak.FlipH = false;
                    
                }
            } 
        }
    }

    
    private void OnCollision(Area2D with)
    {
        //GD.Print("Enemy ütközik");
        if (with.GetParent() is Robot robot)
        {
            //float speed = 0;
            //Vector2 irany = (robot.Position - Position).Normalized();
            //Position += irany * speed;
            //Position = robot.Position;
            timer.Start(1);
            attack = true;
            alak.Animation = "enemy_attack";
            
        }
        
    }

    private void OnCollisionNoMore(Area2D with)
    {
        if (with.GetParent() is Robot robot)
        {
            //GD.Print("kijöttem az ütközési zónából");
            timer.Stop();
            attack = false;
        }
    }

    private void OnTimerTimeout()
    {
        var robot = GetNodeOrNull<Robot>("../Robot");
        if(robot != null)
        {
            robot.Health = robot.Health - 2; 
            robot.Bar = -20;
            //GD.Print("cuki dávid");
        }
    }
    
}
 