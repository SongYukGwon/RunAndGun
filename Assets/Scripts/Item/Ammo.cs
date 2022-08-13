using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    // 0���� 1������ 2���� 3  4��ü ź��
    [SerializeField]
    private string ammoType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GunController>().IncreseAmmo(ammoType);
            Destroy(gameObject);
        }
    }
}
