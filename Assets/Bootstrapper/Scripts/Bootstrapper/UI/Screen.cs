using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Bootstrapper.UI
{
  public class Screen : MonoBehaviour
  {
    [SerializeField] private bool openWithStates;
    [SerializeField] private bool getScreenFromChild = true;

    [ShowIf(nameof(openWithStates))] [SerializeField]
    private GameStateID[] statesToOpenWith;

    [HideIf(nameof(getScreenFromChild))] [SerializeField]
    private GameObject screen;

    private HashSet<GameStateID> states;

    public void Setup()
    {
      states = new HashSet<GameStateID>(statesToOpenWith);
      screen = getScreenFromChild ? transform.GetChild(0).gameObject : screen;
      Subscribe();
    }

    public virtual void Open()
    {
      screen.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
      screen.gameObject.SetActive(false);
    }

    public void TryOpenWithState(GameStateID stateID)
    {
      if (openWithStates && states.Contains(stateID))
        Open();
      else
        Close();
    }

    protected virtual void Subscribe()
    {
    }

    internal virtual void Unsubscribe()
    {
    }
  }
}