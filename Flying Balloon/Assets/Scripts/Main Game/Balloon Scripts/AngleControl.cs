using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleControl : MonoBehaviour {

	public Vector2 angleRange = new Vector2(5, 20); // how much the balloon can wiggle
	public Vector2 angularVelocityBase = new Vector2(10, 15); // minimal velocity range
	public Vector2 angularVelocityIncrease = new Vector2(20, 30); // maximal velocity range

	private float targetAngle = 0; // swing angle of the balloon
	private float targetSpeed = 0; // angular velocity of the balloon

	private Rigidbody2D rbBasket; // reference to "Rigidbody2D" of the basket
	private Transform transBasket; // reference to "Transform" of the basket

	private BalloonControl BalloonControlScript; // reference to "BalloonControl" script

	void Start () {
		BalloonControlScript = gameObject.GetComponent<BalloonControl> ();
		rbBasket = BalloonControlScript.rbBasket;
		transBasket = rbBasket.gameObject.transform;
	}

	public void rotate (float coefficient) { // simulate turbulence
		if (coefficient == 0) { // if it is not in 'turbulence region'
			
			if (Mathf.Abs( getAngle (transBasket.eulerAngles.z)) <= 1) { // if the deviation from the origin is less than 1 degree
				// set the balloon to the origin
				rbBasket.angularVelocity = 0;
				transBasket.rotation = Quaternion.Euler (0, 0, 0);
				targetAngle = 0;

			} else if (targetAngle != 0) {
				// swing back to the origin
				targetAngle = 0;
				changeAngularVelocity (targetAngle, coefficient);
			}

		} else { // if it is in 'turbulence region'
			//Debug.Log(rbBasket.angularVelocity);
			if (rbBasket.angularVelocity < 0.5f * targetSpeed) // if "angularVelocity" droped lower than half of "targetSpeed"
				rbBasket.angularVelocity = targetSpeed; // set "angularVelocity" back to "targetSpeed"
			
			if (Mathf.Sign (targetAngle) * getAngle (transBasket.eulerAngles.z) >= Mathf.Abs (targetAngle)) { // if the balloon's swing angle is bigger than "targetAngle"
				// set new "targetAngle" and new "angularVelocity"
				targetAngle = -Mathf.Sign (targetAngle) * Random.Range (angleRange.x * coefficient, angleRange.y * coefficient);
				changeAngularVelocity (targetAngle, coefficient);
			}

			if (targetAngle == 0) { 
				targetAngle = randomSign () * Random.Range (angleRange.x * coefficient, angleRange.y * coefficient);
				changeAngularVelocity (targetAngle, coefficient);
			}

		}
	}

	private void changeAngularVelocity (float targetAngle, float coefficient) { // set new angular velocity of the balloon
		// formula: direction of movement(-1, 0 or 1) * random value from[ base angular velocity(min) * added angular velocity(min) * "coefficient" ; base angular velocity(max) * added angular velocity(max) * "coefficient" ]
		targetSpeed = Mathf.Sign (targetAngle - getAngle (transBasket.eulerAngles.z)) * 
			Random.Range (angularVelocityBase.x + angularVelocityIncrease.x*coefficient, angularVelocityBase.y + angularVelocityIncrease.y*coefficient);
		rbBasket.angularVelocity = targetSpeed;
	}

	private float getAngle (float angle) { // set "angle" to desired form, -180 <= "angle" <= 180
		if (angle <= 180) {
			return angle;
		} else {
			return angle - 360;
		}
	}

	public static int randomSign () { // returns either 1 or -1 at random
		return Random.value < 0.5? 1 : -1;
	}

}
