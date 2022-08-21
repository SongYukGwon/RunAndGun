using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectPool : MonoBehaviour
{
    //싱글톤 선언
    public static HitEffectPool Instance;

    [SerializeField]
    private GameObject hit_prefab;
    [SerializeField]
    private GameObject other_hit_prefab;

    Queue<GameObject> hitPoolingQueue = new Queue<GameObject>();
    Queue<GameObject> otherPoolingQueue = new Queue<GameObject>();

    void Start()
    {
        Instance = this;

        InitializeHitEffet(40);
        InitializeOtherEffet(40);
    }
    private void InitializeHitEffet(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            hitPoolingQueue.Enqueue(CreateNewHitObject());
        }
    }

    private void InitializeOtherEffet(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            otherPoolingQueue.Enqueue(CreateNewOtherObject());
        }
    }

    private GameObject CreateNewHitObject()
    {
        var newObj = Instantiate(hit_prefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    private GameObject CreateNewOtherObject()
    {
        var newObj = Instantiate(other_hit_prefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static GameObject GetHitObject()
    {
        if (Instance.hitPoolingQueue.Count > 0)
        {
            var obj = Instance.hitPoolingQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewHitObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static GameObject GetOtherObject()
    {
        if (Instance.otherPoolingQueue.Count > 0)
        {
            var obj = Instance.otherPoolingQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewOtherObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    //반환 받는 함수
    public static void ReturnHitObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.hitPoolingQueue.Enqueue(obj);
    }


    //type으로 구분하여 반환 받는 함수
    public static void ReturnOtherObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.otherPoolingQueue.Enqueue(obj);
    }


}
