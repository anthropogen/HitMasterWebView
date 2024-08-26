using Bootstrapper;
using Bootstrapper.Data;
using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
  protected GameData GameData;
  protected PlayerData PlayerData;
  protected GameStateID CurrentGameState => HyperBootstrapper.Instance.CurrentState;

  private void OnEnable()
  {
    HyperBootstrapper.Instance.Subscribe(Loop, LateLoop, PhysicsLoop);
    Enable();
  }

  private void OnDisable()
  {
    HyperBootstrapper.Instance.Unsubscribe(Loop, LateLoop, PhysicsLoop);
    Disable();
  }

  protected void Enable()
  {
  }

  protected void Disable()
  {
  }

  protected virtual void Loop()
  {
  }

  protected virtual void LateLoop()
  {
  }

  protected virtual void PhysicsLoop()
  {
  }
}