using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bootstrapper.StateMachine
{
  internal sealed class StatesInstaller : MonoBehaviour
  {
    [field: SerializeField] public GameStateID FitstStateID { get; private set; }

    internal Dictionary<GameStateID, GameState> Setup()
    {
      var result = new Dictionary<GameStateID, GameState>();
      result = transform.GetComponentsInChildren<GameState>().ToDictionary(s => s.StateID);
      return result;
    }
  }
}