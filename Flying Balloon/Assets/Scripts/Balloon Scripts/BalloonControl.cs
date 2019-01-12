using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour {
    // dodać animację uderzenia koszyka

	public float thrust = 1.5f; // amount of upward force applied per second(equivalent of change in air density when heating up air inside the balloon)
	public float antigravity = 50; // upward force to act as a drag

    public GameObject Top; // reference to the Top part of the balloon
    public GameObject Bottom; // reference to the basket

    public Transform FlameTrans;

	[HideInInspector]
	public AngleControl AngleControlScript; // it simulates turbulence while the balloon is in clouds
    [HideInInspector]
    public ScoreMultiplier ScoreMultiplierScript; // it handles adding 'bonus' scores
    [HideInInspector]
    public Explosions ExplosionsScript; // it handles balloon explosions

    private bool landed = false;

    private float avgVelocity = 0;

    private Vector2 balloonVelocity = new Vector2(0, 0);
    private float balloonAngularVelocity = 0;

    private Rigidbody2D rbBalloon;

    private Animator balloonAnimator;
    private Animator basketAnimator;

    void Start () {
		AngleControlScript = GetComponent<AngleControl>();
        ScoreMultiplierScript = GetComponent<ScoreMultiplier>();
        ExplosionsScript = GetComponent<Explosions>();

        rbBalloon = GetComponent<Rigidbody2D>();

        balloonAnimator = Top.GetComponent<Animator>();
        basketAnimator = Bottom.GetComponent<Animator>();
        balloonAnimator.enabled = false;
        basketAnimator.enabled = false;

        freezeObject (true); // stabilize the balloon
	}

	void Update () {
        avgVelocity = (avgVelocity * 9 + rbBalloon.velocity.y) / 10;

        FlameTrans.localScale = new Vector3(FlameTrans.localScale.x, 1.75f, FlameTrans.localScale.z);
        FlameTrans.localPosition = new Vector3(FlameTrans.localPosition.x, 3.17f, FlameTrans.localPosition.z);
        
		if (!landed) { // if ballooon didn't hit the ground
			if (Input.GetButton ("Fire1")) { // if mouse button is being hold(or screen is being touched when on mobile)
				if (!GameControl.gameStarted) { // if game hasn't begun yet but above is true
					GameControl.instance.beginGame ();
				}
				if (!GameControl.gamePaused) { // if game not paused
                    FlameTrans.localScale = new Vector3(FlameTrans.localScale.x, 2.8f, FlameTrans.localScale.z);
                    FlameTrans.localPosition = new Vector3(FlameTrans.localPosition.x, 3.38f, FlameTrans.localPosition.z);
					rbBalloon.AddForce (Vector2.up * thrust * Time.deltaTime); // apply force(upward * thrust * time it took to render this frame)
				}
			}
            if (!(GameControl.gamePaused && !GameControl.gameEnded) && Top.activeSelf)
            { // if game is not paused
                rbBalloon.AddForce(Vector2.up * antigravity * Time.deltaTime); // apply drag
            }
		} else { // if the balloon was destroyed
            FlameTrans.gameObject.SetActive(false);
		}

	}

    public void freezeObject (bool state) { // stabilize the balloon
		rbBalloon.isKinematic = state; // switches on/off ability to act on this object using forces
        if (state) // if freeze
        {
            balloonVelocity = rbBalloon.velocity;
            balloonAngularVelocity = rbBalloon.angularVelocity;
            rbBalloon.velocity = Vector2.zero;
            rbBalloon.angularVelocity = 0;
        } else
        {
            rbBalloon.velocity = balloonVelocity;
            rbBalloon.angularVelocity = balloonAngularVelocity;
        }
	}
		
	public void hardLanding () { // call this function if the balloon was damaged or it hit the ground(in other words the game is over)
        landed = true;
        if (Mathf.Abs(avgVelocity) > 1.5f)
        { 
            ExplosionsScript.explode (); // explode immediately
		}

        Top.GetComponent<PolygonCollider2D>().enabled = false; // turn off balloon collider for animation
        rbBalloon.angularVelocity = 0; // stop rotation

        // Play animations
        balloonAnimator.enabled = true;
        basketAnimator.enabled = true;
    }

    public void hideBalloon()
    {
        Top.SetActive(false);
    }

}
