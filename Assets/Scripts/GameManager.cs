using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


	// static singleton variable to reference self
	public static GameManager Instance;

	void Awake(){
		Instance = this;
	}

	// Display Variables
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	private int score;
	private Animator animator;
	private bool gameOver;
	private bool restart;

	// Use this for initialization
	void Start(){
		gameOver = false;
		restart  = false;
		animator = gameOverText.GetComponent<Animator> ();
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		displayScore();
	}

	// Update is called once per frame
	void Update () {
		if(restart){
			if(Input.GetKeyDown(KeyCode.R)){
				// reload the scene!
				// Depricated: Application.LoadLevel(Application.loadedLevel);
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

			}
		}

	}

	void displayScore(){
		scoreText.text = "Score: "+ score;
	}


	public void addScore(int newScoreValue){
		score += newScoreValue;
		displayScore();
	}

	public bool isGameOver(){
		return gameOver;
	}

	public void setGameOver(){
		animator.SetTrigger ("GameOverTrigger");
		gameOverText.text = "Game Over";
		gameOver = true;
	}
	public void setRestart(){
		restartText.text = "Press R for Restart";
		restart = true;
	}
}
