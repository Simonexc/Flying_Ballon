using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Balloon Top") || other.gameObject.CompareTag ("Balloon Bottom")) {
			GameControl.instance.stopGame ();
		}
	}

}
