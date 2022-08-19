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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ItemEffet();
        }
    }
}
