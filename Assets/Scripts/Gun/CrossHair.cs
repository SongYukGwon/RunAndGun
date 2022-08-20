using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ũ�ν� ��� �Լ�
public class CrossHair : MonoBehaviour
{

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Animator animator;

    //ũ�ν���� ���¿� ���� ���� ��Ȯ��.
    private float gunAccuracy;
    
    //������ ũ�ν���� �Լ�
    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }

    //�۶� ũ�ν���� �Լ�
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    // �����Ҷ� ũ�ν���� �Լ�
    public void JumpAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }


    //�� Ŭ�ν���� �Լ�
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

    //��Ȳ������ ��Ȯ�� ��ȯ
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
