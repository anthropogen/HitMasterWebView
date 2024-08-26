using Bootstrapper.Patterns;
using UnityEngine;

namespace Bootstrapper.Data
{
  public class DataService : IService
  {
    private readonly string key = "playerData";
    internal GameData GameData = new();
    internal PlayerData PlayerData;

    public DataService()
    {
      Load();
    }

    private PlayerData Load()
    {
      PlayerData = new PlayerData();
      if (!PlayerPrefs.HasKey(key)) return PlayerData;

      var jsonData = PlayerPrefs.GetString(key);
      PlayerData = JsonUtility.FromJson<PlayerData>(jsonData);
      return PlayerData;
    }

    public void Save()
    {
      var data = JsonUtility.ToJson(PlayerData);
      PlayerPrefs.SetString(key, data);
      PlayerPrefs.Save();
    }
  }
}