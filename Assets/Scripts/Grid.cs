using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	// The Grid itself
	public static int w = 10;
	public static int h = 20;
	public static Transform[,] grid = new Transform[w,h];

	public GameManager gameManager;

	private bool gameEnded;
	public float destroyDelay = 0.1f;

	// Use this for initialization
	void Start () {
		gameEnded = false;
		gameManager = GetComponent<GameManager>();

	}

	// Update is called once per frame
	void Update () {

		if(gameManager.isGameOver() && !gameEnded){
			StartCoroutine(EndGameRowDestroy());
			gameEnded = true;

		}
	}
	IEnumerator EndGameRowDestroy(){
		for(int i=0; i<19; i++){
			deleteRow(0);
			decreaseRowsAbove(0);
			Debug.Log("Waiting Now");
			yield return new WaitForSeconds(destroyDelay);
		}
		GameManager.Instance.setRestart();
	}

	// Rounds a vector in case rotations cause coordinates not to be round any longer.
	public static Vector2 roundVec2(Vector2 v){
		return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
	}

	// checks to see if coordinates are inside border or not.
	public static bool insideBorder(Vector2 pos){
		return((int)pos.x >= 0 && (int) pos.x < w && (int)pos.y >= 0);
	}

	// Delets all blocks in a given row.
	public static void deleteRow(int y){
		for (int x = 0; x < w; ++x){
			if(grid[x,y]){
				Destroy(grid[x,y].gameObject);
				grid[x,y] = null;
			}
		}
	}

	// makes rows above fall by one unit after a row is deleted.
	public static void decreaseRow(int y){

		// loop through entire row
		for(int x = 0; x < w; ++x){
			// if there is a block
			if (grid[x, y] != null){
				// move one towards the bottom
				grid[x, y-1] = grid[x, y];
				grid[x, y] = null;

				// Update block Positions
				grid[x, y-1].position += new Vector3(0, -1, 0);

			}
		}
	}

	// Decrease all rows above
	public static void decreaseRowsAbove(int y){
		for(int i = y; i < h; ++i){
			decreaseRow(i);
		}
	}

	// Checks to see if row is full.
	public static bool isRowFull(int y){
		for (int x = 0; x < w; ++x){
			if (grid[x,y] == null){
				return false;
			}

		}
		return true;
	}

	// Deletes all full rows and then always decreases the above row's y coordinate by one.
	public static void deleteFullRows(){
		for (int y = 0; y < h; ++y){
			if (isRowFull(y)){
				deleteRow(y);
				decreaseRowsAbove(y+1);
				GameManager.Instance.addScore(10);
				--y;
			}
		}
	}
}
