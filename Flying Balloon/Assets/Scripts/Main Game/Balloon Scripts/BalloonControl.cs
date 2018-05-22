using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour {

	public float thrust = 1.5f; // amount of upward force applied per second(equivalent of change in air density when heating up air inside the balloon)
	public float antigravity = 50; // upward force to act as a drag
	public float birdsExplosionRotation = -25; // angle at which birds will fly off after an explosion of the balloon
	public float distanceFromGround = 0.5f; // distance between bottom of the basket to the top of the ground, if lower than that the 'Ground score buster' is activated
	public float minModifier = 0.05f; // amount of score added during 'Ground score buster' depends on the distance from the ground, "minModifier" is minimal modifier

	public Rigidbody2D rbBalloon; // reference to rigidbody of the balloon object
	public Rigidbody2D rbBasket; // reference to rigidbody of the basket object
	public Rigidbody2D rbGround; // reference to rigidbody of a ground object

	public GameObject ExplosionPrefab; // reference to prefab of the explosion

	[HideInInspector]
	public AngleControl AngleControlScript; // reference to the AngleControl script - it simulates turbulence while the balloon is in clouds

	private bool nearGround = false; // if distance between the top of the ground and the bottom of the basket is lower than "distanceFromGround"

	private FixedJoint2D fixedConnection; // FixedJoint2D connection between the basket and the balloon
	private DistanceJoint2D distanceConnection; // the same as FixedJoint2D but DistanceJoint2D

	void Start () {
		AngleControlScript = gameObject.GetComponent<AngleControl> ();
		fixedConnection = rbBasket.GetComponent<FixedJoint2D> ();
		distanceConnection = rbBasket.GetComponent<DistanceJoint2D> ();

		freezeObject (true); // stabilize the balloon
	}

	void Update () {
		
		if (!GameControl.instance.gameEnded) { // if game isn't ended
			if (Input.GetButton ("Fire1")) { // if mouse button is being hold(or screen is being touched when on mobile)
				if (!GameControl.instance.gameStarted) { // if game hasn't begun yet but above is true
					GameControl.instance.beginGame ();
				}
				if (!GameControl.instance.gamePaused) // if game isn't being paused
					rbBalloon.AddForce (Vector2.up * thrust * Time.deltaTime); // apply force(upward * thrust * time it took to render this frame)
			}
			if (!GameControl.instance.gamePaused) // if game isn't being paused
				rbBalloon.AddForce (Vector2.up * antigravity * Time.deltaTime); // apply drag
		} else { // if the balloon was destroyed
			rbBalloon.velocity = Vector2.zero; // it HAS TO BE HERE, don't ask why(balloon animation isn't going to work without it)
			rbBalloon.AddForce (Vector2.up * antigravity * 0.7f * Time.deltaTime); // apply a little bit reduced drag to the balloon

			// destroy 'ground score buster' if it exists
			if (nearGround)
				SetScore.instance.deleteScore ("Ground");
			nearGround = false;
		}

		if (!GameControl.instance.gamePaused) { // if game hasn't been paused
			float groundHeight = rbGround.GetComponent<BoxCollider2D> ().size.y;
			float basketHeight = rbBasket.GetComponent<Renderer> ().bounds.size.y;

			float groundPosition = rbGround.position.y + groundHeight / 2f;
			float basketPosition = rbBasket.position.y - basketHeight / 2f;

			if (Mathf.Abs (basketPosition - groundPosition) <= distanceFromGround) { // if distance between the bottom of the basket and the top of the ground < "distanceFromGround"
				float modifier = 1f - Mathf.Abs (basketPosition - groundPosition) / distanceFromGround; // calculate the modifier(score added will change depending on the distance from the ground
				modifier = Mathf.Max (modifier, minModifier); // restrain values of modifier

				if (nearGround) // if it was previously 'near the ground'
					SetScore.instance.addScore ("Ground", modifier); // add score
				else
					SetScore.instance.setScore (0, "Ground"); // create new score entry
				
				nearGround = true;
			} else {
				if (nearGround) // if it was previously 'near the ground'
					SetScore.instance.deleteScore ("Ground"); // delete the ground score
				
				nearGround = false;
			}
		}

	}

	public void freezeObject (bool state) { // stabilize the balloon
		rbBalloon.isKinematic = state; // switches on/off ability to act on this object using forces
		rbBalloon.velocity = Vector2.zero;
		rbBalloon.angularVelocity = 0;

		rbBasket.isKinematic = state; // switches on/off ability to act on this object using forces
		rbBasket.velocity = Vector2.zero;
		rbBasket.angularVelocity = 0;
	}
		
	public void hardLanding (float duration) { // call this function if the balloon was damaged or it hit the ground(in other words the game is over)
		if (!(fixedConnection.enabled || distanceConnection.enabled)) { // if the balloon is disconnected from the basket
			explode (); // explode immediately
		} else { // if balloon is not disconnected from the basket
			// change the connection between them for better animation
			distanceConnection.enabled = true;
			fixedConnection.enabled = false;
		}

		// stop the movement and rotation of the whole balloon
		rbBalloon.velocity = Vector2.zero;
		rbBasket.velocity = Vector2.zero;
		rbBalloon.angularVelocity = 0;
		rbBasket.angularVelocity = -90f / (0.8f * duration); // set basket angular velocity
	}

	public void explode () { // make an explosion
		SpawnEnemies.letGoEnemies = false; // stop spawning enemies

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy"); // get all enemies
		foreach (GameObject enemy in enemies) {
			// simulate enemies flying away from the explosion
			enemy.transform.rotation = Quaternion.Euler (0, 0, birdsExplosionRotation); 
			enemy.GetComponent<Scroll> ().updateSpeed ();
		}

		Instantiate (ExplosionPrefab, rbBasket.gameObject.transform.position, Quaternion.Euler (-90, 0, 0)); // instantiate explosion
		Invoke ("disactivate", Time.deltaTime); // deactivate the balloon
	}

	private void disactivate () { // deactivates the balloon
		gameObject.SetActive (false);
	}

	public void breakRope () { // disconnect the balloon from the basket
		fixedConnection.enabled = false;
	}

}
