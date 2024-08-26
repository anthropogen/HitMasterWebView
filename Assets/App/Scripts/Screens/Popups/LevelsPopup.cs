using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopup : MonoBehaviour
{
  [SerializeField] private Button backBtn;
  [SerializeField] private LevelView template;
  [SerializeField] private Transform container;
  private List<LevelView> levels;
  private MenuSystem menu;
  private MenuScreen menuScreen;

  public void Init(MenuSystem menu, MenuScreen menuScreen)
  {
    this.menu = menu;
    this.menuScreen = menuScreen;
    backBtn.onClick.AddListener(menuScreen.OpenMain);
    levels = new();
    var opened = menu.PlayerData.levels.Count + 1;

    for (var i = 0; i < menu.Blueprint.levels.Count; i++)
    {
      var lvl = menu.Blueprint.levels[i];
      var viewLvl = Instantiate(template, container);
      viewLvl.Init(i < opened, i, 0, OnClickStartLevel);
      levels.Add(viewLvl);
    }
  }

  private void OnClickStartLevel(int i)
  {
    var lvl = menu.Blueprint.levels[i];
    menu.GameData.num = i;
    menu.GameData.Current = lvl;
    menu.ToGame();
  }

  public void UpdateState()
  {
    var opened = menu.PlayerData.levels.Count + 1;
    for (var i = 0; i < levels.Count; i++)
    {
      var lvlView = levels[i];
      lvlView.UpdateState(i < opened);
    }
  }
}