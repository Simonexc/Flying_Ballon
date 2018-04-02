using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTankTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		GameControl.instance.BalloonScript.explode ();
	}
}
