using Bootstrapper.StateMachine;
using UnityEngine;

namespace Bootstrapper.UI
{
  public class UIScreens : MonoBehaviour
  {
    private Screen[] allScreens;
    private GameStateMachine gameStateMachine;

    private void OnDisable()
    {
      gameStateMachine.StateChanged -= TryOpenScreensWithState;
      for (var i = 0; i < allScreens.Length; i++) allScreens[i].Unsubscribe();
    }

    internal void Setup(GameStateMachine gameStateMachine)
    {
      allScreens = GetComponentsInChildren<Screen>();
      for (var i = 0; i < allScreens.Length; i++) allScreens[i].Setup();

      this.gameStateMachine = gameStateMachine;
      gameStateMachine.StateChanged += TryOpenScreensWithState;
    }

    private void TryOpenScreensWithState(GameStateID stateID)
    {
      for (var i = 0; i < allScreens.Length; i++) allScreens[i].TryOpenWithState(stateID);
    }
  }
}