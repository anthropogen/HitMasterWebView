using UnityEngine;
using UnityEngine.UI;
using Screen = Bootstrapper.UI.Screen;

public class TutorialScreen : Screen
{
  [SerializeField] private SimpleTutorialSystem tutorialSystem;
  [SerializeField] private Button backBtn;
  [SerializeField] private Button playBtn;

  protected override void Subscribe()
  {
    backBtn.onClick.AddListener(tutorialSystem.ToMenu);
    playBtn.onClick.AddListener(tutorialSystem.ToGame);
  }

  internal override void Unsubscribe()
  {
    backBtn.onClick.RemoveListener(tutorialSystem.ToMenu);
    playBtn.onClick.RemoveListener(tutorialSystem.ToGame);
  }
}