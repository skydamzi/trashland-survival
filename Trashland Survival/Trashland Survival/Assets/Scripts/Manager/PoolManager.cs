using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[DefaultExecutionOrder(-100)]
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, Transform> poolParents = new Dictionary<GameObject, Transform>();
    private Dictionary<GameObject, int> activeCounts = new Dictionary<GameObject, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        GameEvents.OnGameExit += ResetPools;
    }

    void OnDisable()
    {
        GameEvents.OnGameExit -= ResetPools;
    }

    private void ResetPools()
    {
        foreach (var parent in poolParents.Values)
        {
            Destroy(parent.gameObject);
        }

        poolDictionary.Clear();
        poolParents.Clear();
        activeCounts.Clear();
        Debug.Log("오브젝트 풀 초기화");
    }

    public void PreparePool(GameObject prefab, int size)
    {
        if (prefab == null)
        {
            return;
        }

        if (poolDictionary.ContainsKey(prefab))
        {
            return;
        }

        Transform poolParent = new GameObject(prefab.name + " Pool").transform;
        poolParent.SetParent(this.transform);
        poolParents.Add(prefab, poolParent);
        activeCounts.Add(prefab, 0);

        poolDictionary.Add(prefab, new Queue<GameObject>());

        for (int i = 0; i < size; i++)
        {
            GameObject newObj = Instantiate(prefab, poolParent);
            newObj.GetComponent<Poolable>().sourcePrefab = prefab;
            newObj.SetActive(false);
            poolDictionary[prefab].Enqueue(newObj);
        }
    }

    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null || !poolDictionary.ContainsKey(prefab))
        {
            string prefabName = (prefab == null) ? "null" : prefab.name;
            Debug.LogError(prefabName + "을(를) 위한 풀이 없습니다. 먼저 준비해주세요.");
            return null;
        }

        Queue<GameObject> queue = poolDictionary[prefab];
        GameObject objToGet;

        if (queue.Count > 0)
        {
            objToGet = queue.Dequeue();
        }
        else
        {
            Transform poolParent = poolParents[prefab];
            objToGet = Instantiate(prefab, poolParent);
            objToGet.GetComponent<Poolable>().sourcePrefab = prefab;
        }

        objToGet.transform.position = position;
        objToGet.transform.rotation = rotation;
        objToGet.SetActive(true);

        activeCounts[prefab]++;

        return objToGet;
    }

    public void ReturnToPool(GameObject obj)
    {
        Poolable poolable = obj.GetComponent<Poolable>();
        if (poolable == null || poolable.sourcePrefab == null)
        {
            Debug.LogError(obj.name + " 오브젝트는 풀링이 불가능하거나 원본 프리팹 정보가 없습니다. 대신 파괴합니다.");
            Destroy(obj);
            return;
        }

        GameObject sourcePrefab = poolable.sourcePrefab;
        if (!poolDictionary.ContainsKey(sourcePrefab))
        {
            Debug.LogError("존재하지 않는 풀(" + sourcePrefab.name + ")에 오브젝트를 반납하려고 합니다.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[sourcePrefab].Enqueue(obj);

        activeCounts[sourcePrefab]--;
    }

    public int GetTotalActiveCount()
    {
        return activeCounts.Sum(x => x.Value);
    }
}