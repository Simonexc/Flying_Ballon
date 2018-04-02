using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

	public static float speedModifier = 0; // used for global slowdown

	public bool affectedBySpeedModifier = true;
	public float scrollCoefficient = 1; // used for parallax effect

	private Rigidbody2D rbGround;

	void Start () {
		rbGround = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		Vector2 newVelocity = new Vector2 (GameControl.instance.scrollSpeed * scrollCoefficient, 0);
		if (affectedBySpeedModifier) {
			newVelocity *= speedModifier;
		}
		rbGround.velocity = newVelocity;
	}
}
