using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//타이머 스크립트
public class TimerScript : MonoBehaviour
{

    float sec;
    int min;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private EnemySpawn theEnemySpawner;

    [SerializeField]
    private PlayerStat thePlayerStat;
    
    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    //타이머 계산후 출력하는 함수
    private void Timer()
    {
        sec += Time.deltaTime;

        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if((int)sec > 59)
        {
            sec = 0;
            min++;
            theEnemySpawner.updatedStage(min);
        }
        if(min == 10)
        {
            GameManager.instance.GameClear(thePlayerStat.GetScore());
        }
    }
}
