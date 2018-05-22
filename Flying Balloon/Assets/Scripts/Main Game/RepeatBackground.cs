using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour {

	private float groundLength; // lenght of the ground

	void Start () {
		groundLength = GetComponent<Renderer> ().bounds.size.x; // get lenght from the renderer
	}

	void Update () {
		if (transform.position.x < -groundLength) { // if the ground isn't on the screen
			RepositionBackground ();
		}
	}

	private void RepositionBackground () { // moves the background to the most left position
		GameObject[] grounds = GameObject.FindGameObjectsWithTag ("Ground"); // get all grounds
		float furthestPosition = -100; // position of the most right ground
		foreach (GameObject ground in grounds) {
			furthestPosition = Mathf.Max (furthestPosition, ground.transform.position.x);
		}

		// set the position of this background to be on the right of the most right background
		transform.position = new Vector2 (furthestPosition + groundLength, transform.position.y);
	}

}
