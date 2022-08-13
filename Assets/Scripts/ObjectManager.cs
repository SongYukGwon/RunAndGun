using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //0 Ammo, 1 heal  등장
    //아이템 prefab 저장하는 곳
    [SerializeField]
    private Ammo[] ammo; // 0 assult 1 shotgun 2 all
    [SerializeField]
    private Heal heal;
    [SerializeField]
    private float itemSpawnPer;

    public void TrySpawnItem(Vector3 pos)
    {
        float per = Random.Range(0f, 1f);
        Debug.Log(per);
        if (per < itemSpawnPer)
        {
            SpawmItem(pos);
        }
    }

    private void SpawmItem(Vector3 pos)
    {
        int itemType = Random.Range(0,2);
       
        switch(itemType)
        {
            case (0):
                int ammoType = Random.Range(0, 3);
                Instantiate(ammo[ammoType], pos, Quaternion.identity);
                break;
            case (1):
                Instantiate(heal, pos, Quaternion.identity);
                break;
        }
    }
}
