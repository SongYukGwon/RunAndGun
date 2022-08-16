using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private PlayerStat thePlayerStat;

    NavMeshAgent nav;
    private Transform target;

    //특정 레이어 지정
    int layerDead;

    //좀비 정보
    [SerializeField]
    private ZombieInfo currentZombie;

    //좀비의 필요한 컴포넌트
    private Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider col;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        thePlayerStat = GameObject.Find("Stat").GetComponent<PlayerStat>();
        nav = GetComponent<NavMeshAgent>();
        anim = currentZombie.GetComponent<Animator>();
        rigid = currentZombie.GetComponent<Rigidbody>();
        col = currentZombie.GetComponent<CapsuleCollider>();

        nav.SetDestination(target.position);
        SetZombieWalkORRun();
        layerDead = 9;
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
        TryAttack();
        CalAttackSpeed();
    }

    private void SetZombieWalkORRun()
    {
        if (currentZombie.walkSpeed <= 3f)
            currentZombie.isWalking = true;
        else
            currentZombie.isRunning = true;
        nav.speed = currentZombie.walkSpeed;
    }

    private void CalAttackSpeed()
    {
        if(currentZombie.currentAttackSpeed > 0f)
            currentZombie.currentAttackSpeed -= Time.deltaTime;
    }

    //범위안에 플레이어가 들어왔을때 <수정필요>


    private void TryAttack()
    {
        if(Vector3.Distance(transform.position, target.position)<= 2.0f && currentZombie.currentAttackSpeed <= 0f)
        {
            currentZombie.currentAttackSpeed = currentZombie.attackSpeed;
            StartCoroutine(Attack(target.GetComponent<Collider>()));
        }
    }
        
   


    //공격 실행후 공격 쿨타임 세기
    IEnumerator Attack(Collider collider)
    {
        nav.isStopped = true;
        anim.SetTrigger("Attack");
        currentZombie.isAttack = true;
        yield return new WaitForSeconds(0.5f);
        if (Vector3.Distance(transform.position, target.position) <= 2.0f)
            collider.transform.gameObject.GetComponent<PlayerController>().Damaged(currentZombie.damage);
        currentZombie.isAttack = false;
        nav.isStopped = false;
        yield break;
    }

    //죽지않거나 공격하지않으면 Move함수 실행
    protected void TryMove()
    {
        if (!currentZombie.isDead && !currentZombie.isAttack)
        {
            Move();
        }
    }

    //목표를 설정하는 함수
    protected void Move()
    {
        if (currentZombie.isRunning)
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
        if (!currentZombie.isDead)
        {

            currentZombie.hp -= _dmg;
            Vector3 reactVec = transform.position - _targetPos;

            rigid.AddForce(reactVec.normalized*50 , ForceMode.Impulse);
            if (currentZombie.hp <= 0)
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
        currentZombie.isDead = true;
        anim.SetTrigger("DieFront");
        nav.isStopped = true;
        thePlayerStat.IncreaseEXP(currentZombie.exp);
        FindObjectOfType<ObjectManager>().TrySpawnItem(gameObject.transform.position);
        Destroy(gameObject, 4f);
    }



}
