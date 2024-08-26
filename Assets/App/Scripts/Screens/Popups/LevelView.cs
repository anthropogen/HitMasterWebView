using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
  [SerializeField] private Button btn;
  [SerializeField] private TMP_Text lvlTxt;

  public void Init(bool state, int level, int stars, Action<int> onClick)
  {
    btn.interactable = state;
    btn.onClick.AddListener(() => onClick?.Invoke(level));
    lvlTxt.text = $"{level + 1}";
  }

  public void UpdateState(bool state)
  {
    btn.interactable = state;
  }
}