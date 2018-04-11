using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleControl : MonoBehaviour {

	public float angleRange = 10;

	public Vector2 angularVelocityBase = new Vector2(10, 15);
	public Vector2 angularVelocityIncrease = new Vector2(20, 30);

	private float targetAngle = 0;

	private Rigidbody2D rb;
	private Transform trans;

	private BalloonControl BalloonControlScript;

	void Start () {
		BalloonControlScript = gameObject.GetComponent<BalloonControl> ();
		rb = BalloonControlScript.rbBasket;
		trans = rb.gameObject.transform;
	}

	public void rotate (float coefficient) { // simulate turbulence
		if (coefficient == 0) {
			
			if (Mathf.Abs( getAngle (trans.eulerAngles.z)) <= 1) {
				rb.angularVelocity = 0;
				trans.rotation = Quaternion.Euler (0, 0, 0);
				targetAngle = 0;

			} else if (targetAngle != 0) {
				targetAngle = 0;
				changeAngularVelocity (targetAngle, coefficient);
			}

		} else {
			
			if (Mathf.Sign (targetAngle) * getAngle (trans.eulerAngles.z) >= Mathf.Abs (targetAngle)) {
				targetAngle = -Mathf.Sign (targetAngle) * Random.Range (0, angleRange * coefficient);
				changeAngularVelocity (targetAngle, coefficient);
			}

			if (getAngle (trans.eulerAngles.z) == 0 && rb.angularVelocity == 0) {
				targetAngle = Random.Range (-angleRange * coefficient, angleRange * coefficient);
				changeAngularVelocity (targetAngle, coefficient);
			}

		}
	}

	private void changeAngularVelocity (float targetAngle, float coefficient) {
		rb.angularVelocity = Mathf.Sign (targetAngle - getAngle (trans.eulerAngles.z)) * Random.Range (angularVelocityBase.x + angularVelocityIncrease.x*coefficient, angularVelocityBase.y + angularVelocityIncrease.y*coefficient);
	}

	private float getAngle (float angle) {
		if (angle <= 180) {
			return angle;
		} else {
			return angle - 360;
		}
	}

}
