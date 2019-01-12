using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScore : MonoBehaviour {

	public static SetScore instance;

	// score values for 1 second of performing a certain task
	public Dictionary<string, float> scoresPerSecond = new Dictionary<string, float> () {
		{"Main", 40}, // just fly
		{"Clouds", 100}, // fly in clouds
		{"Ground", 100} // fly near the ground
	};
	// score values for performing a certain task
	public Dictionary<string, float> instantaneousScore = new Dictionary<string, float> () {
		{"Enemy", 5000} // hit enemy eagle from top
	};
	public float deleteDelay = 2; // after how much time the score information should be deleted
	public float height = 300; // what is the offset between new scores
	[HideInInspector]
	public float score = 0; // easy to access main score - add scores to this score before deleting

	public GameObject ScorePrefab;
	public Canvas MainCanvas; // reference to the canvas where the scores will be displayed

	private List<float> scores = new List<float> (); // list of scores displayed(values)
	private List<GameObject> ScoreTexts = new List<GameObject> (); // list of scores displayed(objects)
	private List<int> removeList = new List<int> (); // list of objects avaiting removing
	private Dictionary<string, int> overTimeScoreBustersIDs = new Dictionary<string, int> (); // dictionary with reference ID to the 'score busters' that increase your score over time

	void Start () {
		instance = this;
		height *= MainCanvas.scaleFactor; // change the offset according to the canvas scaling
		setScore (0, "Main"); // set main score to 0
	} 

	void Update () {
		if (!GameControl.gamePaused) { // if game is not paused(or ended)
			addScore ("Main"); // add score for just flying
		}
	}

	IEnumerator DeleteAfterTime (float time) { // this function deletes the first score from "removeList" after a "time" seconds
		yield return new WaitForSeconds (time); // delay time seconds

		// remove the first element from "removeList"
		int i = removeList [0]; 
		removeList.RemoveAt (0);

		Dictionary<string, int> newDict = new Dictionary<string, int>(overTimeScoreBustersIDs); // create a copy of dictionary
		foreach (KeyValuePair<string, int> item in overTimeScoreBustersIDs) { // iterate over whole dictonary where temporary IDs to 'score busters' are stored
			if (item.Value > i) // change the ID
				newDict [item.Key] = item.Value-1; // (if "item" is higher on the list than the item which is being deleted - lower its ID by one to match the changed order)
		}
		overTimeScoreBustersIDs = newDict;

		// change the values of "removeList" like described above
		for(int j = 0; j<removeList.Count; j++) {
			if (removeList[j] > i)
				removeList[j]--;
		}

		incrementScore (scores [i], "Main"); // add the deleted score to the main score
		Destroy (ScoreTexts [i]); // destroy the score object
		ScoreTexts.RemoveAt (i); // remove it from "ScoreTexts"
		scores.RemoveAt (i); // remove score value from "scores"
		reposition (i); // reposition the displayed scores
	}

	public void setScore (float newScore, string type = "Default") { // create new score entry
		int i = scores.Count; // get the position of the new score
			
		scores.Add (newScore); // add new score value to "scores"
		ScoreTexts.Add (createScore (i)); // add new object to "ScoreTexts"

		score += newScore; // increase the score counter

		if (type != "Default") { // if "type" isn't default - it is a 'score buster'
			if (overTimeScoreBustersIDs.ContainsKey (type)) // if it was created before
				overTimeScoreBustersIDs [type] = i; // change the ID
			else
				overTimeScoreBustersIDs.Add (type, i); // add new entry
		} else // if it's one-time buster
			deleteScore ("Default", i); // start deleting
		
		updateScoreText (i); // updates the value of the "i"th score
	}

	public void deleteScore (string type, int i = -1) { // this function deletes the score
		if (i == -1) // if no ID was given
			i = overTimeScoreBustersIDs [type]; // it means it's a 'score buster'
		removeList.Add (i); // add the score ID to "removeList"

		StartCoroutine (DeleteAfterTime (deleteDelay)); // begin deleting
	}

	private void reposition (int i) { // this function graphically reposition the scores when a score was delete
		for (; i<ScoreTexts.Count; i++) { // for each score object in "ScoreTexts" which position is >= i
			ScoreTexts [i].transform.position += Vector3.down * height; // move it down by "height"
		}
	}

	private GameObject createScore (int i) { // this function creates a score object from the prefab
		GameObject newScoreText = Instantiate (ScorePrefab); // instantiate the object
		newScoreText.transform.SetParent (MainCanvas.transform, false); // set parent object to be the canvas
		newScoreText.transform.position += Vector3.up * height * i; // set the position
		return newScoreText;
	}

	public void addScore (string type, float affector = 1) { // this function adds the score
		int i = overTimeScoreBustersIDs [type];
		float increaseScore = scoresPerSecond[type] * affector * Time.deltaTime; // amount of score per second * some affector(default = 1) * time it took to render the frame

		score += increaseScore; // increase the score counter
		scores [i] += increaseScore;

		updateScoreText (i); // updates the displayed score
	}

	private void incrementScore (float val, string type) { // this function increments the 'score buster' "type" by val
		int i = overTimeScoreBustersIDs [type]; // gets ID
		scores [i] += val; // increments the score
		updateScoreText (i); // updates the displayed score
	}

	private void updateScoreText (int i) { // this function updates the displayed score
		int intScore = Mathf.CeilToInt (scores[i]); // converts to float to int
		Text ScoreText = ScoreTexts [i].transform.GetChild (0).GetComponentInChildren<Text> (); // gets the text from the instance of score

		string textVal = intScore.ToString (); // text to display
		if (i != 0) // if it's not the first score
			textVal = "+ " + textVal; // add the + sign before the value
		ScoreText.text = textVal; // updates the display
	}
}
