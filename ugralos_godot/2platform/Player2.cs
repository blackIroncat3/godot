using Godot;
using System;

public class Player2 : Node2D
{   
    int gyorsasag;
    Vector2 irany;
    AnimatedSprite alak;
    float GRAVITY = 500;
    float WALK_FORCE = 600;
    float WALK_MIN_SPEED = 100;
    float WALK_MAX_SPEED = 200;
    float STOP_FORCE = 4000;
    float setaspeed = 400;
    KinematicBody2D player;
    float spring = -300;
    float jump_force = 250;
    float JUMP_MAX_TIME = 0.1f;
    
    private Vector2 velocity = new Vector2(0, 0);
    private float onAirTime = 100;
    private bool jumping = false;
    private bool previousJumpPressed = false;
    private float health = 10;
    [Export]
    PackedScene robbanas;
    Timer sebzes2;
    float jumpcount = 0;
    bool kepesseg = true;
    float OPP_GRAVITY = 500;
    float WALL_CLIMB = 500;
    private bool entered = false;
    float WALL_UGRAS = 0;

    public override void _Ready()
    {
        gyorsasag = 200; // pixel/s
        irany = new Vector2(0, 0);
        alak = (AnimatedSprite)GetNode("KinematicBody2D/AnimatedSprite");
        player = (KinematicBody2D)GetNode("KinematicBody2D");
        sebzes2 = (Timer)GetNode("../sebzes2");
        
    }

    public void Kill()
    {
        var particle = (Particles2D)robbanas.Instance();
        particle.Position = player.GlobalPosition;
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
                GD.Print("halál");
                GetTree().ChangeScene("res://Lose.tscn");
                //QueueFree();

            }
        }
    }

    public override void _Process(float idokulonbseg)
    {
    
        Vector2 force = new Vector2(0, GRAVITY);
        bool left = Input.IsActionPressed("left");
        bool right = Input.IsActionPressed("right");
        bool jump = Input.IsActionPressed("jump");
        bool stop = true;
        
        

        //GD.Print(player.Position);
        
        
        //GD.Print(player.Position);        
        if (left&&!right)
        {
            if (velocity.x > -WALK_MAX_SPEED)
            {
                force.x -= WALK_FORCE;
                stop = false;
                alak.Animation = "player_mozgas";
                alak.FlipH = true;
                
            }
        }
        if (!left&&right)
        {
            if (velocity.x < WALK_MAX_SPEED)
            {
                force.x += WALK_FORCE;
                stop = false;
                alak.Animation = "player_mozgas";
                alak.FlipH = false;
            }
        }

        /*
        if (right || left && jumping)
        {
            GD.Print(left + " and " + right);
        }
        */

        
            
        


        if (velocity.x != 0 && velocity.y != 0 || velocity.x == 0 && velocity.y != 0)
        {
            alak.Animation = "player_jump";
        }
        
        

        if (stop && !jump)
        {
            float velocitySign = Mathf.Sign(velocity.x);
            float velocityLength = Mathf.Abs(velocity.x);
            velocityLength -= STOP_FORCE * idokulonbseg;

            if (velocityLength < 0 && velocity.y == 0)
            {
                velocityLength = 0;
                alak.Animation = "player_idle";
            }
            velocity.x = velocityLength * velocitySign;
        }

        
    
        velocity += force * idokulonbseg;
        velocity = player.MoveAndSlide(velocity, new Vector2(0, -1));



        if(!jumping && !is_near_wall() && !player.IsOnFloor())  
        {
            WALL_UGRAS = 0;
            //GD.Print("test");
        }

        if (Input.IsActionPressed("jump") && !player.IsOnFloor() && is_near_wall() && WALL_UGRAS < 2 && entered) //Falramászás
        {
            if (!jumping && jump)   
            {
                WALL_UGRAS +=1;
                velocity.y =- jump_force;
                jumping = true;
                alak.Animation = "player_jump";
            }


        }
            
        //GD.Print(player.IsOnFloor());


        if (player.IsOnFloor())  //Föld
        {
            onAirTime = 0;
            jumpcount = 0;
            
        }

        if (jumping && velocity.y > 0 && velocity.y < 300)  
        {
            jumping = false;
            //GD.Print(jumping);
        }

        if (onAirTime < JUMP_MAX_TIME && jump && !jumping)
        {
            if(!left&&!right){
                velocity.x = 0;
            }//fixed 1. bug --> readme.md
            //GD.Print("ugrottam egyet");
            velocity.y = -jump_force;
            jumping = true;
            alak.Animation = "player_jump";
        }
        
        if (!jumping && jump && jumpcount < 1 && kepesseg == true)   //14 Simon 4 Földesi Képesség
        {
            //GD.Print(jumpcount);
            jumpcount += 1;
            velocity.y =- jump_force;
            jumping = true;
            alak.Animation = "player_jump";
        }
        
        onAirTime += idokulonbseg;

        //GD.Print(is_near_wall());

        if (marha == true)
        {
            alak.Animation = "player_celebrate";
        }

        
    }
    
    public void _on_bug_body_entered2(KinematicBody2D bug)
    {   
        if (bug.GetParent() is Player2 player)
        {
            velocity.x -= 800;
        }
    }

    public void _on_wall_body_entered2(KinematicBody2D wall)
    {
        if (wall.GetParent() is Player2 player)
        {
            entered = true;
        }
    }


    public void _on_wall_body_exited2(KinematicBody2D wall)
    {
        if (wall.GetParent() is Player2 player)
        {
            entered = false;
        }
    }

    bool is_near_wall()
    {
        var right = GetNodeOrNull<RayCast2D>("KinematicBody2D/right");
        var left = GetNodeOrNull<RayCast2D>("KinematicBody2D/left");
        return (right == null ? false : right.IsColliding()) || (left== null ? false : left.IsColliding());
    }

    void move_and_fall()
    {
        velocity.y = velocity.y + GRAVITY;
        velocity = player.MoveAndSlide(velocity, Vector2.Up);
    }
    
    public void _on_damage_body_shape_entered(KinematicBody2D sajt)
    {
        GD.Print("tüske szúr");
    }


    public void _on_spring_body_entered(Area2D kecske)
    {
        if (kecske.GetParent() is Player2 player)
        {
            velocity.y = spring;
        }
    }
    bool boci = false;

    
    public void _on_cel_body_entered(KinematicBody2D cel)
    {
        
        if (cel.GetParent() is Player2 player && !boci)
        {
            //GD.Print("elérted a célt");
            boci = true;
            kepesseg = true;
            GetTree().ChangeScene("res://NextMain.tscn");
        }
    }
    

    bool marha = false;
    public void _on_END_body_entered(KinematicBody2D END)
    {
        
        if (END.GetParent() is Player2 player && !marha)
        {
            GD.Print("Kivitted a játékot!");
            GetTree().CreateTimer(5).Connect("timeout", this, nameof(stop_celebrating));
            alak.Animation = "player_celebrate";
            marha = true;
        }
    }
    void stop_celebrating()
    {
        GD.Print("letelt az 5sec");
        //alak.Animation = "player_idle";
        marha = false;
        GetTree().ChangeScene("res://Win.tscn");
    }

}
