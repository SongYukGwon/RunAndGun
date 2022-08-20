using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent nav;

    private Transform target;

    //데드 레이어 지정
    int layerDead = 9;
    //에너미 레이어 지정
    int layerEnemy = 7;


    //좀비 정보
    private ZombieInfo currentZombie;

    //좀비의 필요한 컴포넌트
    private PlayerStat thePlayerStat;
    private Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider col;

    void OnEnable()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
        target = GameObject.Find("Player").GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        currentZombie = GetComponent<ZombieInfo>();

        currentZombie.currentHp = currentZombie.hp;

        gameObject.tag = "Enemy";
        gameObject.layer = layerEnemy;
        currentZombie.isDead = false;
        nav.SetDestination(target.position);
        SetZombieWalkORRun();
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
        TryAttack();
        CalAttackSpeed();
    }

    //좀비의 속도 랜덤 설정 함수
    private void SetZombieWalkORRun()
    {
        float speed = Random.Range(currentZombie.walkSpeed, currentZombie.runSpeed);

        if (speed <= 3f)
        {
            currentZombie.isWalking = true;
            currentZombie.isRunning = false;
        }
        else
        {
            currentZombie.isWalking = false;
            currentZombie.isRunning = true;
        }
        nav.speed = speed;
    }

    //공격속도 계산 함수
    private void CalAttackSpeed()
    {
        if(currentZombie.currentAttackSpeed > 0f)
            currentZombie.currentAttackSpeed -= Time.deltaTime;
    }



    //범위안에 플레이어가 들어왔을때
    private void TryAttack()
    {
        if(Vector3.Distance(transform.position, target.position)<= 2.0f && currentZombie.currentAttackSpeed <= 0f && 
            !currentZombie.isDead)
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

            currentZombie.currentHp -= _dmg;
            Vector3 reactVec = transform.position - _targetPos;

            rigid.AddForce(reactVec.normalized*50 , ForceMode.Impulse);
            if (currentZombie.currentHp <= 0)
            {
                StartCoroutine(Dead());
                return;
            }

        }
    }


    //죽는 함수
    protected IEnumerator Dead()
    {
        gameObject.tag = "Dead";
        gameObject.layer = layerDead;
        currentZombie.isDead = true;
        anim.SetTrigger("DieFront");
        nav.isStopped = true;
        thePlayerStat.IncreaseEXP(currentZombie.exp);
        thePlayerStat.IncreseScore(1);

        //오브젝트풀로 수정 예정
        ObjectManager.Instance.TrySpawnItem(gameObject.transform.position);

        yield return new WaitForSeconds(4f);
        EnemyObjectPool.ReturnObject(gameObject);
    }



}
