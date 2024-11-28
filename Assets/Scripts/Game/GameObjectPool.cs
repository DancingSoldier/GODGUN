using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class GameObjectPool
{
    private readonly GameObject prefab;
    private readonly ObjectPool<GameObject> pool;

    private readonly int startSize, maxSize;

    public GameObjectPool(GameObject prefab, int startSize, int maxSize)
    {
        this.prefab = prefab;
        this.startSize = startSize;
        this.maxSize = maxSize;

        this.pool = new ObjectPool<GameObject>
            (
            CreatePooledObject,
            OnGetFromPool,
            OnReturnToPool,
            OnDestroyPooledObject,
            true,
            this.startSize,
            this.maxSize
            );
    }

    public GameObject GetObject(Vector3 position)
    {
        GameObject obj = pool.Get();
        obj.transform.position = position;
        return obj;
    }

    public void ReleaseObject(GameObject obj)
    {
        pool.Release(obj);
    }

    public void DestroyObject(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    private GameObject CreatePooledObject()
    {
        GameObject newObject = GameObject.Instantiate(prefab);

        return newObject;
    }

    // When object is taken from pool, activate it
    private void OnGetFromPool(GameObject pooledObject)
    {
        pooledObject.SetActive(true);
    }

    // When object is in turn returned, disactivate it
    private void OnReturnToPool(GameObject poooledObject)
    {
        poooledObject.SetActive(false);
    }

    // Destroy when discarding
    private void OnDestroyPooledObject(GameObject pooledObject)
    {
        GameObject.Destroy(pooledObject);
    }
}
