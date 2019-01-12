using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour {
    private GameObject SecondObject; // next object with the same tag

    void Start () {
        GameObject[] Objects = GameObject.FindGameObjectsWithTag (gameObject.tag); // get all objects
        if (Objects[0] == this.gameObject)
        {
            SecondObject = Objects[1];
        }
        else
        {
            SecondObject = Objects[0];
        }
        
			
	}

	void Update () {
        if (transform.position.x < -GameControl.instance.screenSize) // if it's not on the screen
        {
            transform.position = new Vector2(SecondObject.transform.position.x + GameControl.instance.screenSize, transform.position.y);
        }

        if (GetComponent<Scroll>() == null)
        {
            if (SecondObject.transform.position.x > transform.position.x)
            {
                transform.position = new Vector2(SecondObject.transform.position.x - GameControl.instance.screenSize, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(SecondObject.transform.position.x + GameControl.instance.screenSize, transform.position.y);
            }
        }
    }
    
}
