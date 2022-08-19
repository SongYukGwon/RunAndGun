using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInfo : MonoBehaviour
{
    //좀비 hp
    public int hp;

    //좀비 현재 hp
    public int currentHp;

    //좀비의 공격 속도
    public float attackSpeed;

    //좀비 걷기 속도
    public float walkSpeed;

    //좀비 뛰는 속도
    public float runSpeed;

    //좀비 공격력
    public int damage;

    //좀비가 주는 경험치
    public int exp;

    //좀비 아이템 스폰 확률
    public float itemSpawnPer;

    //상태변수 확인
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isAttack = false;
    public bool isDead = false;
    public float currentAttackSpeed = 0f;

}
