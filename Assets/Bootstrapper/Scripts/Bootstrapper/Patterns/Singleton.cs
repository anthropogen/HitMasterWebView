using UnityEngine;

namespace Bootstrapper.Patterns
{
  public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    public static T Instance { get; private set; }

    private void Awake()
    {
      if (Instance != null)
        Destroy(gameObject);
      Instance = this as T;
      OnAwake();
    }

    protected virtual void OnAwake()
    {
    }
  }
}