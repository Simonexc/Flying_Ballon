using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

	public float scrollCoefficient = 1; // used for parallax effect
	private Rigidbody2D rbGround;

	// Use this for initialization
	void Start () {
		rbGround = GetComponent<Rigidbody2D> ();
		rbGround.velocity = new Vector2 (GameControl.scrollSpeed * scrollCoefficient, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
