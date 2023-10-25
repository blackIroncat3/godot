using Godot;
using System;

public class Endscreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Restart_pressed()
    {
        GetTree().ChangeScene("res://Palya.tscn");
    }

    public void _on_Quit_pressed()
    {
        GetTree().ChangeScene("res://Menu.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
