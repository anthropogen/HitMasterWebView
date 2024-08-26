using System;
using UnityEngine;

[Serializable]
public abstract class TutorialStep
{
  [field: SerializeField] public string Description { get; private set; }
  protected TutorialSystem tutorialSystem;
  public event Action Completed;

  public abstract void OnUpdate();
  protected abstract void OnComplete();
  protected abstract void OnBegin();

  public void Begin(TutorialSystem tutorialSystem)
  {
    this.tutorialSystem = tutorialSystem;
    OnBegin();
  }

  public void Complete()
  {
    OnComplete();
    Completed?.Invoke();
  }
}