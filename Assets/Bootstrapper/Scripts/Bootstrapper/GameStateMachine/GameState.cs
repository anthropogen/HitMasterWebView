using Bootstrapper.Data;
using UnityEngine;

namespace Bootstrapper.StateMachine
{
  internal sealed class GameState : MonoBehaviour
  {
    [field: SerializeField] internal GameStateID StateID { get; private set; }
    private GameSystem[] systems;

    internal void Init(PlayerData playerData, GameData gameData, GameBlueprint blueprint)
    {
      systems = GetComponentsInChildren<GameSystem>();
      for (var i = 0; i < systems.Length; i++)
      {
        systems[i].InjectData(playerData, gameData, blueprint);
        systems[i].Init();
      }
    }

    internal void Enter()
    {
      for (var i = 0; i < systems.Length; i++) systems[i].EnterState();
    }

    internal void Loop()
    {
      for (var i = 0; i < systems.Length; i++) systems[i].Loop();
    }

    internal void LateLoop()
    {
      for (var i = 0; i < systems.Length; i++) systems[i].LateLoop();
    }

    internal void PhysicsLoop()
    {
      for (var i = 0; i < systems.Length; i++) systems[i].PhysicsLoop();
    }

    internal void Exit()
    {
      for (var i = 0; i < systems.Length; i++) systems[i].ExitState();
    }

    internal void EndGame()
    {
      for (var i = 0; i < systems.Length; i++) systems[i].EndGame();
    }
  }
}