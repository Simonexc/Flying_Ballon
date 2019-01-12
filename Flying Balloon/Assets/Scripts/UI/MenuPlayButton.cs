using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayButton : MonoBehaviour {

	public void Restart()
    {
        GameControl.instance.restartGame();
    }
}
