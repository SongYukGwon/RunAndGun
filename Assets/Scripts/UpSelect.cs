using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpSelect : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI content;
    [SerializeField]
    private TextMeshProUGUI lv;
    private int statusType;


    //ui가 클릭되어있을때 업그레이드를 진행하고 게임재게
    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<PlayerStat>().UpgradeStat(statusType);
        FindObjectOfType<LevelUpPause>().ChangeActive();
    }


    //들어온 num에따라 ui표시되는 것이 달라짐.
    public void RandomStatusSetting(int num)
    {
        statusType = num;
        switch (num)
        {
            case (0):
                title.text = "Health+";
                content.text = "Increase Max HP";
                
                break;
            case (1):
                title.text = "Attack+";
                content.text = "Increase Damage";
                break;
            case (2):
                title.text = "AttackSpeed+";
                content.text = "Increase AttackSpeed";
                break;
            case (3):
                title.text = "SP+";
                content.text = "Increase Max SP";
                break;
            case (4):
                title.text = "Heal+";
                content.text = "Increase heal scale";
                break;
        }
    }

    
}
