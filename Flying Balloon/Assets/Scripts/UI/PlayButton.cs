using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }
    
    void Update () {
		if (GameControl.gamePaused && GameControl.gameStarted && !GameControl.gameEnded)
            button.interactable = true;
        else
            button.interactable = false;
    }

    public void PlayFunction()
    {
        GameControl.instance.resumeGame();
    }
}
