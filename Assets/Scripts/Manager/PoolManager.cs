using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pool
{
    public PoolType poolType;
    public int initialSize;

    private GameObject parent;
    private GameObject prefabObj;
    private List<GameObject> objList = new List<GameObject>();

    public void Initialize(GameObject parent)
    {
        prefabObj = Resources.Load<MonoBehaviour>($"Prefabs/{poolType}").gameObject;
        if (!prefabObj) return;

        this.parent = parent;
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    public GameObject CreateNewObject()
    {
        GameObject obj = GameObject.Instantiate(prefabObj, parent.transform);
        obj.SetActive(false);
        objList.Add(obj);
        return obj;
    }

    public GameObject GetObject()
    {
        GameObject obj = objList.Find(o => !o.activeSelf);
        if (!obj)
        {
            obj = CreateNewObject();
        }
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj, GameObject parent)
    {
        if (!objList.Contains(obj))
        {
            objList.Add(obj);
        }
        obj.SetActive(false);
        obj.transform.SetParent(parent.transform);
    }
}

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private Pool[] pools;
    private Dictionary<PoolType, Pool> poolDic = new Dictionary<PoolType, Pool>();
    private Dictionary<PoolType, GameObject> parentDic = new Dictionary<PoolType, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (Pool pool in pools)
        {
            GameObject poolParent = new GameObject($"{pool.poolType}Pool");
            poolParent.transform.SetParent(transform);

            parentDic[pool.poolType] = poolParent;
            poolDic[pool.poolType] = pool;

            pool.Initialize(parentDic[pool.poolType]);
        }
    }

    public T GetObject<T>(PoolType type, Vector3 position, Transform parent) where T : Component
    {
        if (!poolDic.TryGetValue(type, out Pool pool))
        {
            return null;
        }

        GameObject obj = pool.GetObject();
        if (obj)
        {
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            return obj.GetComponent<T>();
        }
        return null;
    }

    public void ReturnObject(PoolType type, GameObject obj)
    {
        if (!obj) return;
        poolDic[type].ReturnObject(obj, parentDic[type]);
    }
}
