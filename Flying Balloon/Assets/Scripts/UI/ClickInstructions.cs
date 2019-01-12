using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickInstructions : MonoBehaviour
{
    private bool animationPlayed = false;
    private bool animationEnded = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.gameStarted && animationPlayed && !animationEnded)
        {
            animationEnded = true;
            animator.SetInteger("State", 2);
            Invoke("Disable", 0.5f);
        }
        else if (!GameControl.gameStarted && !animationPlayed)
        {
            animationPlayed = true;
            animator.SetInteger("State", 1);
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
