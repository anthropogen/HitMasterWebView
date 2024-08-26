using UnityEngine;

[CreateAssetMenu(fileName = "newTutorialNode", menuName = "Tutorial/Node", order = 51)]
public class TutorialNode : ScriptableObject
{
  [field: SerializeField] public string Key { get; private set; }

  [field: SerializeReference]
  [field: SubclassSelector]
  public TutorialStep[] Steps { get; private set; }
}