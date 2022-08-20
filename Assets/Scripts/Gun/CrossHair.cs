using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//크로스 헤어 함수
public class CrossHair : MonoBehaviour
{

    //필요한 컴포넌트
    [SerializeField]
    private Animator animator;

    //크로스헤어 상태에 따른 총의 정확도.
    private float gunAccuracy;
    
    //걸을때 크로스헤어 함수
    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }

    //뛸때 크로스헤어 함수
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    // 점프할때 크로스헤어 함수
    public void JumpAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }


    //쏠때 클로스헤어 함수
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

    //상황에따른 정확도 반환
    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
            gunAccuracy = 0.03f;
        else if (animator.GetBool("Running"))
            gunAccuracy = 0.1f;
        else
            gunAccuracy = 0.01f;
        return gunAccuracy;
    }


}
