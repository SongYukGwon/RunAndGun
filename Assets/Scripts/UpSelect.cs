using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpSelect : MonoBehaviour, IPointerClickHandler
{

    //ui ������Ʈ
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI content;
    [SerializeField]
    private TextMeshProUGUI lv;

    //��Ȳ ����
    private int statusType;


    //�ʿ��� ������Ʈ
    [SerializeField]
    private PlayerStat thePlayerStat;
    [SerializeField]
    private LevelUpPause theLevelUpPause;

    //ui�� Ŭ���Ǿ������� ���׷��̵带 �����ϰ� �������
    public void OnPointerClick(PointerEventData eventData)
    {
        thePlayerStat.UpgradeStat(statusType);
        theLevelUpPause.ChangeActive();
    }


    //���� num������ uiǥ�õǴ� ���� �޶���.
    public void RandomStatusSetting(int num)
    {
        statusType = num;
        switch (statusType)
        {
            case (0):
                title.text = "Health+";
                content.text = "Increase Max HP";
                lv.text = "LV." + thePlayerStat.addMaxHp.ToString();
                break;
            case (1):
                title.text = "Attack+";
                content.text = "Increase Damage";
                lv.text = "LV." + thePlayerStat.addAttack.ToString();
                break;
            case (2):
                title.text = "AttackSpeed+";
                content.text = "Increase AttackSpeed";
                lv.text = "LV." + thePlayerStat.addAttackSpeed.ToString();
                break;
            case (3):
                title.text = "SP+";
                content.text = "Increase Max SP";
                lv.text = "LV." + thePlayerStat.addMaxSp.ToString();
                break;
            case (4):
                title.text = "Heal+";
                content.text = "Increase heal scale";
                lv.text = "LV." + thePlayerStat.healScale.ToString();
                break;
        }
    }

    
}
