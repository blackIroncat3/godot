using Godot;
using System;

public class Robot : Node2D
{
    [Export]
    PackedScene robbanas;
    int gyorsasag;
    Vector2 irany;
    AnimatedSprite alak;
    PackedScene bulletScene;
    private float health = 10;
    private float pizza = 100;
    ProgressBar sajt;

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
    
    public float Bar
    {
        get
        {
            return pizza;
        }
        set
        {
            pizza = value;
            sajt.Value += value;
        }
    }

    
    public override async void  _Ready()
    {
        gyorsasag = 200; // pixel/s
        irany = new Vector2(0, 0);
        alak = (AnimatedSprite)GetNode("KinematicBody2D/AnimatedSprite");
        bulletScene = GD.Load<PackedScene>("res://Bullet.tscn"); 
        sajt = (ProgressBar)GetNode("ProgressBar");
        
        await ToSignal(GetTree().CreateTimer(1), "timeout");
    }

    

    public override void _Process(float idokulonbseg)
    {
        float speed = 150;
        float sebesseg = speed * idokulonbseg;
        Vector2 mozgas = new Vector2(0,0);

        
        if (Input.IsActionPressed("forward"))
        {
            mozgas.y = -1;
        }
        if (Input.IsActionPressed("down"))
        {
            mozgas.y = 1;
        }
        if (Input.IsActionPressed("left"))
        {
            mozgas.x = -1;
            alak.FlipH = true;
        }
        if (Input.IsActionPressed("right"))
        {
            mozgas.x = 1;
            alak.FlipH = false;
        }
        if (mozgas.x == 0 && mozgas.y == 0)
        {
            alak.Animation = "robot_idle";
        }
        else
        {
            alak.Animation = "robot_mozgas_x"; 

        }   
        //Position += mozgas.Normalized() * idokulonbseg * sebesseg;
        //Rotation = mozgas.Angle();

        Position += mozgas.Normalized() * sebesseg;
        RotationDegrees = 0;

        Position = new Vector2(Mathf.Clamp(Position.x, 0, 1024), Mathf.Clamp(Position.y, 0, 600));
    
        
        //Rotation = (GetGlobalMousePosition() - GlobalPosition).Angle();

        
        //LookAt(GetGlobalMousePosition());
        //var rotation = GetGlobalMousePosition().AngleToPoint(Position);

        
    }



    public override void _UnhandledInput(InputEvent esemeny)
    {
        if(esemeny is InputEventMouseButton egergomb)
        {
            if(egergomb.ButtonIndex == 1 && egergomb.Pressed)
            {
                LookAt(GetGlobalMousePosition());
                //GD.Print(egergomb.Position);
                Bullet bullet = (Bullet)bulletScene.Instance();
                bullet.Position = Position;
                bullet.Rotation = Rotation;
                GetParent().AddChild(bullet);
                GetTree().SetInputAsHandled();
                //GD.Print("Kilőve");
            }
        }
    }

        
        
    
}
