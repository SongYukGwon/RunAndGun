using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{

    float sec;
    int min;

    [SerializeField]
    private TextMeshProUGUI timerText;

    
    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    private void Timer()
    {
        sec += Time.deltaTime;

        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if((int)sec > 59)
        {
            sec = 0;
            min++;
        }

        if(min == 30)
        {

        }
    }
}
