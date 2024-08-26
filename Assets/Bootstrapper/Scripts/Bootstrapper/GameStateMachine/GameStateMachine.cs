using System;
using System.Collections.Generic;
using Bootstrapper.Data;
using UnityEngine;

namespace Bootstrapper.StateMachine
{
  [DefaultExecutionOrder(-198)]
  internal sealed class GameStateMachine : MonoBehaviour
  {
    [SerializeField] private StatesInstaller statesInstaller;
    private GameState alwaysRunState;
    private GameState currentState;
    private Dictionary<GameStateID, GameState> states;
    internal GameStateID CurrentStateID => currentState.StateID;

    private void Update()
    {
      currentState?.Loop();
      alwaysRunState?.Loop();
      Loop?.Invoke();
    }

    private void FixedUpdate()
    {
      currentState?.PhysicsLoop();
      alwaysRunState?.PhysicsLoop();
      PhysicsLoop?.Invoke();
    }

    private void LateUpdate()
    {
      currentState?.LateLoop();
      alwaysRunState?.LateLoop();
      LateLoop?.Invoke();
    }

    internal event Action Loop;
    internal event Action LateLoop;
    internal event Action PhysicsLoop;
    internal event Action<GameStateID> StateChanged;

    internal void Init(PlayerData playerData, GameData gameData, GameBlueprint blueprint)
    {
      states = statesInstaller.Setup();
      foreach (var state in states.Values) state.Init(playerData, gameData, blueprint);
      if (states.ContainsKey(GameStateID.AlwaysRun))
        alwaysRunState = states[GameStateID.AlwaysRun];
      ChangeState(statesInstaller.FitstStateID);
    }

    internal void ChangeState(GameStateID stateID)
    {
      if (stateID == GameStateID.AlwaysRun)
      {
        Debug.LogError($"Can't transit to this state {GameStateID.AlwaysRun}");
        return;
      }

      GameState next = null;
      if (states.ContainsKey(stateID))
        next = states[stateID];
      if (currentState != null)
        currentState.Exit();
      if (next != null)
      {
        currentState = next;
        Debug.Log($"<color=orange>Transition to {stateID}</color>");
        StateChanged?.Invoke(next.StateID);
        currentState.Enter();
      }
    }

    internal void EndGame()
    {
      currentState?.EndGame();
      alwaysRunState?.EndGame();
    }
  }
}