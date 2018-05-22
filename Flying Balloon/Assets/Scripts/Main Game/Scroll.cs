using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

	public float scrollCoefficient = 1; // used for parallax effect

	private Rigidbody2D rbGround; // reference to "Rigidbody2D" of this ground

	void Start () {
		rbGround = GetComponent<Rigidbody2D> ();
		updateSpeed (); // set the speed of the ground
	}

	public void updateSpeed (float speedModifier = 1) { // sets the speed of the ground
		float newVelocity = GameControl.instance.scrollSpeed * scrollCoefficient * speedModifier;
		rbGround.velocity = newVelocity * transform.right;
	}
}
