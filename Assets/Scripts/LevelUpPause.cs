using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPause : MonoBehaviour
{

    bool isPause;



    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
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
        ChangeTimeScale();


    }
}
