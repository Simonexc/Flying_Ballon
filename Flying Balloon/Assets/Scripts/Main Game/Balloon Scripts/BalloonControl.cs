using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour {

	public float thrust = 1.5f;
	public float antigravity = 50f;

	public Rigidbody2D rbBalloon;
	public Rigidbody2D rbBasket;

	public GameObject ExplosionPrefab;

	[HideInInspector]
	public AngleControl AngleControlScript;

	private FixedJoint2D fixedConnection;
	private DistanceJoint2D distanceConnection;

	void Start () {
		AngleControlScript = gameObject.GetComponent<AngleControl> ();
		fixedConnection = rbBasket.GetComponent<FixedJoint2D> ();
		distanceConnection = rbBasket.GetComponent<DistanceJoint2D> ();

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
		Invoke ("disactivate", Time.deltaTime);
	}

	private void disactivate () {
		gameObject.SetActive (false);
	}

	public void breakRope () {
		fixedConnection.enabled = false;
	}

}
