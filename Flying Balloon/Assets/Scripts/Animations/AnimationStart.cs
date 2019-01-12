using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour {

    public float startTime = 0;

    private Animator objectAnimator;
    private AnimationClip objectAnimation;

    // Use this for initialization
    void Start () {
        objectAnimator = GetComponent<Animator>();
        objectAnimation = objectAnimator.runtimeAnimatorController.animationClips[0];

        objectAnimator.Play(objectAnimation.name, -1, startTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
