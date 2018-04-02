using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisions : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.CompareTag ("Balloon Bottom")) {
			GameControl.instance.stopGame ();
			GameControl.instance.hitGround ();
		}
	}

}
