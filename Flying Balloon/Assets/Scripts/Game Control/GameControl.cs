using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	public static GameControl instance;
    
	public static bool gamePaused = true;
	public static bool gameEnded = true;
	public static bool gameStarted = true;
    public static bool inClouds = false; // did the balloon enter the clouds

	public float scrollSpeed = -1.5f; // speed at which the ground is moving

    public float screenSize = 25.6f;
    
    public static float screenWidth;
    public static float screenHeight;

    public string[] movingObjectsNames = { };

    public GameObject Ground;
	public GameObject Balloon;

    public BoxCollider2D basketCollider;

	[HideInInspector]
	public BalloonControl BalloonScript;

    //private bool menuShown = false;
    private bool landed = false;

	private GameObject[][] MovingObjects;

	void Awake () { // called before Start
		instance = this;
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
    }

	void Start () {
		gamePaused = true;
        
		BalloonScript = Balloon.GetComponent<BalloonControl> ();

        MovingObjects = new GameObject[movingObjectsNames.Length][];
        for (int i = 0; i < movingObjectsNames.Length; i++)
        {
            MovingObjects[i] = GameObject.FindGameObjectsWithTag(movingObjectsNames[i]);
        }
	}
    /*
    public void updateGrounds(float speedModifier, bool forced = false) { // updates the velocity of all grounds
        foreach (GameObject[] MovingGroup in MovingObjects)
        {
            foreach (GameObject MovingObject in MovingGroup)
            {
                if (MovingObject.GetComponent<Scroll>() != null)
                    MovingObject.GetComponent<Scroll>().updateSpeed(speedModifier, forced);
            }
        }
    }
    */
    public void moveAllScrollingObjects(float speedModifier, bool forced = false)
    {
        foreach (Scroll ScrollObject in FindObjectsOfType<Scroll>())
        {
            ScrollObject.updateSpeed(speedModifier, forced);
        }
    }

    public void stopGame () { // end game
		gameEnded = true;
		pauseGame ();
	}

	public void hitGround () { // if the balloon hit the ground
		if (!landed) {
            landed = true;
            GetComponent<EndScreenTransitions>().landed = true;
			BalloonScript.hardLanding();
		}
	}

	public void pauseGame () {
        SpawnEnemies.letGoEnemies = false; // start spawning enemies
        gamePaused = true;
		if (!gameEnded) {
            BalloonScript.freezeObject(true);
            //updateGrounds (0, true); // freeze the grounds
            moveAllScrollingObjects(0, true);
		}
        
	}

	public void beginGame () { // start the game
		gameStarted = true;
		resumeGame ();
	}

	public void resumeGame () {
        gamePaused = false;
		BalloonScript.freezeObject (false);
        SpawnEnemies.letGoEnemies = true; // start spawning enemies
        moveAllScrollingObjects(1);
        //updateGrounds (1); // move the grounds
    }

	public void restartGame () {
        Balloon.GetComponent<Scroll>().affected = true;
        GetComponent<MenuManager>().hideMenu();
        GetComponent<EndScreenTransitions>().Restart();
	}

    public float distanceToGround()
    {
        BoxCollider2D groundCollider = Ground.GetComponent<BoxCollider2D>();

        float groundHeight = groundCollider.size.y;
        float groundOffset = groundCollider.offset.y;

        float basketHeight = basketCollider.size.y;
        float basketOffset = basketCollider.offset.y;

        float balloonScale = Balloon.transform.localScale.x;

        float groundPosition = Ground.transform.position.y + groundOffset + groundHeight / 2f;
        float basketPosition = BalloonScript.Bottom.transform.position.y + (basketOffset - basketHeight / 2f) * balloonScale;

        return Mathf.Max(basketPosition - groundPosition, 0);
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload the scene
    }

    public static float mapVal(float val, float max) // val <= max && val >= 0
    {
        return Mathf.Min(Mathf.Max(0, val), max) / max;
    }
}
