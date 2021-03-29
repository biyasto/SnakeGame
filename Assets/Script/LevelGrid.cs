using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid   {

	private Vector2Int foodGridPosition;
	private int GridValue = 10;
	private int height;
	private int width;
	private Sprite foodSprite =GameAsset.instance.FoodSprite;
	GameObject foodGameObject;
	private Snake snake;
	public LevelGrid (int width, int height) {
		this.width = width;
		this.height = height;

	}
	public void Setup(Snake snake)
	{this.snake = snake;
		SpawnFood ();
	}
	private bool CheckPosition(Vector2Int foodGridPosition)
	{ 	
		for (int i = 1; i < snake.GetSnakeMovePositionList().Count; i++)
			if (foodGridPosition == snake.GetSnakeMovePositionList()[i]) {
				return true;
			}
		return false;
	}
	private bool notInBox(Vector2Int GridPosition)
	{ return(GridPosition.x >= GridValue * width || GridPosition.x <= -GridValue * width
		|| GridPosition.y >= GridValue * height || GridPosition.y <= -GridValue * height);
	}
	public bool GameOver()
	{ return (CheckPosition (snake.GetGridPosition())|| notInBox(snake.GetGridPosition()));
	}
	// Update is called once per frame
	public void SpawnFood(){
		do
		{
			foodGridPosition = new Vector2Int (5+GridValue*Random.Range (-width, width - 1), 5+GridValue*Random.Range (-height, height-1));
		}
		while(foodGridPosition == snake.GetGridPosition()||CheckPosition(foodGridPosition));
		foodGameObject = new GameObject ("Food", typeof(SpriteRenderer));
		Display (foodGameObject, foodSprite,foodGridPosition ,5);
	}
	private void Display(GameObject displayObject, Sprite displaySprite, Vector2Int Position,int sortingOrder)
	{
		displayObject.GetComponent<SpriteRenderer> ().sprite = displaySprite;
				displayObject.GetComponent<SpriteRenderer> ().sortingOrder = sortingOrder;
		displayObject.transform.position = new Vector3 (Position.x, Position.y);
	}
	public bool SnakeMoved(Vector2Int snakeGridPosition){
		if(snakeGridPosition == foodGridPosition){
			Object.Destroy(foodGameObject);
				SpawnFood();
			GameHandler.addScore ();
			Debug.Log("Eated");
			return true;
			}
		return false;
	}
	void Update () {

	}
}
