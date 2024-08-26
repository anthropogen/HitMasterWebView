using UnityEngine;
using UnityEngine.UI;

public class MainMenuPopup : MonoBehaviour
{
  [SerializeField] private Button playBtn;
  [SerializeField] private Button settingsBtn;
  [SerializeField] private Button levelBtn;

  public void Init(MenuSystem menuSystem, MenuScreen menuScreen)
  {
    playBtn.onClick.AddListener(menuSystem.ToGame);
    settingsBtn.onClick.AddListener(menuScreen.OpenSettings);
    levelBtn.onClick.AddListener(menuScreen.OpenLevels);
  }
}