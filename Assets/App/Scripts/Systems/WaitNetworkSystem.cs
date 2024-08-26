using Bootstrapper;
using Bootstrapper.StateMachine;
using UnityEngine;

public class WaitNetworkSystem : GameSystem
{
  internal override void Loop()
  {
    if (Application.internetReachability != NetworkReachability.NotReachable)
      HyperBootstrapper.Instance.ChangeState(GameStateID.Loading);
  }
}