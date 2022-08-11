using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    private PlayerStat playerStat;
    private void Start()
    {
        playerStat = GameObject.Find("Stat").GetComponent<PlayerStat>();
    }

    public PlayerStat getPlayerStat()
    {
        return playerStat;
    }
}
