using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class ObjectPool<T> : MonoBehaviour where T : IPoolable<T>
{
    private readonly List<T> _objectsPool;

    [Header("Pool Settings")]
    [SerializeField] private int _poolSize = 8;
    [SerializeField] private int _poolExpansionAmount = 4;
    [SerializeField] private Transform _container;
    [SerializeField] private List<T> _objectsCopy;

    protected ObjectPool()
    {
        _objectsPool = new List<T>(_poolSize);
    }

    private void Awake()
    {
        InitializeAwake();
        
        if (_container == null)
        {
            CreateContainer();
        }
        
        FillPool(_poolSize);
    }

    private void Start()
    {
        InitializeStart();
    }
    
    protected virtual void InitializeAwake()
    {
    }
    
    protected virtual void InitializeStart()
    {
    }

    protected T EnableCopy()
    {
        var objectCopy = GetInactiveObject();
        objectCopy.GetGameObject().SetActive(false);
        return objectCopy;
    }

    protected T EnableCopy(Vector3 position)
    {
        var objectCopy = GetInactiveObject();
        var gameObjectCopy = objectCopy.GetGameObject();
        gameObjectCopy.transform.position = position;
        gameObjectCopy.SetActive(true);
        return objectCopy;
    }

    protected T EnableCopy(Vector3 position, Quaternion rotation)
    {
        var objectCopy = GetInactiveObject();
        var gameObjectCopy = objectCopy.GetGameObject();
        gameObjectCopy.transform.position = position;
        gameObjectCopy.transform.rotation = rotation;
        gameObjectCopy.SetActive(true);
        return objectCopy;
    }

    protected T EnableCopy(Vector3 position, Quaternion rotation, Transform parent)
    {
        var objectCopy = GetInactiveObject();
        var gameObjectCopy = objectCopy.GetGameObject();
        gameObjectCopy.transform.position = position;
        gameObjectCopy.transform.rotation = rotation;
        gameObjectCopy.transform.parent = parent;
        gameObjectCopy.SetActive(true);
        return objectCopy;
    }

    protected T EnableCopy(Vector3 position, Quaternion rotation, Func<T, bool> filter)
    {
        var objectCopy = GetInactiveObject(filter);
        var gameObjectCopy = objectCopy.GetGameObject();
        gameObjectCopy.transform.position = position;
        gameObjectCopy.transform.rotation = rotation;
        gameObjectCopy.SetActive(true);
        return objectCopy;
    }

    protected T EnableCopy(Vector3 position, Quaternion rotation, Transform parent, Func<T, bool> filter)
    {
        var objectCopy = GetInactiveObject(filter);
        var gameObjectCopy = objectCopy.GetGameObject();
        gameObjectCopy.transform.position = position;
        gameObjectCopy.transform.rotation = rotation;
        gameObjectCopy.transform.parent = parent;
        gameObjectCopy.SetActive(true);
        return objectCopy;
    }

    protected virtual void DisableCopy(T copy)
    {
        copy.GetGameObject().SetActive(false);
    }

    private void CreateContainer()
    {
        _container = new GameObject($"{typeof(T)} container").transform;
        _container.parent = transform;
    }

    private T GetNewObject(T copy)
    {
        var objectCopy = Object.Instantiate(copy.GetGameObject().gameObject, _container);
        objectCopy.gameObject.SetActive(false);
        return objectCopy.GetComponent<T>();
    }

    private T GetInactiveObject(Func<T, bool> filter = null)
    {
        if (GetInactiveObjectsCount(filter) <= 0)
        {
            ExpandPool(_poolExpansionAmount, filter);
        }
        
        return _objectsPool
            .Where(filter ?? (copy => true))
            .First(copy => copy.GetActiveSelf() == false);
    }

    private int GetInactiveObjectsCount(Func<T, bool> filter = null)
    {
        
        int count = _objectsPool
            .Where(filter ?? (copy => true))
            .Count(copy => copy.GetActiveSelf() == false);

        return count;
    }

    private void ExpandPool(int amount, Func<T, bool> filter = null)
    {
        _poolSize += amount;

        for (int i = 0; i < amount; i++)
        {
            var filteredCopies = _objectsCopy.Where(filter ?? (copy => true));

            foreach (var filteredCopy in (IEnumerable) filteredCopies)
            {
                AddToPool(GetNewObject((T) filteredCopy));
            }
        }
    }

    private void FillPool(int size)
    {
        foreach (var copy in _objectsCopy)
        {
            for (int i = 0; i < size; i++)
            {
                AddToPool(GetNewObject(copy));
            }
        }
    }

    private void AddToPool(T copy)
    {
        _objectsPool.Add(copy);
        copy.OnDisabled += DisableCopy;
    }

    private void RemoveFromPool(T copy)
    {
        copy.OnDisabled -= DisableCopy;
        _objectsPool.Remove(copy);
        Destroy(copy.GetGameObject());
    }
}

