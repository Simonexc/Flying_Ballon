using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager instance;

    public int scorePerSecond = 10;
    public int hitBirdScore = 500;
    public int inCloudsScorePerSecond = 40;

    public float overlapingScoresDisplacement = 0.5f;

    [HideInInspector]
    public float score = 0;

    public Canvas HUD;

    public Text Score;

    public GameObject ScoreAddTextPrefab;

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        Score.text = "0";
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameControl.gamePaused)
        {
            score += scorePerSecond * Time.deltaTime;
            DisplayScore(Score, score);
        }
	}

    public GameObject CreateScore(Vector3 position, bool selfDestruct)
    {
        position = new Vector3(position.x / HUD.transform.localScale.x, position.y / HUD.transform.localScale.y, position.z);
        float occupiedPosition = CloudsMultiplier.instance.activationPoint + CloudsMultiplier.instance.scoreDisplacement.y;
        if (Mathf.Abs(position.y * HUD.transform.localScale.y - occupiedPosition) < overlapingScoresDisplacement && CloudsMultiplier.instance.CloudsScore != null)
        {
            if (position.y * HUD.transform.localScale.y < occupiedPosition) // if score position is lower than currently occupied
                position = new Vector3(position.x, (occupiedPosition - overlapingScoresDisplacement) / HUD.transform.localScale.y, position.z);
            else
                position = new Vector3(position.x, (occupiedPosition + overlapingScoresDisplacement) / HUD.transform.localScale.y, position.z);
        }
        GameObject newScoreText = Instantiate(ScoreAddTextPrefab, position, Quaternion.identity); // instantiate the object
        newScoreText.transform.SetParent(HUD.transform, false); // set parent object to be the canvas
        if (!selfDestruct)
        {
            newScoreText.GetComponent<SelfDestruct>().duration = 0;
        }

        return newScoreText;
    }

    public void UpdateScore(GameObject scoreText, int value)
    {
        Text newText = scoreText.GetComponent<Text>();
        DisplayScore(newText, value);
        newText.text = "+" + newText.text;
    }

    public void AddScore(int value, Vector3 position)
    {
        Add(value);
        GameObject newScoreText = CreateScore(position, true);
        UpdateScore(newScoreText, value);
    }

    public void Add(int value)
    {
        score = Mathf.Min(score + value, 2000000000);
    }

    public void Add(float value)
    {
        score = Mathf.Min(score + value, 2000000000);
    }

    public void DisplayScore(Text scoreDisplay, float val)
    {
        int scoreProcess = Mathf.CeilToInt(val);
        string scoreDisplayed = "";
        while (scoreProcess > 0)
        {
            if (scoreDisplayed != "")
                scoreDisplayed = " " + scoreDisplayed;
            string newAddition = (scoreProcess % 1000).ToString();
            if (scoreProcess > 1000) // add missing 0s
            {
                if (scoreProcess % 1000 < 100)
                    newAddition = "0" + newAddition;
                if (scoreProcess % 1000 < 10)
                    newAddition = "0" + newAddition;
            }
            scoreDisplayed = newAddition + scoreDisplayed;
            scoreProcess /= 1000;
        }
        if (scoreDisplayed == "")
            scoreDisplayed = "0";

        scoreDisplay.text = scoreDisplayed;
    }
    
}
