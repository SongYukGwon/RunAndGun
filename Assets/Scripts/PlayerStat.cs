using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Image[] barImage;


    private const int HP = 1, SP = 2;

    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
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
    }

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
        Debug.Log(currentSp);
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
            Debug.Log("캐릭터의 방여력이 0이 되었습니다.");
    }




    //플레이어 데미지 받기
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
            //게임종료
        }
    }

    //죽는 함수
    private void Die()
    {
        Debug.Log("Dead");
        Application.Quit();
    }
}