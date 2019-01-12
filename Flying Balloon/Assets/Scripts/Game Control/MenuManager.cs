using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public float letterSpacing = 10;

    public GameObject TopScore;
    public Button MenuPlayButton;

	// Use this for initialization
	void Start () {
        if (!PlayerPrefs.HasKey("Top Score"))
        { // if the Top Score wasn't save before
            PlayerPrefs.SetInt("Top Score", 0);
        }
    }

    public void theEnd()
    {
        int score = Mathf.CeilToInt(ScoreManager.instance.score);
        int topScore = PlayerPrefs.GetInt("Top Score");
        if (topScore < score)
        {
            topScore = score;
            PlayerPrefs.SetInt("Top Score", topScore);
        }

        Text TopScoreText = TopScore.GetComponent<Text>();
        ScoreManager.instance.DisplayScore(TopScoreText, topScore);
        TopScore.transform.GetChild(0).localPosition += Vector3.right * letterSpacing * (13 - TopScoreText.text.Length);
        TopScore.GetComponent<Animator>().SetBool("Menu", true);
        MenuPlayButton.interactable = true;
    }

    public void hideMenu()
    {
        TopScore.GetComponent<Animator>().SetBool("Menu",false);
        MenuPlayButton.interactable = false;
    }
}
