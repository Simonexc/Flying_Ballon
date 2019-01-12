using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InClouds : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) { // if an object just flew into the clouds
		if (other.gameObject.CompareTag ("Balloon Top")) { // if was the balloon
			GameControl.inClouds = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) { // if an object just flew away from the clouds
		if (other.gameObject.CompareTag ("Balloon Top")) { // if was the balloon
			GameControl.inClouds = false;
		}
	}

}
