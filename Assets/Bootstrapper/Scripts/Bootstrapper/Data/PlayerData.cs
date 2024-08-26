using System;
using System.Collections.Generic;

namespace Bootstrapper.Data
{
  [Serializable]
  public class PlayerData
  {
    /// Player data for saving
    public string URL;

    public float volume = 1f;

    public List<LevelDto> levels = new();
  }

  [System.Serializable]
  public class LevelDto
  {
    public int num;
    public float time;
    public float stars;
  }
}