using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//오브젝트풀 사용 필요.
public class ObjectManager : MonoBehaviour
{
    //싱글톤 선언
    public static ObjectManager Instance;

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

    //pool
    Queue<Heal> poolingHealQueue = new Queue<Heal>();
    List<Queue<Ammo>> poolingAmmoQueue = new List<Queue<Ammo>>();

    private void Awake()
    {
        Instance = this;

        //List 초기화
        for (int i = 0; i < ammo.Length; i++)
        {
            poolingAmmoQueue.Add(new Queue<Ammo>());
        }

        //아이템마다 각각 50개씩 초기화해둠
        InitializeHeal(50);
        InitializeAmmo(50);
    }

    //초기화 하는 함수
    private void InitializeHeal(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingHealQueue.Enqueue(CreateNewHealObject());
        }
    }

    private void InitializeAmmo(int initCount)
    {
        //저장되어있는 ammo의 개수만큼 type으로 구분하여 초기화
        for (int j = 0; j < ammo.Length; j++)
        {
            for (int i = 0; i < initCount; i++)
            {
                poolingAmmoQueue[j].Enqueue(CreateNewAmmoObject(j));
            }
        }
        
    }

    //생성하는 함수
    private Heal CreateNewHealObject()
    {
        var newObj = Instantiate(heal);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    //들어온 타입에따라 queue에 저장해둠
    private Ammo CreateNewAmmoObject(int type)
    {
        var newObj = Instantiate(ammo[type]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    //heal 아이템 불러오는 함수
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

    //type에 따라 Ammo아이템 불러오는 함수
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

    //반환 받는 함수
    public static void ReturnHealObject(Heal obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingHealQueue.Enqueue(obj);
    }


    //type으로 구분하여 반환 받는 함수
    public static void ReturnAmmoObject(Ammo obj, int type)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingAmmoQueue[type].Enqueue(obj);
    }

    //외부에서 스폰 요청 받는 함수
    public void TrySpawnItem(Vector3 pos)
    {
        float per = Random.Range(0f, 1f);
        if (per < itemSpawnPer)
        {
            SpawmItem(pos);
        }
    }

    //스폰하는 함수
    //설정된 값에따른 확률로 스폰함.
    private void SpawmItem(Vector3 pos)
    {
        float itemType = Random.Range(0f, healPackPer + ammoPer);
        Object spawnItem = new Object();
        if (itemType < ammoPer)
        {
            int ammoType = Random.Range(0, 3);
            spawnItem = GetAmmoObject(ammoType);
        }
        else if (ammoPer <= itemType && itemType <= healPackPer + ammoPer)
        {
            spawnItem = GetHealObject();
        }

        Instantiate(spawnItem, pos, Quaternion.identity);
    }
}
