using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    // 0±«√— 1∂Û¿Ã«√ 2º¶∞« 3  4¿¸√º ≈∫æ‡
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
