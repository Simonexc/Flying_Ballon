using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public bool constantUpdate = false; // constantly update the speed
    public bool affected = true;
    public float scrollCoefficient = 1; // used for parallax effect
    public float minimalScrollCoefficient = 0; // used for things that moves after the end of the game

    private float actSpeed = 0;

	private Rigidbody2D rbGround; // reference to "Rigidbody2D" of this ground

	void Start () {
		rbGround = GetComponent<Rigidbody2D> ();
		updateSpeed (1); // set the speed of the ground
	}

    void Update()
    {
        /*if (constantUpdate)
        {
            updateSpeed();
        }*/
    }

    public void updateSpeed (float speedModifier = -1, bool forced = false) { // sets the speed of the ground
        if (affected)
        {
            if (actSpeed != -1)
                actSpeed = GameControl.instance.scrollSpeed * (minimalScrollCoefficient + (scrollCoefficient - minimalScrollCoefficient) * speedModifier);
            if (forced)
            {
                rbGround.velocity = Vector2.zero;
                actSpeed = 0;
            }
            else
            {
                rbGround.velocity = actSpeed * transform.right;
            }
        }
	}
}
