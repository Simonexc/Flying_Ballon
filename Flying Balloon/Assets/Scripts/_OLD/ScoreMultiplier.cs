using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour {

    public float activationPoint = 0.5f; // distance between bottom of the basket to the top of the ground, if lower than that the 'Ground score buster' is activated
    public float minModifier = 0.05f; // amount of score added during 'Ground score buster' depends on the distance from the ground, "minModifier" is minimal modifier

    public GameObject Ground;

    public BoxCollider2D basketCollider;

    private BalloonControl BalloonControlScript; // reference to the BalloonControl script - contains all important data about balloon

    private bool nearGround = false; // if distance between the top of the ground and the bottom of the basket is lower than "distanceFromGround"

    // Use this for initialization
    void Start () {
        BalloonControlScript = GetComponent<BalloonControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameControl.gamePaused)
        { // if the game hasn't been paused
            float distance = distanceToGround();

            if (distance <= activationPoint)
            { // if distance between the bottom of the basket and the top of the ground < "distanceFromGround"
                float modifier = 1f - distance / activationPoint; // calculate the modifier(score added will change depending on the distance from the ground)
                modifier = Mathf.Max(modifier, minModifier); // restrain values of modifier

                if (nearGround) // if it was previously 'near the ground'
                    SetScore.instance.addScore("Ground", modifier); // add score
                else
                    SetScore.instance.setScore(0, "Ground"); // create new score entry

                nearGround = true;
            }
            else
            {
                if (nearGround) // if it was previously 'near the ground'
                    SetScore.instance.deleteScore("Ground"); // delete the ground score

                nearGround = false;
            }
        }
    }

    public float distanceToGround()
    {
        BoxCollider2D groundCollider = Ground.GetComponent<BoxCollider2D>();

        float groundHeight = groundCollider.size.y;
        float groundOffset = groundCollider.offset.y;

        float basketHeight = basketCollider.size.y;
        float basketOffset = basketCollider.offset.y;

        float balloonScale = transform.localScale.x;

        float groundPosition = Ground.transform.position.y + groundOffset + groundHeight / 2f;
        float basketPosition = BalloonControlScript.Bottom.transform.position.y + (basketOffset - basketHeight / 2f) * balloonScale;

        return Mathf.Max(basketPosition - groundPosition, 0);
    }

    public bool getNearGroundVariable()
    {
        return nearGround;
    }

    public void SetNearGroundVariable(bool val)
    {
        nearGround = val;
    }
}
