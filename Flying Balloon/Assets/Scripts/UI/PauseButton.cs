using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (!GameControl.gameStarted || GameControl.gameEnded || GameControl.gamePaused)
            button.interactable = false;
        else
            button.interactable = true;
    }

    public void PauseFunction()
    {
        GameControl.instance.pauseGame();
        /*
        if (GameControl.instance.gameStarted)
        {
            if (GameControl.instance.gamePaused)
            {
                GameControl.instance.resumeGame();
            } else
            {
                GameControl.instance.pauseGame();
            }
        }
        */
    }
}
