using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class Pool
{
    public int key;
    public GameObject prefab;
    public int size;
}
public class ObjPoolManager : MonoBehaviour
{
    public static ObjPoolManager instance;
    [SerializeField] public List<Pool> pools;
    private Dictionary<int, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        poolDictionary = new Dictionary<int, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.key, objectPool);
        }

    }
    public GameObject GetObject(int key, Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            Pool poolConfig = pools.Find(p => p.key == key);
            obj = Instantiate(poolConfig.prefab);
        }
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        var tempObstacle = obj.GetComponent<TempObstacle>();
        if (tempObstacle != null)
        {
            tempObstacle.Init(key);
        }

        return obj;
    }

    public void ReturnObject(int key, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }
}
