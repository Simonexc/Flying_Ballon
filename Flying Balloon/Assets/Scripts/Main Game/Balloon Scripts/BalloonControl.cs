using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour {

	public float thrust = 1.5f;
	public float antigravity = 50f;

	public Rigidbody2D rbBalloon;
	public Rigidbody2D rbBasket;
	public FixedJoint2D fixedConnection;
	public DistanceJoint2D distanceConnection;
	public FixedJoint2D rope1;
	public FixedJoint2D rope2;

	public GameObject ExplosionPrefab;

	[HideInInspector]
	public AngleControl AngleControlScript;

	void Start () {
		AngleControlScript = gameObject.GetComponent<AngleControl> ();
		freezeObject (true);
	}

	void Update () {
		
		if (!GameControl.instance.gameEnded) {
			if (Input.GetButton ("Fire1")) {
				if (!GameControl.instance.gameStarted) {
					GameControl.instance.beginGame ();
				}
				rbBalloon.AddForce (Vector2.up * thrust * Time.deltaTime);
			}
			rbBalloon.AddForce (Vector2.up * antigravity * Time.deltaTime);
		} else {
			rbBalloon.velocity = Vector2.zero;
			rbBalloon.AddForce (Vector2.up * antigravity * 0.7f * Time.deltaTime);
		}

	}

	public void freezeObject (bool state) {
		rbBalloon.isKinematic = state;
		rbBalloon.velocity = Vector2.zero;
		rbBalloon.angularVelocity = 0;

		rbBasket.isKinematic = state;
		rbBasket.velocity = Vector2.zero;
		rbBasket.angularVelocity = 0;
	}
		
	public void hardLanding (float duration) {
		if (!(fixedConnection.enabled || distanceConnection.enabled)) {
			explode ();
		} else {
			distanceConnection.enabled = true;
			fixedConnection.enabled = false;
		}

		rbBalloon.velocity = Vector2.zero;
		rbBasket.velocity = Vector2.zero;
		rbBalloon.angularVelocity = 0;
		rbBasket.angularVelocity = -90f / (0.8f * duration);
	}

	public void explode () {
		Instantiate (ExplosionPrefab, rbBasket.gameObject.transform.position, Quaternion.Euler (-90, 0, 0));
		gameObject.SetActive (false);
	}

	public void breakRope () {
		fixedConnection.enabled = false;
		rope1.enabled = false;
		rope2.enabled = false;
	}

}
