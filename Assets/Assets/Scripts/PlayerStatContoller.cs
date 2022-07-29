using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;
    private int currentHp;

    //스테미나
    [SerializeField]
    private int sp;
    private int currentSp;

    //스태미나 증가량
    [SerializeField]
    private int spIncreaseSpeed;

    //스태미나 회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    //스테미나 감소 여부
    private bool spUsed;

    //방어력
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