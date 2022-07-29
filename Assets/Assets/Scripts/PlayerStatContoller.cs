using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    //ü��
    [SerializeField]
    private int hp;
    private int currentHp;

    //���׹̳�
    [SerializeField]
    private int sp;
    private int currentSp;

    //���¹̳� ������
    [SerializeField]
    private int spIncreaseSpeed;

    //���¹̳� ȸ�� ������
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    //���׹̳� ���� ����
    private bool spUsed;

    //����
    [SerializeField]
    private int dp;
    private int currentDp;

    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
    }
}