using Bootstrapper.Data;
using NaughtyAttributes;
using UnityEngine;

public class DataDebugger : MonoBehaviour
{
  [field: SerializeField] public bool Debug { get; private set; }

  [field: SerializeField]
  [field: ShowIf(nameof(Debug))]
  public PlayerData PlayerData { get; private set; }
}