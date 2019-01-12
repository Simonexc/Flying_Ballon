using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageEndScreen : MonoBehaviour {

	static public ManageEndScreen instance;

	public Text ScoreText; // reference to the Score Value Text
	public Text HighScoreText; // reference to the High Score Value Text
	public Text NewText; // reference to the New High Score Value indicator text

	void Start () {
		instance = this;

		if (!PlayerPrefs.HasKey ("High Score")) { // if the High Score wasn't save before
			PlayerPrefs.SetInt ("High Score", 0); // set High Score to 0
		}
	}

	public void theEnd () {
		int score = Mathf.CeilToInt (SetScore.instance.score); // convert score to int
		int highScore = PlayerPrefs.GetInt ("High Score"); // get High Score

		if (score > highScore) { // if you set the new record
			highScore = score;
			PlayerPrefs.SetInt ("High Score", highScore); // update High Score
			NewText.gameObject.SetActive (true); // show 'NEW' text
		}

		ScoreText.text = score.ToString (); // display score
		HighScoreText.text = highScore.ToString (); // display High Score
	}

}
