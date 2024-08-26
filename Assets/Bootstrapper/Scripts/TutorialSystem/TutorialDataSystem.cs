using System.Collections.Generic;
using UnityEngine;

public class TutorialDataSystem
{
  private const string saveKey = "tutorial";
  private readonly HashSet<string> keys;
  private readonly TutorialData tutorialData;

  public TutorialDataSystem()
  {
    if (PlayerPrefs.HasKey(saveKey))
    {
      var json = PlayerPrefs.GetString(saveKey);
      tutorialData = JsonUtility.FromJson<TutorialData>(json);
    }
    else
    {
      tutorialData = new TutorialData();
    }

    keys = new HashSet<string>(tutorialData.CompletedNodes);
  }

  public void CompleteNode(string key)
  {
    keys.Add(key);
    tutorialData.CompletedNodes.Add(key);
    Save();
  }

  public bool HasKey(string key)
  {
    return keys.Contains(key);
  }

  public void Reset()
  {
    keys.Clear();
    tutorialData.CompletedNodes.Clear();
  }

  public void Save()
  {
    var json = JsonUtility.ToJson(tutorialData);
    PlayerPrefs.SetString(saveKey, json);
  }
}