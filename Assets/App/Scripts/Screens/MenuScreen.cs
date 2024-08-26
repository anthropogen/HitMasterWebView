using UnityEngine;
using Screen = Bootstrapper.UI.Screen;

public class MenuScreen : Screen
{
  [SerializeField] private MenuSystem menuSystem;
  [SerializeField] private SettingsPopup settingsPopup;
  [SerializeField] private MainMenuPopup mainPopup;

  protected override void Subscribe()
  {
    settingsPopup.Init(SetMain);
    mainPopup.Init(menuSystem, this);
  }

  public override void Open()
  {
    base.Open();
    SetMain();
  }

  public void OpenSettings()
  {
    mainPopup.gameObject.SetActive(false);
    settingsPopup.gameObject.SetActive(true);
  }

  private void SetMain()
  {
    mainPopup.gameObject.SetActive(true);
    settingsPopup.gameObject.SetActive(false);
  }
}