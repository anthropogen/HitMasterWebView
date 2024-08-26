using UnityEngine;
using UnityEngine.UI;

public class MainMenuPopup : MonoBehaviour
{
  [SerializeField] private Button playBtn;
  [SerializeField] private Button settingsBtn;

  public void Init(MenuSystem menuSystem, MenuScreen menuScreen)
  {
    playBtn.onClick.AddListener(menuSystem.ToTutorial);
    settingsBtn.onClick.AddListener(menuScreen.OpenSettings);
  }
}