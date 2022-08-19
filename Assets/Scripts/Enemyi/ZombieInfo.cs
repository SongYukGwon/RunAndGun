using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInfo : MonoBehaviour
{
    //���� hp
    public int hp;

    //���� ���� hp
    public int currentHp;

    //������ ���� �ӵ�
    public float attackSpeed;

    //���� �ȱ� �ӵ�
    public float walkSpeed;

    //���� �ٴ� �ӵ�
    public float runSpeed;

    //���� ���ݷ�
    public int damage;

    //���� �ִ� ����ġ
    public int exp;

    //���� ������ ���� Ȯ��
    public float itemSpawnPer;

    //���º��� Ȯ��
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isAttack = false;
    public bool isDead = false;
    public float currentAttackSpeed = 0f;

}
