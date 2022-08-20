using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : ItemInfo
{
    // 0±«√— 1∂Û¿Ã«√ 2º¶∞« 3  4¿¸√º ≈∫æ‡
    private GunController gunController;

    private void Start()
    {
        gunController = FindObjectOfType<GunController>();
    }


    protected override void ItemEffet()
    {
        gunController.IncreseAmmo(type);
        int a = 0;
        switch(type)
        {
            case ("assult"):
                a = 0;
                break;
            case ("shotgun"):
                a = 1;
                break;
            case ("all"):
                a = 2;
                break;
        }
        ObjectManager.ReturnAmmoObject(this, a);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ItemEffet();
        }
    }
}
