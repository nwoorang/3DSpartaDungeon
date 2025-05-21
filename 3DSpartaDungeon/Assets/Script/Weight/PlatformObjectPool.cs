using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class PlatformObjectPool : Singleton<PlatformObjectPool>
{
    public GameObject[] Prefabs;

    private Dictionary<string, ObjectPool<GameObject>> pools = new Dictionary<string, ObjectPool<GameObject>>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var prefab in Prefabs)
        {
            var pool = new ObjectPool<GameObject>(
                 () =>
                 {
                     var obj = Instantiate(prefab);
                     obj.transform.SetParent(this.transform);
                     return obj;
                 },
                 obj => obj.SetActive(true),
                 obj => obj.SetActive(false),
                 obj => Destroy(obj),
                 false, 10, 100
             );
            pools.Add(prefab.name, pool);
        }
    }


    public GameObject Get(string prefabName)
    {
        if (pools.TryGetValue(prefabName, out var pool))
        {
            GameObject temp = pool.Get();
            return temp;

        }
        return null;
    }

    public void ReleaseDelayed(string prefabName, GameObject obj, float delay)
    {
        StartCoroutine(ReleaseAfterDelay(prefabName, obj, delay));
    }

    private IEnumerator ReleaseAfterDelay(string prefabName, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Release(prefabName, obj);
    }


    public void Release(string prefabName, GameObject obj)
    {
        if (pools.TryGetValue(prefabName, out var pool))
        {
            pool.Release(obj);
        }
        else
        {
            Destroy(obj);
        }
    }

    public void AllObjectOff()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
            pools[child.gameObject.name.Substring(0, child.gameObject.name.Length - "(Clone)".Length)].Release(child.gameObject);
        }

    }
}
