using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMultiplier : MonoBehaviour {

    public static CloudsMultiplier instance;

    public float cloudsHeight = 1; // the height of clouds
    public float destroyDistance = 2; // destroy the score if balloon is further from clouds than this distance
    public Vector3 scoreDisplacement = new Vector3(0.5f, -0.5f, 0);

    [HideInInspector]
    public float activationPoint = 0; // at what position the turbulences will appear

    [HideInInspector]
    public GameObject CloudsScore = null;

    private float maxPoint = 0; // position of the end of clouds
    private float cloudsScore = 0;

    private AnimationCurve scoreDistribution; // how 'score buster' changes with height

    private BalloonControl BalloonControlScript;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        BalloonControlScript = GetComponent<BalloonControl>();
        scoreDistribution = GetComponent<AngleControl>().distributionCurve;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameControl.gamePaused)
        {
            if (GameControl.inClouds)
            { // if the balloon is in clouds
                if (activationPoint == 0)
                { // if "activationPoint" isn't set
                    activationPoint = transform.position.y - 0.02f; // set activation point
                    maxPoint = activationPoint + cloudsHeight; // set maximal point
                }
                float a = transform.position.y - activationPoint;
                float b = maxPoint - activationPoint;
                // restrain values of a
                float val = Mathf.Min(b, Mathf.Max(a, 0)) / b;

                BalloonControlScript.AngleControlScript.coefficient = val; // wiggle the balloon

                if (val < 1)
                { // if clouds 'score buster' is on
                    if (CloudsScore == null)
                    {
                        Vector3 position = new Vector3(transform.position.x, activationPoint, 0) + scoreDisplacement;
                        CloudsScore = ScoreManager.instance.CreateScore(position, false);
                    }
                    float scoreAdd = ScoreManager.instance.inCloudsScorePerSecond * scoreDistribution.Evaluate(val) * Time.deltaTime;
                    cloudsScore += scoreAdd;
                    ScoreManager.instance.Add(scoreAdd);
                    ScoreManager.instance.UpdateScore(CloudsScore, Mathf.CeilToInt(cloudsScore));
                }


                if (val == 1)
                { // if the balloon hit the top of the clouds
                  // end the game
                    if (CloudsScore != null)
                        CloudsScore.GetComponent<SelfDestruct>().Destroy();
                    CloudsScore = null;
                    cloudsScore = 0;
                    GameControl.instance.stopGame();
                    BalloonControlScript.hideBalloon(); // hides top part of the balloon
                }
            }
            else
            {
                BalloonControlScript.AngleControlScript.coefficient = 0; // stop wiggling the balloon
                
                if (Mathf.Abs(activationPoint - transform.position.y) > destroyDistance) // if the balloon is far enough from the clouds
                {
                    if (CloudsScore != null)
                        CloudsScore.GetComponent<SelfDestruct>().Destroy();
                    CloudsScore = null;
                    cloudsScore = 0;
                }
            }

        }
    }
}
