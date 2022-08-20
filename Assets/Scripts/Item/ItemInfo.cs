using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템이 공통적으로 갖는것 모아두기
public abstract class ItemInfo : MonoBehaviour
{
    [SerializeField]
    protected string type;

    protected abstract void ItemEffet();
}
