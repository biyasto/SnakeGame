using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
	[SerializeField] public Snake Snake;
	private LevelGrid lvGrid;
	private static int Score=1;
	private static bool isDead;
	private static bool isStarted;
	public void Start() {
		resetScore();
		Debug.Log("GameHander.Start");
		lvGrid = new LevelGrid (10, 10);
		Snake.Setup (lvGrid);
		lvGrid.Setup (Snake);
	}
	public void Update() {
		isDead = Snake.isDead ();
		isStarted = Snake.isStarted();
	}
	public static bool GetStatus()
	{return isDead;
	}
	public static bool GetStarted()
	{return isStarted;
	}
	public static int GetScore()
	{return Score;
	}
	public static void addScore()
	{ Score += 10;
	}
	private static void resetScore(){
		Score = 0;
	}
}
