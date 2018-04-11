using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour {

	private float groundLength;

	void Start () {
		groundLength = GetComponent<Renderer> ().bounds.size.x;
	}

	void Update () {
		if (transform.position.x < -groundLength) {
			RepositionBackground ();
		}
	}

	private void RepositionBackground () {
		GameObject[] grounds = GameObject.FindGameObjectsWithTag ("Ground");
		float furthestPosition = -100;
		foreach (GameObject ground in grounds) {
			furthestPosition = Mathf.Max (furthestPosition, ground.transform.position.x);
		}

		transform.position = new Vector2 (furthestPosition + groundLength, transform.position.y);
	}

}
