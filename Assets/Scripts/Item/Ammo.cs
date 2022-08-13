using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : ItemInfo
{
    // 0���� 1������ 2���� 3  4��ü ź��

    protected override void ItemEffet(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GunController>().IncreseAmmo(type);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemEffet(other);
    }
}
