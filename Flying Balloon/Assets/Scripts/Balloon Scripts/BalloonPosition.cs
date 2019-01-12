using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPosition : MonoBehaviour
{
    public float targetPositionFromEdge = 3;
    public float startDistanceFromEdge = -1;
    public float transitionTime = 1;
    public AnimationCurve transitionCurve;

    private bool ended = false;
    private float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timePassed < transitionTime)
           {
            timePassed += Time.deltaTime;

            float delta = Mathf.Abs(targetPositionFromEdge - startDistanceFromEdge);
            float startPosition = -GameControl.screenWidth / 2 + startDistanceFromEdge;
            float newX = startPosition + delta * transitionCurve.Evaluate(GameControl.mapVal(timePassed, transitionTime));

            Vector3 newPosition = transform.position;
            newPosition.x = newX;

            transform.position = newPosition;
        }
        else if (!ended)
        {
            GameControl.gameStarted = false;
            GameControl.gameEnded = false;
            ended = true;
        }
    }
}
