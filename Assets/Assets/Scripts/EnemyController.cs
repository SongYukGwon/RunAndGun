using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //적의 스탯
    //분리 필요성
    [SerializeField]
    private int hp;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private Vector3 destination;

    //상태변수
    private bool isWalking;
    private bool isRunning;
    private bool isAttack;
    private bool isDead = false;

    //필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider col;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
            //nav.SetDestination(transform.position + destination * 5f);
        }

    }

    public void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }
        }

    }

    protected void Dead()
    {
        anim.SetTrigger("DieBack");
    }

}
