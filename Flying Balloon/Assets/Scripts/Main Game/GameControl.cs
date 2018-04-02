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
	public bool[] inClouds = new bool[2] {false, false};

	public float scrollSpeed = -1.5f;
	public float cloudsHeight = 1;
	public float slowDownDuration = 2; // in seconds

	public GameObject Balloon;
	public Text TapText;
	public GameObject GameOverPanel;

	[HideInInspector]
	public BalloonControl BalloonScript;

	private float activationPoint = 0;
	private float maxPoint = 0;
	private float timePassed = -1;

	private Transform TransformCheck;

	void Start () {
		Debug.Log ("START");
		instance = this;
		gamePaused = true;

		GameOverPanel.SetActive (false);
		TapText.gameObject.SetActive (true);
		BalloonScript = Balloon.GetComponent<BalloonControl> ();
		TransformCheck = BalloonScript.rbBalloon.gameObject.transform;
	}

	void Update () {
		if (!gamePaused) {
			
			if (inClouds[0] || inClouds[1]) {
				if (activationPoint == 0) {
					activationPoint = TransformCheck.position.y - 0.02f; // set activation point
					maxPoint = activationPoint + cloudsHeight; // set maximal point
				}
				float a = TransformCheck.position.y - activationPoint;
				float b = maxPoint - activationPoint;
				// restrain values of a
				float val = Mathf.Min (b, Mathf.Max (a, 0)) / b;

				BalloonScript.AngleControlScript.rotate (val);


				if (val == 1) {
					stopGame ();
					BalloonScript.breakRope ();
				}
			} else {
				BalloonScript.AngleControlScript.rotate (0);
			}

		}

		if (timePassed >= 0 && timePassed < slowDownDuration) {
			timePassed += Time.deltaTime;
			Scroll.speedModifier = Mathf.Max (0, 1 - timePassed / slowDownDuration);
		} else if (timePassed >= slowDownDuration) {
			GameOverPanel.SetActive (true);
			BalloonScript.freezeObject (true);
		}
	}

	public void stopGame () {
		gameEnded = true;
		pauseGame ();
	}

	public void hitGround () {
		if (timePassed == -1) {
			timePassed = 0;
			BalloonScript.hardLanding (slowDownDuration);
		}
	}

	public void pauseGame () {
		gamePaused = true;
		if (!gameEnded) {
			Scroll.speedModifier = 0;
		}
	}

	public void beginGame () {
		gameStarted = true;
		TapText.gameObject.SetActive (false);
		resumeGame ();
	}

	public void resumeGame () {
		gamePaused = false;
		BalloonScript.freezeObject (false);
		Scroll.speedModifier = 1;
	}

	public void restartGame () {
		//instance = null;
		//Scroll.speedModifier = 0;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

}
