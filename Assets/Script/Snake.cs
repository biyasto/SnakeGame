using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
	private List<Vector2Int> SnakeMovePositionList;
	private Vector2Int gridMoveDirection;
	private Vector2Int waitGridMoveDirection;
	private Vector2Int gridPosition;
	private Vector2Int tailPosition;
	private float gridMoveTimer;
	private float gridMoveTimerMax;
	private float Speed;
	private float startedSpeed;
	private int snakeBodySize;
	private int SnakeSortingOrder;
	private int gridValue = 10;
	private bool Dead;
	private bool Started;

	private LevelGrid LevelGrid;
	public Sprite snakebodySprite; 
	public Sprite snaketailSprite;
	GameObject SnakebodyObject;

	public void Setup(LevelGrid levelGrid)
	{
		this.LevelGrid = levelGrid;
	}

	private void Awake () {
		gridPosition = new Vector2Int (5, -25); //started position
		startedSpeed = 3f; // ? time per second
		gridMoveTimerMax = 1f/Speed;
		gridMoveTimer = 1f/startedSpeed;
		waitGridMoveDirection = new Vector2Int (0,gridValue);
		SnakeMovePositionList = new List<Vector2Int> ();
		snakeBodySize = 2;
		SnakeSortingOrder = 3;
		Dead = false;
		Started = false;
	}


	private void Update() {
		if (Started) {
			if (!Dead) {
				SpeedUp ();
				HandlerInput ();
				HandlerGridMovement ();
				if (Dead)
					DisplayDead ();

			}
			if (Dead && Input.GetKeyDown (KeyCode.R)) {
				{	

					Loader.Load (Loader.Scene.GameScene);
					Dead = false;
				}
			}
		} else { 
			
			if (Input.GetKeyDown (KeyCode.T))
			{
				Started = true;

			}
		}
	}
	private void SpeedUp()
	{ 
		Speed = (snakeBodySize - 1)/startedSpeed + startedSpeed;
		gridMoveTimerMax = 1f/Speed;

	}
	private void HandlerGridMovement()
	{	

		gridMoveTimer += Time.deltaTime;
		if (gridMoveTimer >= gridMoveTimerMax) {
			gridMoveDirection = waitGridMoveDirection;
			gridPosition += gridMoveDirection;
			gridMoveTimer -= gridMoveTimerMax;
			//check death statement
			if (LevelGrid.GameOver ()) {
				Debug.Log ("Dead");
				Dead = true;
				return;
			}
			//add body length when eat food, add score
			SnakeMovePositionList.Insert (0, gridPosition);

			if (LevelGrid.SnakeMoved (gridPosition)) {
				snakeBodySize++;
			}
			;
			//check body length
			if (SnakeMovePositionList.Count >= snakeBodySize + 1) {
				SnakeMovePositionList.RemoveAt (SnakeMovePositionList.Count - 1);
			}
			//display body // true=detroy body afterdisplay
			DisplayBody(true);
			//transforn head
			transform.eulerAngles = new Vector3 (0, 0, RouteAngle (gridMoveDirection));
			transform.position = new Vector3 (gridPosition.x, gridPosition.y);


		}
	}
	private void DisplayDead()
	{	//false = not detroy body after display
		DisplayBody (false);
		//route the head
		transform.eulerAngles = new Vector3 (0, 0, RouteAngle (gridMoveDirection));
	

	}

	private void DisplayBody(bool delete)
	{
		for (int i = 1; i < SnakeMovePositionList.Count; i++) {

			SnakebodyObject = new GameObject ("body", typeof(SpriteRenderer));

			//find sprite,sorting
			int SortingOrder=SnakeSortingOrder;
			Sprite sprite = snakebodySprite;
			if (i == SnakeMovePositionList.Count - 1) {
				sprite = snaketailSprite;
				SortingOrder = SnakeSortingOrder - 1;
			}
			//display 
			Display(SnakebodyObject, sprite, SnakeMovePositionList [i], SortingOrder);

			//find angle to route
			float angle = 0;
			int next = nextPostion (SnakeMovePositionList [i], SnakeMovePositionList [i - 1]);

			if (i != SnakeMovePositionList.Count - 1) 
			{
				int pre = nextPostion (SnakeMovePositionList [i], SnakeMovePositionList [i + 1]);
				angle = AngleBody (next, pre);
			}
			else 
			{ 
				angle = AngleTail (next);
			}
			//route body
			SnakebodyObject.transform.eulerAngles = new Vector3 (0, 0, angle);
			//detroy body after display
			if(delete) Destroy (SnakebodyObject, gridMoveTimerMax);


		}
	}
	private void Display(GameObject displayObject, Sprite displaySprite, Vector2Int Position, int sortingOrder)
	{
		displayObject.GetComponent<SpriteRenderer> ().sprite = displaySprite;
		displayObject.GetComponent<SpriteRenderer> ().sortingOrder = sortingOrder;
		displayObject.transform.position = new Vector3 (Position.x, Position.y);
	}


	private void HandlerInput()//moverment input
	{	
		if (Input.GetKeyDown (KeyCode.UpArrow) && gridMoveDirection.y != -gridValue ) {
			waitGridMoveDirection.x = 0;
			waitGridMoveDirection.y = gridValue;
		}
		else
			if (Input.GetKeyDown (KeyCode.DownArrow) && gridMoveDirection.y != gridValue ) {
				waitGridMoveDirection.x = 0;
				waitGridMoveDirection.y = -gridValue;
			}
			else
				if (Input.GetKeyDown (KeyCode.LeftArrow) && gridMoveDirection.x != gridValue) {
					waitGridMoveDirection.x = -gridValue;
					waitGridMoveDirection.y = 0;
				}
				else
					if (Input.GetKeyDown (KeyCode.RightArrow)&& gridMoveDirection.x != -gridValue) {
						waitGridMoveDirection.x = gridValue;
						waitGridMoveDirection.y = 0;
					}
	}
	private int nextPostion(Vector2Int p1, Vector2Int p2)
	{
		if(p1.x!=p2.x)
			{if(p1.x>p2.x) return 2;
			 else 		   return 4;}
		else
			{if(p1.y>p2.y) return 3;
			 else 		   return 1;}
	}
	private float RouteAngle(Vector2Int Direction){
		float n = Mathf.Atan2 (Direction.y, Direction.x) * Mathf.Rad2Deg;
		if (n < 0)
			n = n + 360;
		if (n > 360)
			n = n - 360;
		return n-90f;
	}

	private float AngleTail(int next)
	{	
		float angle = 0;
		switch(next){
		case 1: {angle=0;break;}
		case 2: {angle=90;break;}
		case 3: {angle=180;break;}
		case 4: {angle=-90;break;}
		default : break;
		}
		return angle;
	}
	private float AngleBody(int next, int pre)
	{ float angle = 0;
		switch (next) {
		case 1:
			{switch (pre) {
				case 2:{angle = -135;break;}
				case 3:{angle = 180;break;}
				case 4:{angle = 135;break;}
				default:break;}
			break;}
		case 2:
			{switch (pre) {
				case 1:{angle = -135;break;}
				case 3:{angle = -45;break;}
				case 4:{angle = 90;break;}
				default:break;}
			break;}
		case 3:
			{switch (pre) {
				case 1:{angle = 0;break;}
				case 2:{angle = -45;break;}
				case 4:{angle = 45;break;}
				default:break;}
				break;}
		case 4:
			{switch (pre) {
				case 1:{angle = 135;break;}
				case 2:{angle = -90;break;}
				case 3:{angle = 45;break;}
				default:break;}
				break;}
		default:
		break;
		}
	return angle;
	}
	public Vector2Int GetGridPosition ()
	{ return gridPosition;
	}
	public List<Vector2Int> GetSnakeMovePositionList()
	{ return SnakeMovePositionList;
	}
	public bool isDead()
	{
		return Dead;
	}
	public bool isStarted()
	{
		return Started;
	}

}