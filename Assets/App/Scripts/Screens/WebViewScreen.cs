using UnityEngine;
using Screen = Bootstrapper.UI.Screen;

public class WebViewScreen : Screen
{
  private string url;
  private UniWebView webView;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape)) GoBack();
  }
  
  public override void Close()
  {
    base.Close();
    if (webView == null)
      return;
    webView.Hide();
  }

  public void SetUrl(string url)
  {
    this.url = url;
    if (webView == null)
      webView = gameObject.AddComponent<UniWebView>();
    webView.OnOrientationChanged += OnOrientationChanged;
    webView.Frame = new Rect(0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height);
    webView.Show();
    webView.Load(url);
    webView.SetBackButtonEnabled(false);
  }

  private void OnOrientationChanged(UniWebView webView, ScreenOrientation orientation)
  {
    Debug.Log(UnityEngine.Screen.width);
    Debug.Log(UnityEngine.Screen.height);
    webView.Frame = new Rect(0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height);
  }

  private void GoBack()
  {
    var current = webView.Url;
    if (current == url)
      return;
    var canBack = webView.CanGoBack;
    if (canBack)
      webView.GoBack();
  }
}