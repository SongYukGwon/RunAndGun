using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//오브젝트풀 사용 필요.
public class ObjectManager : MonoBehaviour
{
    //0 Ammo, 1 heal  등장
    //아이템 prefab 저장하는 곳
    [SerializeField]
    private Ammo[] ammo; // 0 assult 1 shotgun 2 all
    [SerializeField]
    private Heal heal;
    [SerializeField]
    private float itemSpawnPer;
    [SerializeField]
    private float healPackPer;
    [SerializeField]
    private float ammoPer;

    public static ObjectManager Instance;

    Queue<Heal> poolingHealQueue = new Queue<Heal>();
    List<Queue<Ammo>> poolingAmmoQueue = new List<Queue<Ammo>>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < ammo.Length; i++)
        {
            poolingAmmoQueue.Add(new Queue<Ammo>());
        }

        InitializeHeal(50);
        InitializeAmmo(50);
    }

    private void InitializeHeal(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingHealQueue.Enqueue(CreateNewHealObject());
        }
    }

    private void InitializeAmmo(int initCount)
    {
        for (int j = 0; j < ammo.Length; j++)
        {
            for (int i = 0; i < initCount; i++)
            {
                poolingAmmoQueue[j].Enqueue(CreateNewAmmoObject(j));
            }
        }
        
    }


    private Heal CreateNewHealObject()
    {
        var newObj = Instantiate(heal);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    private Ammo CreateNewAmmoObject(int type)
    {
        var newObj = Instantiate(ammo[type]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }


    public static Heal GetHealObject()
    {
        if (Instance.poolingHealQueue.Count > 0)
        {
            var obj = Instance.poolingHealQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewHealObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static Ammo GetAmmoObject(int type)
    {
        if (Instance.poolingAmmoQueue[type].Count > 0)
        {
            var obj = Instance.poolingAmmoQueue[type].Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewAmmoObject(type);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnHealObject(Heal obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingHealQueue.Enqueue(obj);
    }

    public static void ReturnAmmoObject(Ammo obj, int type)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingAmmoQueue[type].Enqueue(obj);
    }


    public void TrySpawnItem(Vector3 pos)
    {
        float per = Random.Range(0f, 1f);
        if (per < itemSpawnPer)
        {
            SpawmItem(pos);
        }
    }

    private void SpawmItem(Vector3 pos)
    {
        float itemType = Random.Range(0f, healPackPer + ammoPer);
        Object spawnItem = new Object();
        if (itemType < ammoPer)
        {
            int ammoType = Random.Range(0, 3);
            Debug.Log(ammoType);
            spawnItem = GetAmmoObject(ammoType);
        }
        else if (ammoPer <= itemType && itemType <= healPackPer + ammoPer)
        {
            spawnItem = GetHealObject();
        }

        Instantiate(spawnItem, pos, Quaternion.identity);
    }
}
