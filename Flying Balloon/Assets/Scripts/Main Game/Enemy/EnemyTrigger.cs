using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour {

	public GameObject FeatherExplosionPrefab; // referenece to the 'feather' explosion prefab

	//private GameObject ExplosionInstance; // variable to store the explosion object in order to delete it later
	private bool getScore = true;

	void OnCollisionEnter2D (Collision2D other) { // if enemy is in collision with an object
		if ((other.gameObject.CompareTag ("Balloon Top") || other.gameObject.CompareTag ("Balloon Bottom")) && getScore) { // if it's the balloon
			// end the game and destroy this enemy
			GameControl.instance.stopGame ();
			DestroyThisObject ();
		}
	}

	void OnTriggerEnter2D (Collider2D other) { // if some object hit the enemy from the top
		if (other.gameObject.CompareTag ("Balloon Bottom") && getScore) { // if it's the basket
			SetScore.instance.setScore (SetScore.instance.instantaneousScore ["Enemy"]); // add score for hitting an enemy
			DestroyThisObject (); // destroy this enemy
		}
	}

	public void DestroyThisObject () { // destroys this enemy object and launches the explosion effect
		getScore = false;
		GetComponent<Renderer>().enabled = false; // hide this object
		Vector3 position = new Vector3 (transform.position.x, transform.position.y, -5);
		Instantiate (FeatherExplosionPrefab, position, Quaternion.identity); // toggle the explosion

		Invoke ("DestroyObject", Time.deltaTime); // wait a little bit
	}

	private void DestroyObject () {
		Destroy (this.gameObject);
	}

}
