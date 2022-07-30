using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
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

    //�÷��̾� ������ �ޱ�
    public void Damaged(int dam)
    {
        if (currentHp - dam > 0)
        {
            currentHp -= dam;
            Debug.Log(currentHp);
        }
        else
        {
            Die();
            //��������
        }
    }

    //�״� �Լ�
    private void Die()
    {
        Debug.Log("Dead");
    }
}