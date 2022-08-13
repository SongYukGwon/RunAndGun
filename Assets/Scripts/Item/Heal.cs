using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : ItemInfo
{
    protected override void ItemEffet(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("ÈúÆÑÀ» ¸Ô¾ú½À´Ï´Ù.");
            other.GetComponent<PlayerController>().IncreseHealPack(1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemEffet(other);
    }
}
