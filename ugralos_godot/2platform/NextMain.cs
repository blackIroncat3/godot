using Godot;
using System;

public class NextMain : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Timer sebzes2;
    private bool dmg = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sebzes2 = (Timer)GetNode("./sebzes2");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    void _on_sebzes2_timeout(){
        if(dmg){
            var jatekos = GetNodeOrNull<Player2>("./Player2");  //SIMON DÁVID timer
            if(jatekos != null)
            {
                jatekos.Health = jatekos.Health - 2; //todo: nincs ide megírva a sebzés időnként... 
                //jatekos.Bar = -20;
                //GD.Print("cuki dávid");
            }
        }
    }
    public void _on_damage_body_entered2(Node2D sajt)
    {
        if(sajt.IsInGroup("ok")){
            dmg = true;
            GD.Print("-1 HP");
            var jatekos = GetNodeOrNull<Player2>("./Player2");  //SIMON DÁVID ha a csapdába belelépsz
            if(jatekos != null)
            {
                sebzes2.Start(1);
                jatekos.Health = jatekos.Health - 2; //todo: nincs ide megírva a sebzés időnként... 
                //jatekos.Bar = -20;
                //GD.Print("cuki dávid");
            }
        } 
    }
    public void _on_damage2_body_exited(Node2D sajt2){
        dmg = false;
        sebzes2.Stop();
    }
}
