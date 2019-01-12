using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameControl.gamePaused && !GameControl.gameEnded && GameControl.gameStarted) // if game paused
        {
            anim.speed = 0;
        } else
        {
            anim.speed = 1;
        }
	}
}
