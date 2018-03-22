using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour {

	public static BalloonControl instance;

	public float thrust = 1.5f;
	public float angleRange = 10;

	public Vector2 angularVelocityBase = new Vector2(10, 15);
	public Vector2 angularVelocityIncrease = new Vector2(20, 30);
	[HideInInspector]
	public Vector2 startPosition;
	[HideInInspector]
	public Rigidbody2D rbBalloon;

	private float targetAngle = 0;

	// Use this for initialization
	void Start () {
		instance = this;
		startPosition = this.transform.position;
		rbBalloon = GetComponent<Rigidbody2D> ();
		freezeObject (true);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1")) {
			if (!GameControl.instance.gameStarted) {
				GameControl.instance.beginGame ();
			}
			rbBalloon.AddForce(Vector2.up * thrust * Time.deltaTime);
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		GameControl.instance.stopGame ();
	}

	public void freezeObject (bool state) {
		rbBalloon.isKinematic = state;
	}

	public void bringItBack () {
		transform.position = startPosition;
		transform.rotation = Quaternion.Euler (0, 0, 0);
		rbBalloon.angularVelocity = 0;
		rbBalloon.velocity = Vector2.zero;
	}

	public void rotate (float coefficient) {
		if (coefficient == 0) {
			if (Mathf.Abs( getAngle (transform.eulerAngles.z)) <= 1) {
				rbBalloon.angularVelocity = 0;
				transform.rotation = Quaternion.Euler (0, 0, 0);
				targetAngle = 0;
			} else if (targetAngle != 0) {
				targetAngle = 0;
				changeAngularVelocity (targetAngle, coefficient);
			}
		} else {
			if (Mathf.Sign (targetAngle) * getAngle (transform.eulerAngles.z) >= Mathf.Abs (targetAngle)) {
				targetAngle = -Mathf.Sign (targetAngle) * Random.Range (0, angleRange * coefficient);
				changeAngularVelocity (targetAngle, coefficient);
			}

			if (getAngle (transform.eulerAngles.z) == 0 && rbBalloon.angularVelocity == 0) {
				targetAngle = Random.Range (-angleRange * coefficient, angleRange * coefficient);
				changeAngularVelocity (targetAngle, coefficient);
			}
		}
	}

	private void changeAngularVelocity (float targetAngle, float coefficient) {
		rbBalloon.angularVelocity = Mathf.Sign (targetAngle - getAngle (transform.eulerAngles.z)) * Random.Range (angularVelocityBase.x + angularVelocityIncrease.x*coefficient, angularVelocityBase.y + angularVelocityIncrease.y*coefficient);
	}

	private float getAngle (float angle) {
		if (angle <= 180) {
			return angle;
		} else {
			return angle - 360;
		}
	}
}
