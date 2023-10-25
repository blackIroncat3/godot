using Godot;
using System;

public class Palya : Node2D
{
    int pont;
	Label pontFelirat;
	Timer idozito;
	bool kezd;
	Label idoFelirat;
	RandomNumberGenerator pos;
	[Export]
	PackedScene PsPenz;
	[Export]
	PackedScene PsHp;
	[Export]
	PackedScene psZombie;
	[Signal]
    delegate void gyujt();
	private Timer HpTimer;    
	void _on_Idozito_timeout()
	{
		GetTree().ChangeScene("res://Endscreen.tscn");
	}
	public void _on_Back_pressed()
	{
		
		GetTree().ChangeScene("res://Menu.tscn");
	}
	public void AddPenz()
	{
		for (int i = 0; i < 29; i++)
		{
			Node2D penz = (Node2D)PsPenz.Instance();
			penz.Position = new Vector2(pos.RandiRange(30, 975), pos.RandiRange(30, 530));
			//GD.Print(penz.Position + " " + i);
			penz.Connect("gyujt", this, "_on_Penz_gyujt");
			AddChild(penz);
		}
	}
	public void AddHp()
	{
		var hp = GetNodeOrNull<Heal>("Heal");
        if(hp == null)
        {
			Node2D hp2 = (Node2D)PsHp.Instance();
			hp2.Position = new Vector2(pos.RandiRange(30, 975), pos.RandiRange(30, 530));
			//hp2.Connect("gyujt", this, "_on_Penz_gyujt");
			AddChild(hp2);
        }
	}
	void _on_Penz_gyujt()
	{
        //GD.Print("Sikeres begyűjtés!");
		pont++;
		pontFelirat.Text = pont.ToString();
		if (kezd)
		{
			idozito.Start();
			kezd = false;
		}
	}
	public override void _Ready()
	{
		pont = 0;
		pos = new RandomNumberGenerator();
        pos.Randomize();
		pontFelirat = (Label)GetNode("PontFelirat");
		idozito = (Timer)GetNode("Idozito");
		kezd = true;
		idoFelirat = (Label)GetNode("IdoFelirat");
		AddPenz();
		HpTimer = GetNode<Timer>("Hp_Timer");
        HpTimer.Connect("timeout", this,nameof(OnTimerTimeoutHp));
		HpTimer.Start(15);
		AddZombie();
	}
	public void OnTimerTimeoutHp()
	{
		AddHp();
	}
	/*
	* enemy1 --> (938, 71)
	* enemy2 --> (913, 521)
	* enemy3 --> (75, 531)
	* enemy4 --> (83, 71)
	* enemy5 --> (443, 244)
	*/
  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		idoFelirat.Text = $"{idozito.TimeLeft:N0}";
		if(GetNodeOrNull<Enemy>("Enemy") == null && GetNodeOrNull<Enemy>("Enemy1") == null && GetNodeOrNull<Enemy>("Enemy2") == null && GetNodeOrNull<Enemy>("Enemy3") == null && GetNodeOrNull<Enemy>("Enemy4") == null){
			AddZombie5();

		}

		if (GetNodeOrNull<Robot>("Robot") == null )
		{
			GetTree().ChangeScene("res://Endscreen.tscn");
		}
	} 
	public void AddZombie()
	{
		int i = 1;
		Vector2[] positions = {new Vector2(913, 521),new Vector2(75, 531),new Vector2(83, 71),new Vector2(443, 244)};
		foreach (var pos_z in positions)
		{
			Node2D zombie = (Node2D)psZombie.Instance();
			zombie.Position = pos_z;
			zombie.Name = $"Enemy{i}";
			
			AddChild(zombie);
			i++;
		}
	}
	
	public void AddZombie5()
	{
		int i = 0;
		Vector2[] positions = {new Vector2(938, 71), new Vector2(913, 521),new Vector2(75, 531),new Vector2(83, 71),new Vector2(443, 244)};
		foreach (var pos_z in positions)
		{
			Node2D zombie = (Node2D)psZombie.Instance();
			zombie.Position = pos_z;
			if(i != 0){
				zombie.Name = $"Enemy{i}";
			}
			//GD.Print(zombie.Name);
			AddChild(zombie);
			i++;
		}
	}
}