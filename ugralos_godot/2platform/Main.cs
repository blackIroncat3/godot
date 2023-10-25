using Godot;
using System;

public class Main : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Timer sebzes;
    private bool dmg = false;

    public override void _Ready()
    {
        sebzes = (Timer)GetNode("./sebzes");
    }

    public override void _Process(float delta)
    {
        
    }

    void _on_sebzes_timeout(){
        if(dmg){
            var jatekos = GetNodeOrNull<Player>("./Player");  //SIMON DÁVID timer
            if(jatekos != null)
            {
                jatekos.Health = jatekos.Health - 2; //todo: nincs ide megírva a sebzés időnként... 
                //jatekos.Bar = -20;
                //GD.Print("cuki dávid");
            }
        }
    }
    
    public void _on_damage_body_entered(Node2D sajt)
    {
       if(sajt.IsInGroup("ok")){
            GD.Print("-1 HP");
            dmg = true;
            var jatekos = GetNodeOrNull<Player>("./Player");  //SIMON DÁVID ha a csapdába belelépsz
            if(jatekos != null)
            {
                sebzes.Start(1);
                jatekos.Health = jatekos.Health - 2; //todo: nincs ide megírva a sebzés időnként... 
                //jatekos.Bar = -20;
                //GD.Print("cuki dávid");
            }
       } 
        
    }

    public void _on_damage_body_exited(Node2D sajt2){
        dmg = false;
        sebzes.Stop();
    }
}
