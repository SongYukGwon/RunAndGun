using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    //크로스헤어 상태에 따른 총의 정확도.
    private float gunAccuracy;

    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }
    public void JumpAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
        {
            animator.SetTrigger("Walk_Fire");
        }
        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("Crouch_Fire");
        }
        else
        {
            animator.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
            gunAccuracy = 0.03f;
        else if (animator.GetBool("Running"))
            gunAccuracy = 0.6f;
        else
            gunAccuracy = 0.01f;
        return gunAccuracy;
    }


}
