using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bootstrapper.Patterns
{
  public class ComponentPool<T> where T : Component
  {
    private readonly Transform container;
    private readonly HashSet<T> pool = new();
    private readonly T template;

    public ComponentPool(T template, int capacity, Transform container)
    {
      if (capacity <= 0) throw new ArgumentException();
      this.template = template;
      this.container = container;
      CreatePool(capacity);
    }

    public T Get()
    {
      var obj = pool.FirstOrDefault(o => o.gameObject.activeSelf == false);
      if (obj == null)
        obj = CreateObject();
      obj.gameObject.SetActive(true);
      return obj;
    }

    public T GetAt(Vector3 position)
    {
      var obj = Get();
      obj.transform.position = position;
      return obj;
    }

    public T GetAt(Vector3 position, Quaternion rotation)
    {
      var obj = GetAt(position);
      obj.transform.rotation = rotation;
      return obj;
    }

    private void CreatePool(float capacity)
    {
      for (var i = 0; i < capacity; i++) CreateObject();
    }

    private T CreateObject()
    {
      var obj = Object.Instantiate(template, container);
      pool.Add(obj);
      obj.gameObject.SetActive(false);
      return obj;
    }
  }
}