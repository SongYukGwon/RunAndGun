using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("������ �Ծ����ϴ�.");
            other.GetComponent<PlayerController>().IncreseHealPack(1);
            Destroy(gameObject);
        }
    }
}
