using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTankTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) { // if something touched the fuel tank
		GameControl.instance.BalloonScript.explode (); // trigger the explosion
		if (other.CompareTag ("Enemy")) { // if it was an enemy
			GameControl.instance.stopGame ();
			GameControl.instance.hitGround (); // simulate hitting the ground
			other.GetComponent<EnemyTrigger> ().DestroyThisObject (); // destroy the enemy
		}
	}
}
