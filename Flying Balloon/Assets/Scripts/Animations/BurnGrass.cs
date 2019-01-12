using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnGrass : MonoBehaviour {

    public float explosionRadius = 3;
    public float yDisplacement = 0.2f;

    public GameObject burningGrass;

    public void Burn(float x)
    {
        if (Mathf.Abs(transform.position.x-x) <= explosionRadius)
        {
            Vector3 pos = transform.position;
            pos.y += yDisplacement;
            GameObject burning = Instantiate(burningGrass, pos, Quaternion.Euler(0, 0, 0));
            burning.transform.parent = transform;

            Invoke("deactivate", 0.4f); // deactivate the grass
        }
    }

    private void deactivate()
    { // deactivates the grass
        GetComponent<Renderer>().enabled = false;
    }
}
