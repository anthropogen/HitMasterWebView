using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
  [SerializeField] private Button backBtn;

  public void Init(Action onBackClick)
  {
    backBtn.onClick.AddListener(() => onBackClick?.Invoke());
  }
}