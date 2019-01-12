using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

	public float destroyPosition = -10f; // position at which the object will be destroyed

	void Update () {
		if (transform.position.x <= destroyPosition) { // the x-position of the object is less than "destroyPosition"
			Destroy (this.gameObject); // destroy this object
		}
	}
}
