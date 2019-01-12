using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float timeout = 2;
    public float duration = 1;

    private bool destructionBegan = false;

	// Use this for initialization
	void Start () {
        if (duration != 0)
        {
            Invoke("Destroy", duration);
        }
	}

    void Update()
    {
        if (GameControl.gameEnded)
            Destroy();
    }

    public void Destroy()
    {
        if (!destructionBegan)
        {
            destructionBegan = true;
            GetComponent<Animator>().SetBool("End", true);
            Destroy(this.gameObject, timeout);
        }
    }

}
