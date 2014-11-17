using System;
using GXPEngine;
using System.Drawing;

public class MyGame : Game
{
    Player player;
	public MyGame () : base(800, 600, false)
	{
        player = new Player();
        this.AddChild(player);
	}
	
	void Update () {
		//empty
	}

	static void Main() {
		new MyGame().Start();
	}
}

