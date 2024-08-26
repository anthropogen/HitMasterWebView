using System;
using System.Linq;
using UnityEngine;

public class TutorialSystem : GameEntity
{
  [SerializeField] private TutorialNode[] nodes;
  private TutorialDataSystem dataSystem;
  private TutorialNode node;
  private bool shouldUpdate;
  private int stepIndex;
  public event Action<TutorialStep> StepBegan;
  public event Action<TutorialStep> StepCompleted;

  public void Begin()
  {
    dataSystem = new TutorialDataSystem();
    if (!TutorialComleted())
    {
      SelectNode();
      BeginStep();
    }
  }

  protected override void Loop()
  {
    if (shouldUpdate && node != null)
      node.Steps[stepIndex].OnUpdate();
  }

  private bool TutorialComleted()
  {
    return nodes.Where(n => !dataSystem.HasKey(n.Key)).Count() == 0;
  }

  private void SelectNode()
  {
    for (var i = 0; i < nodes.Length; i++)
      if (!dataSystem.HasKey(nodes[i].Key))
      {
        node = nodes[i];
        return;
      }
  }

  private void BeginStep()
  {
    if (TutorialComleted()) return;
    node.Steps[stepIndex].Completed += CompleteAndContinue;
    node.Steps[stepIndex].Begin(this);
    shouldUpdate = true;
    StepBegan?.Invoke(node.Steps[stepIndex]);
  }

  private void CompleteAndContinue()
  {
    CompleteStep();
    BeginStep();
  }

  private void CompleteStep()
  {
    ExitStep();
    StepCompleted?.Invoke(node.Steps[stepIndex]);
    stepIndex++;
    if (stepIndex >= nodes.Length)
    {
      stepIndex = 0;
      dataSystem.CompleteNode(node.Key);
      SelectNode();
    }
  }

  private void ExitStep()
  {
    node.Steps[stepIndex].Completed -= CompleteAndContinue;
    shouldUpdate = false;
  }
}