using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//레벨업시 실행되는 클래스
// 사용자의 능력치 상승을 위함.
public class LevelUpPause : MonoBehaviour
{


    //멈춰있는 상태
    bool isPause;

    //필요한 컴포넌트
    [SerializeField]
    private Image[] selectUI;
    [SerializeField]
    private GameObject selectObject;
    [SerializeField]
    private PlayerController thePlayerController;


    //표시하기로 선택된 능력들을 알기위한 리스트
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
        thePlayerController.ChangeLevelUpdate(false);
        seletedStatus.Clear();
        selectObject.SetActive(false);
    }


    //레벨업시 실행되는함수
    //게임진행을 막고 업그레이드 UI를 표출
    public void LevelUp()
    {
        isPause = false;
        ChangeTimeScale();
        thePlayerController.ChangeLevelUpdate(true);
        selectObject.SetActive(true);

        //랜덤값을 얻어서 리스트안에 없으면 해당 숫자를 RandomStatusSetting으로 넘겨줌.
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
