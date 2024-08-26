using System.Collections.Generic;
using UnityEngine;

namespace Bootstrapper.Data
{
  [CreateAssetMenu(fileName = "GameBlueprint", menuName = "New GameBlueprint")]
  public class GameBlueprint : ScriptableObject
  {
    public List<LevelBlueprint> levels;
  }

  [System.Serializable]
  public class LevelBlueprint
  {
    public LevelConfig level;
    public float time;
  }
}