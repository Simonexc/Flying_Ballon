using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenTransitions : MonoBehaviour {

    [HideInInspector]
    public bool landed = false;
    public float slowDownDuration = 2; // the time it takes to slow down the ground to 0 after balloon hitting the ground
    public float maxSpeed = 20;
    public float endTransitionTime = 1.5f;
    //public float flash = 0.5f;
    public AnimationCurve accelerationCurve;
    public AnimationCurve endTransitionCurve;

    public Animator EndFlashAnimator;
    public AnimationClip EndClip;
    
    private bool menuShown = false;
    private bool restart = false;
    //private bool start = true;
    private float timePassed = 0;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {   
        /*
        if (start)
        {
            timePassed += Time.deltaTime;
            EndFlash.SetActive(true);
            Color flashColor = EndFlash.GetComponent<Image>().color;
            flashColor.a = endTransitionCurve.Evaluate(1 - GameControl.mapVal(timePassed, flash));
            EndFlash.GetComponent<Image>().color = flashColor;
            if (timePassed >= flash)
            {
                timePassed = 0;
                start = false;
                EndFlash.SetActive(false);
            }
        }
        */
        if (landed && timePassed < slowDownDuration)
        { // if the balloon already hit the ground but the grounds haven't stopped yet
            timePassed += Time.deltaTime;
            GameControl.instance.moveAllScrollingObjects(accelerationCurve.Evaluate(1 - GameControl.mapVal(timePassed, slowDownDuration)));
        }
        else if (timePassed >= slowDownDuration)
        { // if the grounds have stopped already
          // show the restart screen
            if (!menuShown)
            {
                menuShown = true;
                GetComponent<MenuManager>().theEnd();
                GameControl.instance.BalloonScript.freezeObject(true);
            }
        }
        if (restart)
        {
            timePassed += Time.deltaTime;
            GameControl.instance.moveAllScrollingObjects(maxSpeed * endTransitionCurve.Evaluate(GameControl.mapVal(timePassed, endTransitionTime)));
            
            if (timePassed >= endTransitionTime - EndClip.length)
            {
                EndFlashAnimator.SetBool("End", true);
                /*EndFlash.SetActive(true);
                Color flashColor = EndFlash.GetComponent<Image>().color;
                flashColor.a = endTransitionCurve.Evaluate(GameControl.mapVal(timePassed - (endTransitionTime - flash), flash));
                EndFlash.GetComponent<Image>().color = flashColor;
                */
            }
            
            if (timePassed >= endTransitionTime)
                GameControl.instance.reloadScene();
        }
    }

    public void Restart()
    {
        restart = true;
        timePassed = 0;
    }
}
