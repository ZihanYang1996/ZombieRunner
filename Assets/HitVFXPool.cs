using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HitVFXPool : MonoBehaviour
{
    static HitVFXPool instance;
    public static HitVFXPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HitVFXPool>();
                if (instance == null)
                {
                    Debug.LogError("No Hit VFX Pool instance found in scene. Adding a new one.");
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<HitVFXPool>();
                    obj.name = "Hit VFX Pool";
                }
            }
            return instance;
        }
    }

    [SerializeField] GameObject hitVFXPrefab;
    [SerializeField] int defaultPoolSize = 10;
    [SerializeField] int maxPoolSize = 20;
    [SerializeField] bool collectionChecks = true;

    IObjectPool<GameObject> pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (pool == null)
            {
                Debug.Log("Creating Hit VFX Pool. Upon request.");
                pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, defaultPoolSize, maxPoolSize);
            }
            return pool;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Destroying duplicate Hit VFX Pool instance.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Debug.Log("First Hit VFX Pool instance created.");
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    GameObject CreatePooledItem()
    {
        GameObject newHitVFX = Instantiate(hitVFXPrefab);
        newHitVFX.transform.SetParent(transform);
        newHitVFX.name = "Hit VFX Instance";
        return newHitVFX;
    }

    void OnTakeFromPool(GameObject item)
    {
        item.SetActive(true);
    }

    void OnReturnedToPool(GameObject item)
    {
        item.SetActive(false);
    }

    void OnDestroyPoolObject(GameObject item)
    {
        Destroy(item);
    }

}
