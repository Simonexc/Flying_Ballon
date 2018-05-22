using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	public static GameControl instance;

	[HideInInspector]
	public bool gamePaused = true;
	[HideInInspector]
	public bool gameEnded = false;
	[HideInInspector]
	public bool gameStarted = false;

	[HideInInspector]
	public bool[] inClouds = new bool[2] {false, false}; // variable to store informations about clouds

	public float scrollSpeed = -1.5f; // speed at which the ground is moving
	public float cloudsHeight = 1; // the height of clouds
	public float slowDownDuration = 2; // the time it takes to slow down the ground to 0 after balloon hitting the ground
	public float cloudsScoreActivation = 0.2f; // the minimal position to activate the clouds 'score buster' - fraction of the clouds height

	public GameObject Balloon;
	public Text TapText;
	public GameObject GameOverPanel;

	[HideInInspector]
	public BalloonControl BalloonScript;

	private bool closeClouds = false; // if the clouds 'score buster' is active

	private float activationPoint = 0; // at what position the turbulences will appear
	private float maxPoint = 0; // position of the end of clouds
	private float timePassed = -1; // time passed since the balloon hit the ground

	private Transform TransformCheck; // reference to the balloon's transform

	private GameObject[] Grounds;

	void Awake () { // called before Start
		instance = this;
	}

	void Start () {
		gamePaused = true;

		GameOverPanel.SetActive (false); // turn off the restart screen
		TapText.gameObject.SetActive (true); // activate the player instructions
		BalloonScript = Balloon.GetComponent<BalloonControl> ();
		TransformCheck = BalloonScript.rbBalloon.gameObject.transform;

		Grounds = GameObject.FindGameObjectsWithTag ("Ground"); // get all grounds
	}

	void Update () {
		if (!gamePaused) {
			
			if (inClouds[0] || inClouds[1]) { // if the balloon is in clouds
				if (activationPoint == 0) { // if "activationPoint" isn't set
					activationPoint = TransformCheck.position.y - 0.02f; // set activation point
					maxPoint = activationPoint + cloudsHeight; // set maximal point
				}
				float a = TransformCheck.position.y - activationPoint;
				float b = maxPoint - activationPoint;
				// restrain values of a
				float val = Mathf.Min (b, Mathf.Max (a, 0)) / b;

				BalloonScript.AngleControlScript.rotate (val); // wiggle the balloon

				if (cloudsScoreActivation <= val && val < 1) { // if clouds 'score buster' is on
					if (closeClouds) // if it was in clouds before
						SetScore.instance.addScore ("Clouds", val); // add score
					else
						SetScore.instance.setScore (0, "Clouds"); // set new score
					closeClouds = true;
				} else { // if the clouds 'score buster' is no longer active
					if (closeClouds) // if it was active before
						SetScore.instance.deleteScore ("Clouds"); // delete the score
					closeClouds = false;
				}


				if (val == 1) { // if the balloon hit the top of the clouds
					// end the game
					stopGame ();
					BalloonScript.breakRope (); // disconnect the balloon from the basket
				}
			} else {
				BalloonScript.AngleControlScript.rotate (0); // stop wiggling the balloon
			}

		}

		if (timePassed >= 0 && timePassed < slowDownDuration) { // if the balloon already hit the ground but the grounds haven't stopped yet
			timePassed += Time.deltaTime;
			updateGrounds (Mathf.Max (0, 1 - timePassed / slowDownDuration)); // update the velocity of the grounds
		} else if (timePassed >= slowDownDuration) { // if the grounds have stopped already
			// show the restart screen
			ManageEndScreen.instance.theEnd ();
			GameOverPanel.SetActive (true);
			BalloonScript.freezeObject (true);
		}
	}

	private void updateGrounds (float speedModifier) { // updates the velocity of all grounds
		foreach (GameObject Ground in Grounds) {
			Ground.GetComponent<Scroll> ().updateSpeed (speedModifier);
		}
	}

	public void stopGame () { // end game
		gameEnded = true;
		pauseGame ();
	}

	public void hitGround () { // if the balloon hit the ground
		if (timePassed == -1) {
			timePassed = 0;
			BalloonScript.hardLanding (slowDownDuration);
		}
	}

	public void pauseGame () {
		gamePaused = true;
		if (!gameEnded) {
			updateGrounds (0); // freeze the grounds
		}
	}

	public void beginGame () { // start the game
		gameStarted = true;
		SpawnEnemies.letGoEnemies = true; // start spawning enemies
		TapText.gameObject.SetActive (false); // hide the instructions
		resumeGame ();
	}

	public void resumeGame () {
		gamePaused = false;
		BalloonScript.freezeObject (false);
		updateGrounds (1); // move the grounds
	}

	public void restartGame () {
		//instance = null;
		//Scroll.speedModifier = 0;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name); // reload the scene
	}

}
