using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPause : MonoBehaviour
{

    bool isPause;

    [SerializeField]
    private Image[] selectUI;
    [SerializeField]
    private GameObject selectObject;

    private List<int> seletedStatus;

    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
        seletedStatus = new List<int>();
    }


    //게임진행 멈춤 or 진행
    private void ChangeTimeScale()
    {
        if (isPause)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }


    //업그레이드 완료시 실행되는 함수
    public void ChangeActive()
    {
        isPause = true;
        ChangeTimeScale();
        FindObjectOfType<PlayerController>().ChangeLevelUpdate(false);
        seletedStatus.Clear();
        selectObject.SetActive(false);
    }


    //레벨업시 실행되는함수
    //게임진행을 막고 업그레이드 UI를 표출
    public void LevelUp()
    {
        isPause = false;
        ChangeTimeScale();
        FindObjectOfType<PlayerController>().ChangeLevelUpdate(true);
        selectObject.SetActive(true);
        int i = 0;
        while (i<selectUI.Length) 
        {
            int num = Random.Range(0, 4);
            if(seletedStatus.Contains(num))
            {
                continue;
            }
            else
            {
                seletedStatus.Add(num);
                selectUI[i].GetComponent<UpSelect>().RandomStatusSetting(num);
                i += 1;
            }
        }

        
    }
}
