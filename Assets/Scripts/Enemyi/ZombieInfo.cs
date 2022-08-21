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


    //������ ������ �����ϴ� �Լ�
    // �������������� ���� ������ ������ �þ.
    public void SetStatus(int stage)
    {
        hp = stage + 1;
        damage = stage+1;
        exp = stage + 1;
        currentHp = hp;
    }

}
