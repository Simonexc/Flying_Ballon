using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour {

    public float birdsExplosionRotation = -25; // angle at which birds will fly off after an explosion of the balloon

    public GameObject ExplosionPrefab; // reference to prefab of the explosion

    private BalloonControl BalloonControlScript; // reference to the BalloonControl script - contains all important data about balloon

    // Use this for initialization
    void Start () {
        BalloonControlScript = GetComponent<BalloonControl>();
	}

    public void explode()
    { // make an explosion
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // get all enemies
        foreach (GameObject enemy in enemies)
        {
            // simulate enemies flying away from the explosion
            enemy.GetComponent<Animator>().SetBool("Exploded", true);
            enemy.GetComponent<Scroll>().updateSpeed();
        }

        Vector3 pos = BalloonControlScript.Bottom.transform.position;
        pos.z = -3;
        if (GameControl.instance.distanceToGround() < 0.2f)
        { // if explosion occured close to the ground
            GameObject[] Grass1 = GameObject.FindGameObjectsWithTag("Grass Blade 1");
            GameObject[] Grass2 = GameObject.FindGameObjectsWithTag("Grass Blade 2");
            foreach (GameObject Grass in Grass1)
            {
                Grass.GetComponent<BurnGrass>().Burn(transform.position.x);
            }
            foreach (GameObject Grass in Grass2)
            {
                Grass.GetComponent<BurnGrass>().Burn(transform.position.x);
            }
        }
        GameObject Explosion = Instantiate(ExplosionPrefab, pos, Quaternion.Euler(-90, 0, 0)); // instantiate explosion
        Explosion.transform.parent = GameControl.instance.Ground.transform;

        Invoke("deactivate", Time.deltaTime); // deactivate the balloon
    }

    private void deactivate()
    { // deactivates the balloon
        gameObject.SetActive(false);
    }

}
