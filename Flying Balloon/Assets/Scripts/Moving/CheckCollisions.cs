using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisions : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D collision) { // if some object hit the ground
		if (collision.gameObject.CompareTag ("Player")) { // if it was the balloon
			// end the game and announce the hit
			GameControl.instance.stopGame ();
			GameControl.instance.hitGround ();
		}
	}

}
