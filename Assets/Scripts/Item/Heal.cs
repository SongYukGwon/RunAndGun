using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heal������ �Լ�
public class Heal : ItemInfo
{

    private PlayerStat thePlayerStat;

    private void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }

    protected override void ItemEffet()
    {
        thePlayerStat.IncreseHealPack(1);
        ObjectManager.ReturnHealObject(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ItemEffet();
        }
    }
}
