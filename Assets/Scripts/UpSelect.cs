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


    //ui�� Ŭ���Ǿ������� ���׷��̵带 �����ϰ� �������
    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<PlayerStat>().UpgradeStat(statusType);
        FindObjectOfType<LevelUpPause>().ChangeActive();
    }


    //���� num������ uiǥ�õǴ� ���� �޶���.
    public void RandomStatusSetting(int num)
    {
        statusType = num;
        switch (num)
        {
            case (0):
                title.text = "Health+";
                content.text = "Increase Max HP";
                lv.text = "LV." + FindObjectOfType<PlayerStat>().addMaxHp.ToString();
                break;
            case (1):
                title.text = "Attack+";
                content.text = "Increase Damage";
                lv.text = "LV." + FindObjectOfType<PlayerStat>().addAttack.ToString();
                break;
            case (2):
                title.text = "AttackSpeed+";
                content.text = "Increase AttackSpeed";
                lv.text = "LV." + FindObjectOfType<PlayerStat>().addAttackSpeed.ToString();
                break;
            case (3):
                title.text = "SP+";
                content.text = "Increase Max SP";
                lv.text = "LV." + FindObjectOfType<PlayerStat>().addMaxSp.ToString();
                break;
            case (4):
                title.text = "Heal+";
                content.text = "Increase heal scale";
                lv.text = "LV." + FindObjectOfType<PlayerStat>().healScale.ToString();
                break;
        }
    }

    
}
