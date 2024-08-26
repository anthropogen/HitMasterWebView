using UnityEngine;
using UnityEngine.UI;
using Screen = Bootstrapper.UI.Screen;

public class ResultScreen : Screen
{
  [SerializeField] private Button menuBtn;
  [SerializeField] private Button playBtn;
  [SerializeField] private ResultSystem resultSystem;

  protected override void Subscribe()
  {
    menuBtn.onClick.AddListener(resultSystem.ToMenu);
    playBtn.onClick.AddListener(resultSystem.ToGame);
  }

  internal override void Unsubscribe()
  {
    menuBtn.onClick.RemoveListener(resultSystem.ToMenu);
    playBtn.onClick.RemoveListener(resultSystem.ToGame);
  }
}