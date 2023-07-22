using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolData
{
    public string name;
    public GameObject origin;
    public List<GameObject> poolingObjects = new List<GameObject>();
}

public class PoolManager : Singleton<PoolManager>
{
    public List<PoolData> poolDatas = new List<PoolData>();
    private Dictionary<string, PoolData> poolDictionary = new Dictionary<string, PoolData>();

    protected override void OnCreated()
    {
        base.OnCreated();
        foreach (var poolData in poolDatas)
            poolDictionary.Add(poolData.name, poolData);
    }

    public GameObject Init(string objectName)
    {
        if (!poolDictionary.ContainsKey(objectName)) return null;

        var poolData = poolDictionary[objectName];

        if (poolData.poolingObjects == null)
            poolData.poolingObjects = new List<GameObject>();

        var disableObjects = poolData.poolingObjects.FindAll((obj) => !obj.gameObject.activeSelf);
        if (disableObjects.Count > 0)
        {
            var disableObject = disableObjects[0];
            disableObject.gameObject.SetActive(true);
            return disableObject;
        }
     
        var obj = Instantiate(poolData.origin);
        poolData.poolingObjects.Add(obj);
        return obj;
    }
}
