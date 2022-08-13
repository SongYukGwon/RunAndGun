using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInfo : MonoBehaviour
{
    [SerializeField]
    protected string type;

    protected abstract void ItemEffet(Collider other);
}
