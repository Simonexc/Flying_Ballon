using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

	public float scrollCoefficient = 1; // used for parallax effect

	private Rigidbody2D rbGround;

	void Start () {
		rbGround = GetComponent<Rigidbody2D> ();
		updateSpeed ();
	}

	public void updateSpeed (float speedModifier = 1) {
		Vector2 newVelocity = new Vector2 (GameControl.instance.scrollSpeed * scrollCoefficient * speedModifier, 0);
		rbGround.velocity = newVelocity;
	}
}
