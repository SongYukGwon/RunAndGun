using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField]
    private int damage;

    //���º���
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isAttack = false;
    private bool isDead = false;
    private float currentAttackSpeed = 0f;

    //�ʿ��� ������Ʈ
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider col;

    NavMeshAgent nav;
    private Transform target;

    //특정 레이어 지정
    int layerDead;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(target.position);
        if (walkSpeed <= 3f)
            isWalking = true;
        else
            isRunning = true;
        nav.speed = walkSpeed;
        layerDead = 9;
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
    }

    //범위안에 플레이어가 들어왔을때
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player" && currentAttackSpeed == 0 && !isDead)
        {
            StartCoroutine(Attack(collider));
        }
    }


    //공격 실행후 공격 쿨타임 세기
    IEnumerator Attack(Collider collider)
    {
        Debug.Log("Player Hit");
        nav.isStopped = true;
        collider.transform.gameObject.GetComponent<PlayerController>().Damaged(damage);
        anim.SetTrigger("Attack");
        isAttack = true;
        currentAttackSpeed = attackSpeed;
        while(currentAttackSpeed > 1.0f)
        {
            currentAttackSpeed -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        currentAttackSpeed = 0f;
        isAttack = false;
        nav.isStopped = false;
        yield break;
    }

    //죽지않거나 공격하지않으면 Move함수 실행
    protected void TryMove()
    {
        if (!isDead && !isAttack)
        {
            Move();
        }
    }

    //목표를 설정하는 함수
    protected void Move()
    {
        if (isRunning)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Walk", true);
        }
            nav.SetDestination(target.transform.position);
    }

    //데미지 받는 함수
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


    //죽는 함수
    protected void Dead()
    {
        gameObject.tag = "Dead";
        gameObject.layer = layerDead;
        isDead = true;
        //gameObject.transform.Find(
        //"Z_Head").gameObject.transform.position = new Vector3(0,1,0);
        anim.SetTrigger("DieFront");
        nav.isStopped = true;
        //Destroy(gameObject, 3f);
    }

}
