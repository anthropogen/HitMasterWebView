using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T template;
    private int capacity = 5;
    private Transform container;
    private HashSet<T> allObjects;

    public ObjectPool(T template, int capacity, Transform container)
    {
        this.template = template;
        if (capacity < 0) throw new ArgumentException("Capacity less than 0");
        this.capacity = capacity;
        this.container = container;
        InitialPool();
    }

    private void InitialPool()
    {
        allObjects = new HashSet<T>();
        for (int i = 0; i < capacity; i++)
        {
            var obj = CreateObj();
            obj.gameObject.SetActive(false);
        }
    }

    private T CreateObj()
    {
        var obj = GameObject.Instantiate(template, container);
        allObjects.Add(obj);
        return obj;
    }

    private bool TryGetObj(out T obj)
    {
        obj = allObjects.FirstOrDefault((p) => p.gameObject.activeSelf == false);
        return obj != null;
    }
    public T Get()
    {
        T obj;
        if (TryGetObj(out obj))
            return obj;
        return CreateObj();
    }
    public T GetAt(Vector3 point)
    {
        var obj = Get();
        obj.transform.position = point;
        obj.transform.parent = null;
        obj.transform.rotation = Quaternion.identity;
        obj.gameObject.SetActive(true);
        return obj;
    }
    public T GetAt(Vector3 point, Quaternion rotation)
    {
        var obj = GetAt(point);
        obj.transform.rotation = rotation;
        return obj;
    }
}
