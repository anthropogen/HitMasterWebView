using System;
using Bootstrapper.Data;
using Bootstrapper.Patterns;
using Bootstrapper.StateMachine;
using Bootstrapper.UI;
using UnityEngine;

namespace Bootstrapper
{
  [DefaultExecutionOrder(-199)]
  public sealed class HyperBootstrapper : Singleton<HyperBootstrapper>
  {
    [SerializeField] private GameStateMachine gameStateMachine;
    [SerializeField] private GameBlueprint blueprint;
    [SerializeField] private UIScreens uiScreens;
    [SerializeField] private DataDebugger dataDebugger;
    private DataService dataService;
    public GameStateID CurrentState => gameStateMachine.CurrentStateID;

    private void OnDestroy()
    {
      dataService.Save();
      gameStateMachine.EndGame();
    }


    protected override void OnAwake()
    {
      ServiceLocator.Register(new DataService());
      dataService = ServiceLocator.Get<DataService>();
      var playerdata = dataDebugger.Debug ? dataDebugger.PlayerData : dataService.PlayerData;
      uiScreens.Setup(gameStateMachine);
      gameStateMachine.Init(playerdata, dataService.GameData, blueprint);
    }

    public void ChangeState(GameStateID next)
    {
      gameStateMachine.ChangeState(next);
    }

    internal void Subscribe(Action loop, Action lateLoop, Action physicsLoop)
    {
      gameStateMachine.Loop += loop;
      gameStateMachine.LateLoop += lateLoop;
      gameStateMachine.PhysicsLoop += physicsLoop;
    }

    internal void Unsubscribe(Action loop, Action lateLoop, Action physicsLoop)
    {
      gameStateMachine.Loop -= loop;
      gameStateMachine.LateLoop -= lateLoop;
      gameStateMachine.PhysicsLoop -= physicsLoop;
    }

    public void Save()
    {
      dataService.Save();
    }
  }
}