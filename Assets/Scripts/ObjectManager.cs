using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������ƮǮ ��� �ʿ�.
public class ObjectManager : MonoBehaviour
{
    //�̱��� ����
    public static ObjectManager Instance;

    //0 Ammo, 1 heal  ����
    //������ prefab �����ϴ� ��
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

        //List �ʱ�ȭ
        for (int i = 0; i < ammo.Length; i++)
        {
            poolingAmmoQueue.Add(new Queue<Ammo>());
        }

        //�����۸��� ���� 50���� �ʱ�ȭ�ص�
        InitializeHeal(50);
        InitializeAmmo(50);
    }

    //�ʱ�ȭ �ϴ� �Լ�
    private void InitializeHeal(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingHealQueue.Enqueue(CreateNewHealObject());
        }
    }

    private void InitializeAmmo(int initCount)
    {
        //����Ǿ��ִ� ammo�� ������ŭ type���� �����Ͽ� �ʱ�ȭ
        for (int j = 0; j < ammo.Length; j++)
        {
            for (int i = 0; i < initCount; i++)
            {
                poolingAmmoQueue[j].Enqueue(CreateNewAmmoObject(j));
            }
        }
        
    }

    //�����ϴ� �Լ�
    private Heal CreateNewHealObject()
    {
        var newObj = Instantiate(heal);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    //���� Ÿ�Կ����� queue�� �����ص�
    private Ammo CreateNewAmmoObject(int type)
    {
        var newObj = Instantiate(ammo[type]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    //heal ������ �ҷ����� �Լ�
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

    //type�� ���� Ammo������ �ҷ����� �Լ�
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

    //��ȯ �޴� �Լ�
    public static void ReturnHealObject(Heal obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingHealQueue.Enqueue(obj);
    }


    //type���� �����Ͽ� ��ȯ �޴� �Լ�
    public static void ReturnAmmoObject(Ammo obj, int type)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingAmmoQueue[type].Enqueue(obj);
    }

    //�ܺο��� ���� ��û �޴� �Լ�
    public void TrySpawnItem(Vector3 pos)
    {
        float per = Random.Range(0f, 1f);
        if (per < itemSpawnPer)
        {
            SpawmItem(pos);
        }
    }

    //�����ϴ� �Լ�
    //������ �������� Ȯ���� ������.
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
