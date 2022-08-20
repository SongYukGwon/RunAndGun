using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunUIHandler : MonoBehaviour
{
    //���̹���
    [SerializeField]
    private Image[] gunImages;
    
    //�Ѿ� �̹���
    [SerializeField]
    private Image[] gunAmmoImages;

    private int currentImageNum = 1;
    //private const int PISTOL = 0, ASSULT = 1, SHOTGUN = 2;

    
    //�ѹٲ𶧸��� ȣ��Ǵ� UI ���� �Լ�
    public void ChangeGunUI(int type)
    {
        gunImages[type].color = new Color(1, 1, 1, 1);
        gunAmmoImages[type].color = new Color(1, 1, 1, 1);

        gunImages[currentImageNum].color = new Color(1, 1, 1, 0);
        gunAmmoImages[currentImageNum].color = new Color(1, 1, 1, 0);

        currentImageNum = type;
        
    }
}
