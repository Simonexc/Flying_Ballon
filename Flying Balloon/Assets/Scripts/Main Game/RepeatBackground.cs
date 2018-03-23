using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour {

	private float groundLength;

	private BoxCollider2D groundCollider;

	// Use this for initialization
	void Start () {
		groundCollider = GetComponent<BoxCollider2D> ();
		groundLength = groundCollider.size.x * transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < -groundLength) {
			RepositionBackground ();
		}
	}

	private void RepositionBackground () {
		Vector2 newPosition = new Vector2 (transform.position.x + groundLength * 2f, transform.position.y);
		transform.position = newPosition;
	}
}
