using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleControl : MonoBehaviour {

	public Vector2 angleRange = new Vector2(5, 20); // how much the balloon can wiggle
    public Vector2 timeVariation = new Vector2(0.8f, 1.5f); // time it takes to perform a swing(from 0 to some value and back to 0)    

    public AnimationCurve velocityCurve; // swing movement
    public AnimationCurve distributionCurve; // how wiggle changes with height

    [HideInInspector]
    public float coefficient = 0;

    private float maxVelocity = 0; // avg velocity is half the max velocity
    private float duration = 0.01f;
    private float timePassed = 0;

    private GameObject Top;

    void Start()
    {
        Top = GetComponent<BalloonControl>().Top;
    }

    void Update()
    {
        if (!GameControl.gamePaused)
        {
            timePassed = Mathf.Min(timePassed + Time.deltaTime, duration); // restrain timePassed <= duration
            Vector3 rotation = new Vector3(0, 0, maxVelocity * velocityCurve.Evaluate(timePassed / duration) * Time.deltaTime);
            transform.Rotate(rotation);

            if (coefficient == 0 && Mathf.Abs(transform.rotation.eulerAngles.z) <= 0.2f)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
                duration = 0.01f;
                timePassed = 0;
                maxVelocity = 0;
            }

            if (timePassed == duration && Top.activeSelf)
            {
                duration = timeVariation.x + (timeVariation.y - timeVariation.x) * distributionCurve.Evaluate(coefficient);
                if (coefficient == 0)
                {
                    maxVelocity = 2 * -getAngle(transform.rotation.eulerAngles.z) / duration;
                    timePassed = 0;
                }
                else
                {
                    float targetAngle = angleRange.x + (angleRange.y - angleRange.x) * distributionCurve.Evaluate(coefficient);
                    targetAngle *= -Mathf.Sign(getAngle(transform.rotation.eulerAngles.z));
                    maxVelocity = 2 * -(getAngle(transform.rotation.eulerAngles.z) - targetAngle) / duration;
                    timePassed = 0;
                }
            }
        }
    }

    private float getAngle(float angle)
    { // set "angle" to desired form, -180 <= "angle" <= 180
        if (angle <= 180)
        {
            return angle;
        }
        else
        {
            return angle - 360;
        }
    }

}
