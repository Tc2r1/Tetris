using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

	public float fallSpeed;

	public GameManager gameManager;
	public SpawnManager spawnManager;
	private float lastFall = 0;
	private bool gameOver = false;

	// Use this for initialization
	void Start () {

		if(!gameManager){
			gameManager = FindObjectOfType<GameManager>();
		}

		if(!spawnManager){
			spawnManager = FindObjectOfType<SpawnManager>();
		}
		// On Initial Spawn, if Invalid, Trigger GameOver
		if(!isValidGridPos() && !gameOver){
			gameManager.setGameOver();
			//Destroy(gameObject);
			DestroyImmediate (gameObject);
			gameOver = true;
		}


	}

	// Update is called once per frame
	void Update() {
		// Move Left
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			// Modify position
			transform.position += new Vector3(-1, 0,0);

			// Check if position is valid
			if (isValidGridPos()){
				updateGrid();
			} else {
				transform.position += new Vector3(1,0,0);
			}
		}

		// Move Left
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			// Modify position
			transform.position += new Vector3(1, 0,0);

			// Check if position is valid
			if (isValidGridPos()){
				updateGrid();
			} else {
				transform.position += new Vector3(-1,0,0);
			}
		}

		// Rotate
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			transform.Rotate(0,0,-90);

			// See if valid
			if (isValidGridPos()) {
				updateGrid();
			} else {
				transform.Rotate(0,0,90);
			}
		 }

		// Fall
		if (Input.GetKeyDown(KeyCode.DownArrow) ||
			Time.time - lastFall >= fallSpeed){
			// Modify Position
			transform.position += new Vector3(0, -1, 0);

			// See if valid
			if(isValidGridPos()){
				updateGrid();
			} else {
				// It's not valid, revert.
				transform.position += new Vector3(0,1,0);

				// Clear filled horizontal lines
				Grid.deleteFullRows();

				// Spawn next Group
				if (!GameManager.Instance.isGameOver()){
					spawnManager.spawnNext();
				}

				// Disable script
				enabled = false;
			}
			lastFall = Time.time;
		}
	}

	// Varifies each child block's position
	bool isValidGridPos(){
		foreach (Transform child in transform){
			Vector2 v = Grid.roundVec2(child.position);

			// If not inside border?
			if(!Grid.insideBorder(v))
			return false;

			// Block in grid cell (and not part of the same group)?
			if (Grid.grid[(int)v.x, (int)v.y] != null &&
            Grid.grid[(int)v.x, (int)v.y].parent != transform)
            return false;
		}
		return true;
	}

	void updateGrid(){
		// Remove old children from the grid
		for (int y = 0; y < Grid.h; ++y)
			for (int x = 0; x < Grid.w; ++x)
				if (Grid.grid[x, y] != null)
					if (Grid.grid[x, y].parent == transform)
						Grid.grid[x, y] = null;

		// Add new children to grid
		foreach (Transform child in transform){
			Vector2 v = Grid.roundVec2(child.position);
			Grid.grid[(int)v.x, (int)v.y] = child;
		}
	}
}
