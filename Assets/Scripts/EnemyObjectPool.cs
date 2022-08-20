using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������Ʈ Ǯ �����Լ�
public class EnemyObjectPool : MonoBehaviour
{
    //�̱��� ����
    public static EnemyObjectPool Instance;

    //���� ������
    [SerializeField]
    private GameObject poolingObjectPrefab;

    //pool
    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();


    //Ȱ��ȭ �Ǹ� �̱����� ���� ����� objectpool �ʱ�ȭ ����
    private void Awake()
    {
        Instance = this;
        Initialize(150);
    }

    //initCount��ŭ ������ �̸� �ص�
    private void Initialize(int initCount)
    {
        for(int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }


    //�����ϴ� �Լ�
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    //�ܺο��� pool�� ��û�ϴ� �Լ�
    //queue�� ��������� ���ְ� ������ �����Ͽ� ��
    public static GameObject GetObject()
    {
        if(Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }


    //��ȯ�޴� �Լ�
    //queue�� �ٽ� ����
    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }

}