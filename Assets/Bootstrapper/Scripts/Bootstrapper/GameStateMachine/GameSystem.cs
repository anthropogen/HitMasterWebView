using Bootstrapper.Data;
using UnityEngine;

namespace Bootstrapper.StateMachine
{
  public abstract class GameSystem : MonoBehaviour
  {
    public GameData GameData { get; private set; }
    public PlayerData PlayerData { get; private set; }
    public GameBlueprint Blueprint { get; private set; }

    public void InjectData(PlayerData playerData, GameData gameData, GameBlueprint blueprint)
    {
      PlayerData = playerData;
      GameData = gameData;
      Blueprint = blueprint;
    }

    public void ToTutorial()
    {
      HyperBootstrapper.Instance.ChangeState(GameStateID.Tutorial);
    }

    public void ToGame()
    {
      HyperBootstrapper.Instance.ChangeState(GameStateID.Game);
    }

    public void ToResult()
    {
      HyperBootstrapper.Instance.ChangeState(GameStateID.Result);
    }

    public void ToMenu()
    {
      HyperBootstrapper.Instance.ChangeState(GameStateID.Menu);
    }

    internal virtual void Init()
    {
    }

    internal virtual void EnterState()
    {
    }

    internal virtual void Loop()
    {
    }

    internal virtual void LateLoop()
    {
    }

    internal virtual void PhysicsLoop()
    {
    }

    internal virtual void ExitState()
    {
    }

    internal virtual void EndGame()
    {
    }
  }
}