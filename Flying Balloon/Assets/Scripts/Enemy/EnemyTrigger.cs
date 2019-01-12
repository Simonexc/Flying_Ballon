using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour {

    public Vector3 scoreDisplacement = new Vector3(0.5f, 0.5f, 0);

	public GameObject FeatherExplosionPrefab; // referenece to the 'feather' explosion prefab
    
	private bool getScore = true;

	void OnTriggerEnter2D (Collider2D other) { // if some object hit the enemy from the top
        if (getScore) {
            if (other.gameObject.CompareTag("Balloon Bottom")) { // if it's the basket
                ScoreManager.instance.AddScore(ScoreManager.instance.hitBirdScore, other.transform.position + scoreDisplacement);
                DestroyThisObject(); // destroy this enemy
            } else if (other.gameObject.CompareTag("Balloon Top"))
            {
                GameControl.instance.stopGame();
                DestroyThisObject(); // destroy this enemy
            } else if (other.gameObject.CompareTag("Fuel Tank"))
            {
                GameControl.instance.stopGame();
                GameControl.instance.hitGround(); // simulate hitting the ground
                GameControl.instance.BalloonScript.ExplosionsScript.explode();
                DestroyThisObject(); // destroy this enemy
            }
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
