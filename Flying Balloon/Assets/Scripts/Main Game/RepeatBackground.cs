using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour {

	public string objectTag = "Ground";
	public float freeSpace = 0f;
	public float repositionOffset = 0f;
	public float fixMove = 0f; // 0 if not "intelligent" reposition
		
	private float groundLength; // lenght of the ground

	void Start () {
		if (fixMove == 0)
			groundLength = GetComponent<Renderer> ().bounds.size.x; // get lenght from the renderer
	}

	void Update () {
		if (fixMove == 0) {
			if (transform.position.x < -groundLength - freeSpace - repositionOffset) { // if the ground isn't on the screen
				RepositionBackground ();
			}
		} else {
			if (transform.position.x < -fixMove) { // if the ground isn't on the screen
				RepositionBackground ();
			}
		}
	}

	private void RepositionBackground () { // moves the background to the most left position
		GameObject[] grounds = GameObject.FindGameObjectsWithTag (objectTag); // get all grounds
		float furthestPosition = -100; // position of the most right ground
		foreach (GameObject ground in grounds) {
			furthestPosition = Mathf.Max (furthestPosition, ground.transform.position.x);
		}



		// set the position of this background to be on the right of the most right background
		if (fixMove == 0)
			transform.position = new Vector2 (furthestPosition + freeSpace + groundLength, transform.position.y);
		else
			transform.position = new Vector2 (furthestPosition + fixMove, transform.position.y);
	}

}
