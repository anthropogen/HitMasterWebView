using System;
using UnityEngine;
using UnityEngine.UI;

public class PolicyPopup : MonoBehaviour
{
  [SerializeField] private Button backBtn;
  
  public void Init(Action onBackClick)
  {
    backBtn.onClick.AddListener(() => onBackClick?.Invoke());
  }
}