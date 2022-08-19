using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������ƮǮ ��� �ʿ�.
public class ObjectManager : MonoBehaviour
{
    //0 Ammo, 1 heal  ����
    //������ prefab �����ϴ� ��
    [SerializeField]
    private Ammo[] ammo; // 0 assult 1 shotgun 2 all
    [SerializeField]
    private Heal heal;
    [SerializeField]
    private float itemSpawnPer;
    [SerializeField]
    private float healPackPer;
    [SerializeField]
    private float ammoPer;

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
        float itemType = Random.Range(0f, healPackPer+ammoPer);
        Object spawnItem = new Object(); 
        if(itemType< ammoPer)
        {
            int ammoType = Random.Range(0, 3);
            spawnItem = ammo[ammoType];
        }
        else if(ammoPer < itemType && itemType < healPackPer+ammoPer)
        {
            spawnItem = heal;
        }

        Instantiate(spawnItem, pos, Quaternion.identity);
    }
}
