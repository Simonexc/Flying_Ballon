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

	private float activationPoint = 0f;

	// Use this for initialization
	void Start () {
		instance = this;
		gamePaused = true;
		GameOverPanel.SetActive (false);
		TapText.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (!gamePaused) {
			
			SpriteRenderer sprite = Balloon.GetComponent<SpriteRenderer> ();
			if (inClouds[0] || inClouds[1]) {
				if (activationPoint == 0f) {
					activationPoint = Balloon.transform.position.y - 0.02f; // set activation point
				}
				float a = Balloon.transform.position.y - activationPoint;
				float b = maxPoint - activationPoint;
				// restrain values of a
				float val = Mathf.Min (b, Mathf.Max (a, 0)) / b;
				sprite.color = new Color (1f, 1f, 1f - val);

				BalloonControl.instance.rotate (val);
				//Rigidbody2D rbBalloon = Balloon.GetComponent<Rigidbody2D> ();
				/*
				Debug.Log (Balloon.transform.rotation.z);
				if (Balloon.transform.rotation.z > Mathf.Deg2Rad * 40f * val) {
					Debug.Log (">");
					rbBalloon.angularVelocity = val * Random.Range (-60f * val, 0f);
				} else if (Balloon.transform.rotation.z < Mathf.Deg2Rad *  -40f * val) {
					Debug.Log ("<");
					rbBalloon.angularVelocity = val * Random.Range (0f, 60f * val);
				} else {
					rbBalloon.angularVelocity = Mathf.Max(-60f*val, Mathf.Min(rbBalloon.angularVelocity + val * Random.Range (-3f * val, 3f * val), -60f*val));
				}
				*/


				if (val == 1) {
					stopGame ();
				}
			} else {
				BalloonControl.instance.rotate (0);
				sprite.color = Color.white;
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
		BalloonControl.instance.freezeObject (true);
	}

	public void beginGame () {
		gameStarted = true;
		TapText.gameObject.SetActive (false);
		resumeGame ();
	}

	public void resumeGame () {
		gamePaused = false;
		BalloonControl.instance.freezeObject (false);
	}

	public void restartGame () {
		gameEnded = false;
		gameStarted = false;
		gamePaused = true;
		GameOverPanel.SetActive (false);
		TapText.gameObject.SetActive (true);
		BalloonControl.instance.bringItBack ();
	}

}
