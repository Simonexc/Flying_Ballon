using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

	public float destroyPosition = -10f;

	void Update () {
		if (transform.position.x <= destroyPosition || GameControl.instance.gameEnded) {
			Destroy (this.gameObject);
		}
	}
}
