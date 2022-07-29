using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //���� ����
    //�и� �ʿ伺
    [SerializeField]
    private int hp;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private Vector3 destination;

    //���º���
    private bool isWalking;
    private bool isRunning;
    private bool isAttack;
    private bool isDead = false;

    //�ʿ��� ������Ʈ
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
        gameObject.tag = "Dead";
        //gameObject.transform.Find("Z_Head").gameObject.transform.position = new Vector3(0,1,0);
        anim.SetTrigger("DieFront");
        Destroy(gameObject, 3f);
    }

}
