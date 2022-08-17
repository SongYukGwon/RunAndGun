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

    private void ChangeTimeScale()
    {
        if (isPause)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

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
