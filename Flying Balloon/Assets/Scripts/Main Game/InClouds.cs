using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InClouds : MonoBehaviour {

	public int id = 0;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Balloon Top")) {
			GameControl.instance.inClouds[id] = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Balloon Top")) {
			GameControl.instance.inClouds[id] = false;
		}
	}

}
