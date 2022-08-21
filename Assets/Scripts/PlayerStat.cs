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
    [SerializeField]
    private TextMeshProUGUI healCountText;

    private const int HP = 0, SP = 1, EXP = 2, LV = 3;

    private int healpack;

    public int score;


    //������ �������ͽ�
    public int addAttack=0;
    public int addAttackSpeed=0;
    public int addMaxHp=0;
    public int healScale=0;
    public int addMaxSp = 0;

    void Start()
    {
        currentHp = 50;
        currentSp = sp;
        currentDp = dp;
        level = 1;
        healpack = 0;
        score = 0;
    }

    private void Update()
    {
        SPRechargeTime();
        SpRecover();
        GaugeUpdate();
    }

    //���׷��̵� ������ ���� �ɷ�ġ ���� �Լ�
    public void UpgradeStat(int statusType)
    {
        switch(statusType)
        {
            case (0):
                addMaxHp += 1;
                hp += 20;
                break;
            case (1):
                addAttack += 1;
                break;
            case (2):
                addAttackSpeed += 1;
                break;
            case (3):
                addMaxSp += 1;
                sp += 20;
                break;
            case (4):
                healScale += 1;
                break;
        }
    }

    //heal ������ ���� ���� ǥ�� �Լ�
    private void ChangeHealCount(int count)
    {
        healCountText.text = count.ToString();
    }

    //heal ������ ���� ���� �Լ�
    public void IncreseHealPack(int num)
    {
        healpack += num;
        ChangeHealCount(healpack);
    }

    //�÷��̾� currentHp ���� �Լ�
    public void Heal()
    {
        if (healpack > 0)
        {
            IncreaseHP(healScale*10 + 30);
            healpack -= 1;
            ChangeHealCount(healpack);
        }
        else
        {
            Debug.Log("������ �����ϴ�.");
        }
    }

    //sp ������ ���ð� �Լ�
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

    //player stat UI ó�� �Լ�
    private void GaugeUpdate()
    {
        barImage[HP].fillAmount = (float)currentHp / hp;
        barImage[SP].fillAmount = (float)currentSp / sp;
        barImage[EXP].fillAmount = (float)currentExp / exp;
        text[HP].text = currentHp.ToString();
        text[SP].text = string.Format("{0:.0}", currentSp);
        text[EXP].text = string.Format("{0:00}", ((float)currentExp / exp)*100)+"%";
        text[LV].text = level.ToString();
    }

    //sp ������ �Լ�
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



    //����ġ �����Լ�
    public void IncreaseEXP(int getExp)
    {
        currentExp += getExp;
        if (currentExp >= exp)
        {
            LevelUp();
        }
    }

    //���� �� �Լ�
    private void LevelUp()
    {
        currentExp = 0;

        level += 1;
        exp += 30;

        FindObjectOfType<LevelUpPause>().LevelUp();
    }


    //�÷��̾� ������ �ޱ�
    public void Damaged(int dam)
    {
        if (currentHp - dam > 0)
        {
            currentHp -= dam;
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
        GameManager.instance.GameOver(score);
    }

    //�ܺο��� ȣ��Ǵ� ���� �����Լ�
    public void IncreseScore(int num)
    {
        score += num;
    }


    //���� ��ȯ �Լ�
    public int GetScore()
    {
        return score;
    }
}