using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�������� ����Ǵ� Ŭ����
// ������� �ɷ�ġ ����� ����.
public class LevelUpPause : MonoBehaviour
{
    //�ʿ��� ������Ʈ
    [SerializeField]
    private Image[] selectUI;
    [SerializeField]
    private GameObject selectObject;
    [SerializeField]
    private PlayerController thePlayerController;


    //ǥ���ϱ�� ���õ� �ɷµ��� �˱����� ����Ʈ
    private List<int> seletedStatus;

    // Start is called before the first frame update
    void Start()
    {
        seletedStatus = new List<int>();
    }


    


    //���׷��̵� �Ϸ�� ����Ǵ� �Լ�
    public void ChangeActive()
    {
        GameManager.instance.ChangeTimeScale(true);
        thePlayerController.ChangeLevelUpdate(false);
        seletedStatus.Clear();
        selectObject.SetActive(false);
    }


    //�������� ����Ǵ��Լ�
    //���������� ���� ���׷��̵� UI�� ǥ��
    public void LevelUp()
    {
        GameManager.instance.ChangeTimeScale(false);
        thePlayerController.ChangeLevelUpdate(true);
        selectObject.SetActive(true);

        //�������� �� ����Ʈ�ȿ� ������ �ش� ���ڸ� RandomStatusSetting���� �Ѱ���.
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
