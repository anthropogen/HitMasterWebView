using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Screen = Bootstrapper.UI.Screen;

public class LoadingScreen : Screen
{
  [SerializeField] private Image progressBar;
  [SerializeField] [Range(0, 1)] private float endProgressValue = 0.8f;
  [SerializeField] private float progressDuration = 3f;

  private void OnEnable()
  {
    StartCoroutine(LoadingVisualization());
  }

  private IEnumerator LoadingVisualization()
  {
    var value = progressBar.fillAmount = 0;
    var speed = endProgressValue / progressDuration;
    while (progressBar.fillAmount < endProgressValue)
    {
      yield return null;
      value += Time.deltaTime * speed;
      progressBar.fillAmount = value;
    }
  }
}