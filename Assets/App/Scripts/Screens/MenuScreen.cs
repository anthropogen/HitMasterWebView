using UnityEngine;
using Screen = Bootstrapper.UI.Screen;

public class MenuScreen : Screen
{
  [SerializeField] private MenuSystem menuSystem;
  [SerializeField] private SettingsPopup settingsPopup;
  [SerializeField] private MainMenuPopup mainPopup;
  [SerializeField] private LevelsPopup levelsPopup;

  public void Init()
  {
    settingsPopup.Init(OpenMain);
    mainPopup.Init(menuSystem, this);
    levelsPopup.Init(menuSystem, this);
  }

  public override void Open()
  {
    base.Open();
    OpenMain();
  }

  public void OpenSettings()
  {
    mainPopup.gameObject.SetActive(false);
    settingsPopup.gameObject.SetActive(true);
    levelsPopup.gameObject.SetActive(false);
  }

  public void OpenMain()
  {
    mainPopup.gameObject.SetActive(true);
    settingsPopup.gameObject.SetActive(false);
    levelsPopup.gameObject.SetActive(false);
  }

  public void OpenLevels()
  {
    mainPopup.gameObject.SetActive(false);
    settingsPopup.gameObject.SetActive(false);
    levelsPopup.gameObject.SetActive(true);
    levelsPopup.UpdateState();
  }
}