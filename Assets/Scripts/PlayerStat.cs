using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStat : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;
    private int currentHp;

    //스테미나
    [SerializeField]
    private float sp;
    private float currentSp;

    //경험치
    [SerializeField]
    private float exp;
    private float currentExp =0;

    private int level;

    //스태미나 증가량
    [SerializeField]
    private int spIncreaseSpeed;

    //스태미나 회복 딜레이
    [SerializeField]
    private float spRechargeTime;
    private float currentSpRechargeTime;

    //스테미나 감소 여부
    private bool spUsed;

    //방어력
    [SerializeField]
    private int dp;
    private int currentDp;


    //UI 표시 컴포넌트
    [SerializeField]
    private Image[] barImage;
    [SerializeField]
    private TextMeshProUGUI[] text;
    [SerializeField]
    private TextMeshProUGUI healCountText;

    private const int HP = 0, SP = 1, EXP = 2, LV = 3;

    private int healpack;

    public int score;


    //레벨업 스테이터스
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

    //업그레이드 종류에 따른 능력치 증가 함수
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

    //heal 아이템 갯수 증가 표시 함수
    private void ChangeHealCount(int count)
    {
        healCountText.text = count.ToString();
    }

    //heal 아이템 갯수 증가 함수
    public void IncreseHealPack(int num)
    {
        healpack += num;
        ChangeHealCount(healpack);
    }

    //플레이어 currentHp 증가 함수
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
            Debug.Log("힐팩이 없습니다.");
        }
    }

    //sp 재충전 대기시간 함수
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

    //player stat UI 처리 함수
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

    //sp 재충전 함수
    private void SpRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed * Time.deltaTime;
        }
    }

    //HP 채우기
    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    //SP증가
    public void IncreaseSP(int _count)
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }


    //SP소모
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



    //경험치 증가함수
    public void IncreaseEXP(int getExp)
    {
        currentExp += getExp;
        if (currentExp >= exp)
        {
            LevelUp();
        }
    }

    //레벨 업 함수
    private void LevelUp()
    {
        currentExp = 0;

        level += 1;
        exp += 30;

        FindObjectOfType<LevelUpPause>().LevelUp();
    }


    //플레이어 데미지 받기
    public void Damaged(int dam)
    {
        if (currentHp - dam > 0)
        {
            currentHp -= dam;
        }
        else
        {
            Die();
            //게임종료
        }
    }

    //죽는 함수
    private void Die()
    {
        GameManager.instance.GameOver(score);
    }

    //외부에서 호출되는 점수 증가함수
    public void IncreseScore(int num)
    {
        score += num;
    }


    //점수 반환 함수
    public int GetScore()
    {
        return score;
    }
}