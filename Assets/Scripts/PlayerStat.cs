using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStat : MonoBehaviour
{
    //ü��
    [SerializeField]
    private int hp;
    private int currentHp;

    //���׹̳�
    [SerializeField]
    private float sp;
    private float currentSp;

    //����ġ
    [SerializeField]
    private float exp;
    private float currentExp =0;

    private int level;

    //���¹̳� ������
    [SerializeField]
    private int spIncreaseSpeed;

    //���¹̳� ȸ�� ������
    [SerializeField]
    private float spRechargeTime;
    private float currentSpRechargeTime;

    //���׹̳� ���� ����
    private bool spUsed;

    //����
    [SerializeField]
    private int dp;
    private int currentDp;


    //UI ǥ�� ������Ʈ
    [SerializeField]
    private Image[] barImage;
    [SerializeField]
    private TextMeshProUGUI[] text;

    private const int HP = 0, SP = 1, EXP = 2, LV = 3;

    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
        level = 1;
    }

    private void Update()
    {
        SPRechargeTime();
        SpRecover();
        GaugeUpdate();
    }

    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime += Time.deltaTime;
            else
                spUsed = false;
        }
    }

    private void GaugeUpdate()
    {
        barImage[HP].fillAmount = (float)currentHp / hp;
        barImage[SP].fillAmount = (float)currentSp / sp;
        barImage[EXP].fillAmount = (float)currentExp / exp;
    }

    private void SpRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed * Time.deltaTime;
        }
    }

    //HP ä���
    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    //SP����
    public void IncreaseSP(int _count)
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }


    //SP�Ҹ�
    public void DecreaseStamina(float _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;
        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }
    public float GetCurrentSP()
    {
        return currentSp;
    }


    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("ĳ������ �濩���� 0�� �Ǿ����ϴ�.");
    }

    public void IncreaseEXP(int getExp)
    {
        currentExp += getExp;
        Debug.Log(currentExp);
        if (currentExp >= exp)
        {
            Debug.Log("LevelUp");
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExp = 0;

        level += 1;
        exp += 50;
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
        Application.Quit();
    }
}