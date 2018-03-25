using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public float maxPoint = 4.6f;

	public GameObject Balloon;
	public Text TapText;
	public GameObject GameOverPanel;
	public GameObject Enemy;

	private float activationPoint = 0;

	private BalloonControl balloonScript;

	// Use this for initialization
	void Start () {
		instance = this;
		gamePaused = true;

		GameOverPanel.SetActive (false);
		TapText.gameObject.SetActive (true);
		balloonScript = Balloon.GetComponent<BalloonControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gamePaused) {
			
			//SpriteRenderer sprite = Balloon.GetComponent<SpriteRenderer> ();
			if (inClouds[0] || inClouds[1]) {
				if (activationPoint == 0f) {
					activationPoint = Balloon.transform.position.y - 0.02f; // set activation point
				}
				float a = Balloon.transform.position.y - activationPoint;
				float b = maxPoint - activationPoint;
				// restrain values of a
				float val = Mathf.Min (b, Mathf.Max (a, 0)) / b;
				//sprite.color = new Color (1f, 1f, 1f - val);

				balloonScript.rotate (val);


				if (val == 1) {
					stopGame ();
				}
			} else {
				balloonScript.rotate (0);
				//sprite.color = Color.white;
			}

		}
	}

	public void stopGame () {
		gameEnded = true;
		pauseGame ();
		GameOverPanel.SetActive (true);
	}

	public void pauseGame () {
		gamePaused = true;
		balloonScript.freezeObject (true);
	}

	public void beginGame () {
		gameStarted = true;
		TapText.gameObject.SetActive (false);
		resumeGame ();
	}

	public void resumeGame () {
		gamePaused = false;
		balloonScript.freezeObject (false);
	}

	public void restartGame () {
		gameEnded = false;
		gameStarted = false;

		GameOverPanel.SetActive (false);
		TapText.gameObject.SetActive (true);
		balloonScript.bringItBack ();
	}

}
