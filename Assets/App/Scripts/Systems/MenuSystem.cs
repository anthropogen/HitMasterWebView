using Bootstrapper.StateMachine;
using UnityEngine;

public class MenuSystem : GameSystem
{
  [SerializeField] private MenuScreen menuScreen;

  internal override void Init()
  {
    menuScreen.Init();
  }
}