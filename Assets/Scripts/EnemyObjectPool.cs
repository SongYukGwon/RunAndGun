using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//좀비 오브젝트 풀 생성함수
public class EnemyObjectPool : MonoBehaviour
{
    //싱글톤 선언
    public static EnemyObjectPool Instance;

    //좀비 프리펩
    [SerializeField]
    private GameObject poolingObjectPrefab;

    //pool
    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();


    //활성화 되면 싱글톤을 위한 선언과 objectpool 초기화 선언
    private void Awake()
    {
        Instance = this;
        Initialize(150);
    }

    //initCount만큼 생성을 미리 해둠
    private void Initialize(int initCount)
    {
        for(int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }


    //생성하는 함수
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    //외부에서 pool에 요청하는 함수
    //queue에 들어있으면 빼주고 없으면 생성하여 줌
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


    //반환받는 함수
    //queue에 다시 들어옴
    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }

}